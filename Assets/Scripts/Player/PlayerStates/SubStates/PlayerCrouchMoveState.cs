using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrouchMoveState : PlayerGroundedState
{
    public PlayerCrouchMoveState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string _animationBoolName) : base(player, stateMachine, playerData, _animationBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.SetColliderHeight(playerData.crouchColliderHeight);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(!isExitingState)
        {
            core.Movement.SetVelocityX(playerData.crouchMovementVelocity * core.Movement.FacingDirection);
            core.Movement.CheckIfShouldFlip(xInput);

            if(xInput == 0)
            {
                stateMachine.ChangeState(player.CrouchIdleState);
            }
            else if(yInput != -1 && !isTouchingCeiling)
            {
                stateMachine.ChangeState(player.MoveState);
            }
        }
    }

    public override void Exit()
    {
        base.Exit();

        player.SetColliderHeight(playerData.standColliderHeight);
    }
}
