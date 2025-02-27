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

    [SerializeField] private PlayerData playerData;
    #endregion

    #region Components
    public Animator Animator { get; private set; }
    public InputManager InputManager { get; private set; }
    public Rigidbody2D RigidBody { get; private set; }
    #endregion

    #region Check Transform
    [SerializeField] private Transform groundCheck;

    #endregion

    #region Other Variables
    public Vector2 CurrentVelocity { get; private set; }
    public int FacingDirection { get; private set; }

    private Vector2 workspace;

    private const string IDLE = "idle";
    private const string MOVE = "move";
    private const string INAIR = "inAir";
    private const string LAND = "land";
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
    }

    private void Start()
    {
        Animator = GetComponent<Animator>();
        InputManager = GetComponent<InputManager>();
        RigidBody = GetComponent<Rigidbody2D>();

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

    public void CheckIfShouldFlip(int xInput)
    {
        if(xInput != 0 && xInput != FacingDirection)
        {
            Flip();
        }
    }
    #endregion

    #region Other Functions
    private void AnimationTrigger() => StateMachine.CurrentState.AnimationTrigger();

    private void AnimationFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();

    private void Flip()
    {
        FacingDirection *= -1;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }
    #endregion
}
