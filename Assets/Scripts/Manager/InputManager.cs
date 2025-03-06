using System;
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

    public bool[] AttackInputs { get; private set; }

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

       int count = Enum.GetValues(typeof(CombatInputs)).Length;
       AttackInputs = new bool[count];

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
        _playerInputActions.Player.PrimaryAttack.performed += OnPrimaryAttackInput;
        _playerInputActions.Player.PrimaryAttack.canceled += OnPrimaryAttackInput;
        _playerInputActions.Player.SecondaryAttack.performed += OnSecondaryAttackInput;
        _playerInputActions.Player.SecondaryAttack.canceled += OnSecondaryAttackInput;
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
        _playerInputActions.Player.PrimaryAttack.performed -= OnPrimaryAttackInput;
        _playerInputActions.Player.PrimaryAttack.canceled -= OnPrimaryAttackInput;
        _playerInputActions.Player.SecondaryAttack.performed -= OnSecondaryAttackInput;
        _playerInputActions.Player.SecondaryAttack.canceled -= OnSecondaryAttackInput;
        _playerInputActions.Player.Interact.performed -= OnInteract;
        _playerInputActions.Player.Disable();
    }

    private void OnPrimaryAttackInput(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            AttackInputs[(int)CombatInputs.primary] = true;
        }

        if(context.canceled)
        {
            AttackInputs[(int)CombatInputs.primary] = false;
        }
    }

    private void OnSecondaryAttackInput(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            AttackInputs[(int)CombatInputs.secondary] = true;
        }

        if(context.canceled)
        {
            AttackInputs[(int)CombatInputs.secondary] = false;
        }
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        RawMoveInput = context.ReadValue<Vector2>();

        NormalizeInputX = Mathf.RoundToInt(RawMoveInput.x);
        NormalizeInputX = Mathf.RoundToInt(RawMoveInput.y);
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

public enum CombatInputs
{
    primary,
    secondary,
}