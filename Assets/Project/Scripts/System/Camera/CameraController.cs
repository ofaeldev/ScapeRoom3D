using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform playerBody;
    [SerializeField] private float mouseSensitivity = 100f;

    private float xRotation = 0f;

    private Vector2 lookInput;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;  // Trava o cursor no centro da tela
    }

    // M�todo para receber input do mouse via New Input System
    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    void Update()
    {
        UpdateRotationPlayer();
    }

    private void UpdateRotationPlayer()
    {
        // Ajusta a rota��o vertical da c�mera (pitch)
        xRotation -= lookInput.y * mouseSensitivity * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);  // Limita para n�o virar pra tr�s

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Rotaciona o corpo do jogador no eixo Y (yaw)
        playerBody.Rotate(Vector3.up * lookInput.x * mouseSensitivity * Time.deltaTime);
    }
}
