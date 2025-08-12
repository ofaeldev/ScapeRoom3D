using FSM;
using PlayerInput;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;
using UnityEngine.Windows;

public class PlayerContext : MonoBehaviour
{
    [Header("Componentes")]
    public Rigidbody rb;

    [Header("Configurações")]
    public float jumpForce = 5f;
    public float groundCheckRadius = 0.2f;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public Animator animator;
    public float rotationSensitivity;

    [Header("Movement System")]
    [HideInInspector] public Vector2 moveInput;
    [HideInInspector] public float currentSpeed; 

    [Header("Turn System")]
    [HideInInspector] public bool turnLeftInput; //botão de giro para a esquerda pressionado (padrão - Q )
    [HideInInspector] public bool turnRightInput; //botão de giro para a direita pressionado (padrão - E )
    [HideInInspector] public bool isTurningInPlace;
    [HideInInspector] public bool waitingToEnterTurnState;

    [Header("Inventory System")]
    [HideInInspector] public bool inventoryInput; //botão de inventory pressionado (padrão - I)
    [HideInInspector] public bool interactInput; //botão de interação pressionado (padrão - F)
    [HideInInspector] public bool exitInteractInput;

    [Header("Jump System")]
    [HideInInspector] public bool jumpInput; //botão de pulo pressionado (padrão - espaço)
    [HideInInspector] public bool isGrounded; //detecta o jogador no chão

    [Header("Fall System")]
    public float lastGroundedY;      // Altura no momento em que estava no chão
    public float fallStartThreshold = 0.3f; // Altura mínima para considerar uma queda real
    public float timeSinceLeftGround;
    public float minFallTime = 0.1f;
    public float minFallSpeed = -1.5f;
    public bool wasGroundedLastFrame; // Para detectar a transição
    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float lowJumpMultiplier = 2f;

    [Header("Sprint System")]
    [HideInInspector] public bool sprintInput = false;
    
    [HideInInspector] public Vector2 lookInput;

    public UIFeedback uiFeedbackPanel;


    [Header("Estados da FSM")]
    public State<PlayerContext> idleState;
    public State<PlayerContext> moveState;
    public State<PlayerContext> jumpState;
    public State<PlayerContext> fallingState;
    public State<PlayerContext> interactState;
    public State<PlayerContext> menuState;
    public State<PlayerContext> sprintState;

    [Header("Instance")]
    [HideInInspector]public static PlayerContext Instance { get; private set; }

    private StateMachine<PlayerContext> stateMachine;

    [HideInInspector] public InputBlocker inputBlocker;
    [HideInInspector] public InteractionDetector interactionDetector;
    [HideInInspector] public InventoryUI inventoryUI;
    [HideInInspector] public IInteractable CurrentInteractable;
    [HideInInspector] public PlayerAnimatorController playerAnimatorController;
    [HideInInspector]public PlayerInputReader inputReader;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        stateMachine = new StateMachine<PlayerContext>(this, idleState);
    }

    #region MonoBehavior
    private void Awake()
    {
        inventoryUI = FindFirstObjectByType<InventoryUI>();
        inputBlocker = GetComponent<InputBlocker>();
        interactionDetector = GetComponent<InteractionDetector>();
        playerAnimatorController = GetComponent<PlayerAnimatorController>();
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = GetComponentInChildren<Rigidbody>();
            if (rb == null)
                Debug.LogError("Rigidbody não encontrado no PlayerContext ou filhos! Verifique o objeto do jogador.");
        }

        inputReader = GetComponent<PlayerInputReader>();
        if (inputReader == null)
            Debug.LogError("PlayerInputReader não encontrado no PlayerContext! Verifique o objeto do jogador.");

        if (idleState == null || moveState == null || jumpState == null || fallingState == null || interactState == null)
            Debug.LogWarning("Estados da FSM não atribuídos no Inspector.");
    }
    private void Update()
    {
        CheckGrounded();
        CheckFalling();
        stateMachine.Tick();
        jumpInput = false;
        interactInput = false;
        inventoryInput = false;
        
    }

    private void FixedUpdate()
    {
        stateMachine.FixedTick();
    }
    #endregion
    private void OnEnable()
    {
        if (inputReader != null)
        {
            inputReader.OnMove += HandleMove;
            inputReader.OnJump += HandleJump;
            inputReader.OnInteract += HandleInteract;
            inputReader.OnExitInteract += HandleExitInteract;
            inputReader.OnLook += HandleLook;
            inputReader.OnInventory += HandleInventory;
            inputReader.OnSprintPressed += HandleSprintPressed;
            inputReader.OnTurnLeft += HandleTurnLeft;
            inputReader.OnTurnRight += HandleTurnRight;
        }
    }

    private void OnDisable()
    {
        if (inputReader != null)
        {
            inputReader.OnMove -= HandleMove;
            inputReader.OnJump -= HandleJump;
            inputReader.OnInteract -= HandleInteract;
            inputReader.OnExitInteract -= HandleExitInteract;
            inputReader.OnLook -= HandleLook;
            inputReader.OnInventory -= HandleInventory;
            inputReader.OnSprintPressed -= HandleSprintPressed;
            inputReader.OnTurnLeft -= HandleTurnLeft;
            inputReader.OnTurnRight -= HandleTurnRight;
        }
    }


    #region Inputs
    private void HandleTurnLeft()
    {
        turnLeftInput = true;
    }
    private void HandleTurnRight()
    {
        turnRightInput = true;
    }
    private void HandleSprintPressed()
    {
        sprintInput = !sprintInput;
    }

    private void HandleInventory()
    {
        inventoryInput = true;
    }

    private void HandleExitInteract()
    {
        exitInteractInput = true;
    }

    private void HandleLook(Vector2 look)
    {
        lookInput = look;
    }

    private void HandleMove(Vector2 move)
    {
        moveInput = move;
    }

    private void HandleJump()
    {
        jumpInput = true;
    }

    private void HandleInteract()
    {
        interactInput = true;
    }
    #endregion
    private void CheckFalling()
    {
        if (rb.linearVelocity.y < 0)
        {
            // Está caindo
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if (rb.linearVelocity.y > 0 && !jumpInput)
        {
            // Pulou e soltou o botão – faz o pulo mais curto
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }

        if (isGrounded)
        {
            lastGroundedY = transform.position.y;
            timeSinceLeftGround = 0f;
        }
        else
        {
            timeSinceLeftGround += Time.deltaTime;
        }

        wasGroundedLastFrame = isGrounded;
    }

    private void CheckGrounded()
    {
        if (groundCheck == null)
        {
            Debug.LogWarning("GroundCheck não atribuído.");
            isGrounded = false;
            return;
        }

        float extraHeight = 0.05f; // margem para tolerância
        Ray ray = new Ray(groundCheck.position, Vector3.down);

        // 1️⃣ Raycast — detecta contato direto
        bool rayHit = Physics.Raycast(ray, groundCheckRadius + extraHeight, groundLayer);

        // 2️⃣ SphereCast — detecta em rampas e bordas
        bool sphereHit = Physics.SphereCast(groundCheck.position, groundCheckRadius, Vector3.down, out RaycastHit hitInfo, extraHeight, groundLayer);

        // 3️⃣ Combina resultados e evita bug quando estamos subindo rápido
        isGrounded = (rayHit || sphereHit) && rb.linearVelocity.y <= 0.1f;

        // Debug para visualizar
        Color color = isGrounded ? Color.green : Color.red;
        Debug.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * (groundCheckRadius + extraHeight), color);
        Debug.DrawRay(groundCheck.position, Vector3.down * groundCheckRadius, color);
    }

    public void ChangeState(State<PlayerContext> newState)
    {
        stateMachine.ChangeState(newState);
    }
    public bool GetMovement(Vector2 moveInput, float moveSpeed)
    {
        if (rb == null) return false;

        if (moveInput.magnitude > 0.1f)
        {
            // Converte input 2D para vetor 3D no espaço local do jogador
            Vector3 input = new Vector3(moveInput.x, 0f, moveInput.y);
            Vector3 moveDirection = transform.TransformDirection(input).normalized;

            // Calcula nova velocidade horizontal
            Vector3 velocity = moveDirection * moveSpeed;

            // Mantém velocidade vertical (gravidade, pulo)
            velocity.y = rb.linearVelocity.y;

            // Aplica a velocidade ao Rigidbody
            rb.linearVelocity = velocity;

            // Atualiza animação
            playerAnimatorController.SetMovement(moveInput);

            // Rotaciona o jogador conforme input de look (mouse)
            float yaw = lookInput.x * rotationSensitivity;
            transform.Rotate(Vector3.up, yaw);

            return true; // está se movendo
        }
        else
        {
            // Se não está se movendo, zera velocidade horizontal para parar
            Vector3 velocity = rb.linearVelocity;
            velocity.x = 0;
            velocity.z = 0;
            rb.linearVelocity = velocity;

            playerAnimatorController.SetMovement(Vector2.zero);

            return false; // parado
        }
    }
}