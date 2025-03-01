using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInputActions _playerInputActions;
    private PlayerInput playerInput;
    private Camera cam;


    public Vector2 RawMoveInput { get; private set; }
    public Vector2 RawDashDirectionInput { get; private set; }
    public Vector2Int DashDirectionInput { get; private set; }
    public int NormalizeInputX { get; private set; }
    public int NormalizeInputY { get; private set; }
    public bool IsJumping { get; private set; }
    public bool IsRunning { get; private set; }
    public bool IsInteract { get; private set; }
    public bool JumpInputStop { get; private set; }
    public bool GrabInput { get; private set; }
    public bool DashInput { get; private set; }
    public bool DashInputStop { get; private set; }

    [SerializeField] private float inputHoldTime = 0.2f;

    private float jumpInputStartTime;
    private float dashInputStartTime;

    private void Awake()
    {
        _playerInputActions = new PlayerInputActions();   
    }

    private void Start()
    {
       playerInput = GetComponent<PlayerInput>();
       cam = Camera.main;
    }

    private void Update()
    {
        CheckJumpInputHoldTime();
        CheckDashInputHoldTime();
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
        _playerInputActions.Player.Grab.performed += OnGrab;
        _playerInputActions.Player.Grab.canceled += OnGrab;
        _playerInputActions.Player.Dash.performed += OnDash;
        _playerInputActions.Player.Dash.canceled += OnDash;
        _playerInputActions.Player.DashDirection.performed += OnDashDirectionInput;
        _playerInputActions.Player.DashDirection.canceled += OnDashDirectionInput;
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
        _playerInputActions.Player.Grab.performed -= OnGrab;
        _playerInputActions.Player.Grab.canceled -= OnGrab;
        _playerInputActions.Player.Dash.performed -= OnDash;
        _playerInputActions.Player.Dash.canceled -= OnDash;
        _playerInputActions.Player.DashDirection.performed -= OnDashDirectionInput;
        _playerInputActions.Player.DashDirection.canceled -= OnDashDirectionInput;
        _playerInputActions.Player.Interact.performed -= OnInteract;
        _playerInputActions.Player.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        RawMoveInput = context.ReadValue<Vector2>();

        if(Mathf.Abs(RawMoveInput.x) > 0.5f)
        {
            NormalizeInputX = (int)(RawMoveInput * Vector2.right).normalized.x;
        }
        else
        {
            NormalizeInputX = 0;
        }

        if(Mathf.Abs(RawMoveInput.y) > 0.5f)
        {
            NormalizeInputY = (int)(RawMoveInput * Vector2.up).normalized.y;
        }
        else
        {
            NormalizeInputY = 0;
        }
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            IsJumping = true;
            JumpInputStop = false;
            jumpInputStartTime = Time.time;
        }

        if (context.canceled)
        {
            JumpInputStop = true;
        }
    }

    private void OnGrab(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            GrabInput = true;
        }
        
        if(context.canceled)
        {
            GrabInput = false;
        }
    }

    private void OnDash(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            DashInput = true;
            DashInputStop = false;
            dashInputStartTime = Time.time;
        }

        else if(context.canceled)
        {
            DashInputStop = true;
        }
    }

    private void OnDashDirectionInput(InputAction.CallbackContext context)
    {
        RawDashDirectionInput = context.ReadValue<Vector2>();

        if(playerInput.currentControlScheme == "Keyboard")
        {
            RawDashDirectionInput = cam.ScreenToWorldPoint((Vector3)RawDashDirectionInput) - transform.position;
        }

        DashDirectionInput = Vector2Int.RoundToInt(RawDashDirectionInput.normalized);
    }

    private void OnRun(InputAction.CallbackContext context)
    {
        IsRunning = context.performed;
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        IsInteract = context.performed;
    }

    public void UseJump() => IsJumping = false;

    public void UseDash() => DashInput = false;

    private void CheckJumpInputHoldTime()
    {
        if(Time.time >= jumpInputStartTime + inputHoldTime)
        {
            IsJumping = false;
        }
    }

    private void CheckDashInputHoldTime()
    {
        if(Time.time >= dashInputStartTime + inputHoldTime)
        {
            DashInput = false;
        }
    }
}
