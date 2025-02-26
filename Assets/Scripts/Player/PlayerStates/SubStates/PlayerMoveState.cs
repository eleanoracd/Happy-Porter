using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string _animationBoolName) : base(player, stateMachine, playerData, _animationBoolName)
    {
    }
}
