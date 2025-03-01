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

    [SerializeField] private PlayerData playerData;
    #endregion

    #region Components
    public Animator Animator { get; private set; }
    public InputManager InputManager { get; private set; }
    public Rigidbody2D RigidBody { get; private set; }
    public Transform DashDirectionIndicator { get; private set; }
    #endregion

    #region Check Transform
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Transform ledgeCheck;

    #endregion

    #region Other Variables
    public Vector2 CurrentVelocity { get; private set; }
    public int FacingDirection { get; private set; }

    private Vector2 workspace;

    private const string IDLE = "idle";
    private const string MOVE = "move";
    private const string INAIR = "inAir";
    private const string LAND = "land";
    private const string WALLSLIDE = "wallSlide";
    private const string WALLCLIMB = "wallClimb";
    private const string WALLGRAB = "wallGrab";
    private const string LEDGECLIMBSTATE = "ledgeClimbState";

    #endregion

    #region Unity CallBack Functions
    private void Awake()
    {
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
    }

    private void Start()
    {
        Animator = GetComponent<Animator>();
        InputManager = GetComponent<InputManager>();
        RigidBody = GetComponent<Rigidbody2D>();
        DashDirectionIndicator = transform.Find("DashDirectionIndicator");

        FacingDirection = 1;
        
        StateMachine.Initialize(IdleState);
    }

    private void Update()
    {
        CurrentVelocity = RigidBody.velocity;
        StateMachine.CurrentState.LogicUpdate();
    }
    
    private void FixedUpdate() 
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }
    #endregion

    #region  Set Functions

    public void SetVelocityZero()
    {
        RigidBody.velocity = Vector2.zero;
        CurrentVelocity = Vector2.zero;
    }

    public void SetVelocity(float velocity, Vector2 angle, int direction)
    {
        angle.Normalize();
        workspace.Set(angle.x * velocity * direction, angle.y * velocity);
        RigidBody.velocity = workspace;
        CurrentVelocity = workspace;
    }

    public void SetVelocity(float velocity, Vector2 direction)
    {
        workspace = direction * velocity;
        RigidBody.velocity = workspace;
        CurrentVelocity = workspace;
    }

    public void SetVelocityX(float velocity)
    {
        workspace.Set(velocity, CurrentVelocity.y);
        RigidBody.velocity = workspace;
        CurrentVelocity = workspace;
    }

    public void SetVelocityY(float velocity)
    {
        workspace.Set(CurrentVelocity.x, velocity);
        RigidBody.velocity = workspace;
        CurrentVelocity = workspace;
    }
    #endregion

    #region  Check Functions
    public bool CheckIfGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, playerData.groundCheckRadius, playerData.whatIsGround);
    }

    public bool CheckIfTouchingWall()
    {
        return Physics2D.Raycast(wallCheck.position, Vector2.right * FacingDirection, playerData.wallCheckDistance, playerData.whatIsGround);
    }

    public bool CheckIfTouchingLedge()
    {
        return Physics2D.Raycast(ledgeCheck.position, Vector2.right * FacingDirection, playerData.wallCheckDistance, playerData.whatIsGround);
    }

    public bool CheckIfTouchingWallBack()
    {
        return Physics2D.Raycast(wallCheck.position, Vector2.right * -FacingDirection, playerData.wallCheckDistance, playerData.whatIsGround);
    }

    public void CheckIfShouldFlip(int xInput)
    {
        if(xInput != 0 && xInput != FacingDirection)
        {
            Flip();
        }
    }
    #endregion

    #region Other Functions
    public Vector2 DetermineCornerPosition()
    {
        RaycastHit2D xHit = Physics2D.Raycast(wallCheck.position, Vector2.right * FacingDirection, playerData.wallCheckDistance, playerData.whatIsGround);
        float xDistance = xHit.distance;
        workspace.Set(xDistance * FacingDirection, 0f);
        RaycastHit2D yHit = Physics2D.Raycast(ledgeCheck.position + (Vector3)(workspace), Vector2.down, ledgeCheck.position.y - wallCheck.position.y, playerData.whatIsGround);
        float yDistance = yHit.distance;

        workspace.Set(wallCheck.position.x + (xDistance * FacingDirection), ledgeCheck.position.y - yDistance);
        return workspace;
    }

    private void AnimationTrigger() => StateMachine.CurrentState.AnimationTrigger();

    private void AnimationFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();

    private void Flip()
    {
        FacingDirection *= -1;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }
    #endregion
}
