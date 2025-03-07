using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLedgeClimbState : PlayerState
{
    private Vector2 detectedPosition;
    private Vector2 cornerPosition;
    private Vector2 startPosition;
    private Vector2 stopPosition;
    private Vector2 workspace;

    private bool isHanging;
    private bool isClimbing;
    private bool jumpInput;
    private bool isTouchingCeiling;

    private int xInput;
    private int yInput;

    private const string CLIMBLEDGE = "climbLedge";
    private const string ISTOUCHINGCEILING = "isTouchingCeiling";

    public PlayerLedgeClimbState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animationBoolName) : base(player, stateMachine, playerData, animationBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        player.Animator.SetBool(CLIMBLEDGE, false);
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();

        isHanging = true;
    }

    public override void Enter()
    {
        base.Enter();

        core.Movement.SetVelocityZero();
        player.transform.position = detectedPosition;
        cornerPosition = DetermineCornerPosition();

        startPosition.Set(cornerPosition.x - (core.Movement.FacingDirection * playerData.startOffset.x), cornerPosition.y - playerData.startOffset.y);
        stopPosition.Set(cornerPosition.x + (core.Movement.FacingDirection * playerData.stopOffset.x), cornerPosition.y + playerData.stopOffset.y);

        player.transform.position = startPosition;
    }

    public override void Exit()
    {
        base.Exit();

        isHanging = false;

        if(isClimbing)
        {
            player.transform.position = stopPosition;
            isClimbing = false;
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(isAnimationFinished)
        {
            if(isTouchingCeiling)
            {
                stateMachine.ChangeState(player.CrouchIdleState);
            }
            else
            {
                stateMachine.ChangeState(player.IdleState);
            }
        }
        else
        {
            xInput = player.InputManager.NormalizeInputX;
            yInput = player.InputManager.NormalizeInputY;
            jumpInput = player.InputManager.IsJumping;

            core.Movement.SetVelocityZero();
            player.transform.position = startPosition;

            if(xInput == core.Movement.FacingDirection && isHanging && !isClimbing)
            {
                CheckForspace();
                isClimbing = true;
                player.Animator.SetBool(CLIMBLEDGE, true);
            }
            else if(yInput == -1 && isHanging && !isClimbing)
            {
                stateMachine.ChangeState(player.InAirState);
            }
            else if(jumpInput && !isClimbing)
            {
                player.WallJumpState.DetermineWallJumpDirection(true);
                stateMachine.ChangeState(player.WallJumpState);
            }
        }

    }

    public void SetDetectedPosition(Vector2 position) => detectedPosition = position;

    private void CheckForspace()
    {
        isTouchingCeiling = Physics2D.Raycast(cornerPosition + (Vector2.up * 0.015f) + (Vector2.right * core.Movement.FacingDirection * 0.015f), Vector2.up, playerData.standColliderHeight, core.CollisionSenses.WhatIsGround);
        player.Animator.SetBool(ISTOUCHINGCEILING, isTouchingCeiling);
    }

    private Vector2 DetermineCornerPosition()
    {
        RaycastHit2D xHit = Physics2D.Raycast(core.CollisionSenses.WallCheck.position, Vector2.right * core.Movement.FacingDirection, core.CollisionSenses.WallCheckDistance, core.CollisionSenses.WhatIsGround);
        float xDistance = xHit.distance;
        workspace.Set((xDistance + 0.015f) * core.Movement.FacingDirection, 0f);
        RaycastHit2D yHit = Physics2D.Raycast(core.CollisionSenses.LedgeCheck.position + (Vector3)(workspace), Vector2.down, core.CollisionSenses.LedgeCheck.position.y - core.CollisionSenses.WallCheck.position.y + 0.015f, core.CollisionSenses.WhatIsGround);
        float yDistance = yHit.distance;

        workspace.Set(core.CollisionSenses.WallCheck.position.x + (xDistance * core.Movement.FacingDirection), core.CollisionSenses.LedgeCheck.position.y - yDistance);
        return workspace;
    }
}
