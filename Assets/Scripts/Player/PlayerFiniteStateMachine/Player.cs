using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region State Variable
    public PlayerStateMachine StateMachine { get; private set; }

    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerLandState LandState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerInAirState InAirState { get; private set; }
    public PlayerWallClimbState WallClimbState { get; private set; }
    public PlayerWallGrabState WallGrabState { get; private set; }
    public PlayerWallSlideState WallSlideState { get; private set; }
    public PlayerWallJumpState WallJumpState { get; private set; }
    public PlayerLedgeClimbState LedgeClimbState { get; private set; }
    public PlayerDashState DashState { get; private set; }
    public PlayerCrouchIdleState CrouchIdleState { get; private set; }
    public PlayerCrouchMoveState CrouchMoveState { get; private set; }
    public PlayerAttackState PrimaryAttackState { get; private set; }
    public PlayerAttackState SecondaryAttackState { get; private set; }

    [SerializeField] private PlayerData playerData;
    #endregion

    #region Components
    public Core Core { get; private set; }
    public Animator Animator { get; private set; }
    public InputManager InputManager { get; private set; }
    public Rigidbody2D RigidBody { get; private set; }
    public Transform DashDirectionIndicator { get; private set; }
    public BoxCollider2D PlayerCollider { get; private set;}
    public PlayerInventory Inventory { get; private set; }
    #endregion

    #region Other Variables

    private Vector2 workspace;

    private const string IDLE = "idle";
    private const string MOVE = "move";
    private const string INAIR = "inAir";
    private const string LAND = "land";
    private const string WALLSLIDE = "wallSlide";
    private const string WALLCLIMB = "wallClimb";
    private const string WALLGRAB = "wallGrab";
    private const string LEDGECLIMBSTATE = "ledgeClimbState";
    private const string CROUCHIDLE = "crouchIdle";
    private const string CROUCHMOVE = "crouchMove";
    private const string ATTACK = "attack";

    #endregion

    #region Unity CallBack Functions
    private void Awake()
    {
        Core = GetComponentInChildren<Core>();

        StateMachine = new PlayerStateMachine();  

        IdleState = new PlayerIdleState(this, StateMachine, playerData, IDLE);
        MoveState = new PlayerMoveState(this, StateMachine, playerData,  MOVE);
        JumpState = new PlayerJumpState(this, StateMachine, playerData,  INAIR);
        InAirState = new PlayerInAirState(this, StateMachine, playerData,  INAIR);
        LandState = new PlayerLandState(this, StateMachine, playerData,  LAND);
        WallSlideState = new PlayerWallSlideState(this, StateMachine, playerData, WALLSLIDE);
        WallGrabState = new PlayerWallGrabState(this, StateMachine, playerData, WALLGRAB);
        WallClimbState = new PlayerWallClimbState(this, StateMachine, playerData, WALLCLIMB);
        WallJumpState = new PlayerWallJumpState(this, StateMachine, playerData, INAIR);
        LedgeClimbState = new PlayerLedgeClimbState(this, StateMachine, playerData, LEDGECLIMBSTATE);
        DashState = new PlayerDashState(this, StateMachine, playerData, INAIR);
        CrouchIdleState = new PlayerCrouchIdleState(this, StateMachine, playerData, CROUCHIDLE);
        CrouchMoveState = new PlayerCrouchMoveState(this, StateMachine, playerData, CROUCHMOVE);
        PrimaryAttackState = new PlayerAttackState(this, StateMachine, playerData, ATTACK);
        SecondaryAttackState = new PlayerAttackState(this, StateMachine, playerData, ATTACK);
    }

    private void Start()
    {
        Animator = GetComponent<Animator>();
        InputManager = GetComponent<InputManager>();
        RigidBody = GetComponent<Rigidbody2D>();
        DashDirectionIndicator = transform.Find("DashDirectionIndicator");
        PlayerCollider = GetComponent<BoxCollider2D>();
        Inventory = GetComponent<PlayerInventory>();

        PrimaryAttackState.SetWeapon(Inventory.weapons[(int)CombatInputs.primary]);
        // SecondaryAttackState.SetWeapon(Inventory.weapons[(int)CombatInputs.secondary]);
        
        StateMachine.Initialize(IdleState);
    }

    private void Update()
    {
        Core.LogicUpdate();
        StateMachine.CurrentState.LogicUpdate();
    }
    
    private void FixedUpdate() 
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }
    #endregion

    #region Other Functions
    public void SetColliderHeight(float height)
    {
        Vector2 center = PlayerCollider.offset;
        workspace.Set(PlayerCollider.size.x, height);

        center.y += (height - PlayerCollider.size.y) / 2;

        PlayerCollider.size = workspace;
        PlayerCollider.offset = center;
    }

    private void AnimationTrigger() => StateMachine.CurrentState.AnimationTrigger();

    private void AnimationFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();

    
    #endregion
}
