using PlayerInput;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputReader : MonoBehaviour
{
    private PlayerInputActions _inputActions;
    
    public event Action<Vector2> OnMove;
    public event Action<Vector2> OnLook;
    public event Action OnJump;
    public event Action OnInteract;
    public event Action OnSprintPressed;
    public event Action OnTurnLeft;
    public event Action OnTurnRight;
    public event Action OnExitInteract;
    public event Action OnInventory;

    private void Awake()
    {
        _inputActions = new PlayerInputActions();

        _inputActions.Player.Move.performed += ctx => OnMove?.Invoke(ctx.ReadValue<Vector3>());
        _inputActions.Player.Move.canceled += ctx => OnMove?.Invoke(Vector3.zero);

        _inputActions.Player.Jump.performed += ctx => OnJump?.Invoke();

        _inputActions.Player.Sprint.started += ctx => OnSprintPressed?.Invoke();

        _inputActions.Player.TurnLeft.started += ctx => OnTurnLeft?.Invoke();
        _inputActions.Player.TurnRight.started += ctx => OnTurnRight?.Invoke();

        _inputActions.Player.Interact.performed += ctx => OnInteract?.Invoke();
        _inputActions.Player.ExitInteract.performed += ctx => OnExitInteract?.Invoke();

        _inputActions.Player.Inventory.performed += ctx => OnInventory?.Invoke();

        _inputActions.Player.Look.performed += ctx => OnLook?.Invoke(ctx.ReadValue<Vector2>());
        _inputActions.Player.Look.canceled += ctx => OnLook?.Invoke(Vector2.zero);
    }

    private void OnEnable()
    {
        _inputActions.Enable();
    }

    private void OnDisable()
    {
        _inputActions.Disable();
    }
}