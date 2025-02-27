using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    protected int xInput;

    private bool IsJumping;

    public PlayerGroundedState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string _animationBoolName) : base(player, stateMachine, playerData, _animationBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        xInput = player.InputManager.NormalizeInputX;
        IsJumping = player.InputManager.IsJumping;

        if (IsJumping)
        {
            stateMachine.ChangeState(player.JumpState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
