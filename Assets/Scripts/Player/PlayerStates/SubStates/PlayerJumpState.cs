using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerAbilityState
{
    private int ammountOfJumpsLeft;

    public PlayerJumpState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animationBoolName) : base(player, stateMachine, playerData, animationBoolName)
    {
        ammountOfJumpsLeft = playerData.ammountOfJumps;
    }

    public override void Enter()
    {
        base.Enter();

        player.InputManager.UseJump();
        player.SetVelocityY(playerData.jumpVelocity);
        isAbilityDone = true;
        ammountOfJumpsLeft--;
        player.InAirState.SetIsJumping();
    }

    public bool CanJump()
    {
        if(ammountOfJumpsLeft > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ResetAmmountOfJumpsLeft() => ammountOfJumpsLeft = playerData.ammountOfJumps;

    public void DecreaseAmmountOfJumpsLeft() => ammountOfJumpsLeft--;
}
