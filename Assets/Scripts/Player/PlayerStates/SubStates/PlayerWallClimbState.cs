using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallClimbState : PlayerWallState
{
    public PlayerWallClimbState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animationBoolName) : base(player, stateMachine, playerData, animationBoolName)
    {
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if( !isExitingState)
        {
            player.SetVelocityY(playerData.wallClimbVelocity);

            if(yInput != 1)
            {
                stateMachine.ChangeState(player.WallGrabState);
            }
        }
    }
}
