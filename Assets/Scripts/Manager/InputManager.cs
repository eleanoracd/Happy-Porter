using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInputActions _playerInputActions;

    public Vector2 RawMoveInput { get; private set; }
    public int NormalizeInputX { get; private set; }
    public int NormalizeInputY { get; private set; }
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
        RawMoveInput = context.ReadValue<Vector2>();

        NormalizeInputX = (int)(RawMoveInput * Vector2.right).normalized.x;
        NormalizeInputY = (int)(RawMoveInput * Vector2.up).normalized.y;
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            IsJumping = true;
        }
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
