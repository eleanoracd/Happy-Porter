using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInputActions _playerInputActions;

    public Vector2 MoveInput { get; private set; }
    public bool IsJumping { get; private set; }
    public bool IsRunning { get; private set; }
    public bool IsInteract { get; private set; }

    private void Awake()
    {
        _playerInputActions = new PlayerInputActions();   
    }

    private void OnEnable()
    {
        _playerInputActions.Player.Enable();
        _playerInputActions.Player.Move.performed += OnMove;
        _playerInputActions.Player.Move.canceled += OnMove;
        _playerInputActions.Player.Jump.performed += OnJump;
        _playerInputActions.Player.Jump.canceled += OnJump;
        _playerInputActions.Player.Run.performed += OnRun;
        _playerInputActions.Player.Run.canceled += OnRun;
        _playerInputActions.Player.Interact.performed += OnInteract;
    }

    private void OnDisable()
    {
        _playerInputActions.Player.Move.performed -= OnMove;
        _playerInputActions.Player.Move.canceled -= OnMove;
        _playerInputActions.Player.Jump.performed -= OnJump;
        _playerInputActions.Player.Jump.canceled -= OnJump;
        _playerInputActions.Player.Run.performed -= OnRun;
        _playerInputActions.Player.Run.canceled -= OnRun;
        _playerInputActions.Player.Interact.performed -= OnInteract;
        _playerInputActions.Player.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>();
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        IsJumping = context.performed;
    }

    private void OnRun(InputAction.CallbackContext context)
    {
        IsRunning = context.performed;
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        IsInteract = context.performed;
    }
}
