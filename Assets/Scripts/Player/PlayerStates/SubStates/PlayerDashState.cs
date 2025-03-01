using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerAbilityState
{
    public bool CanDash { get; private set; }
    private bool isHolding;
    private bool dashInputStop;

    private float lashDashtime;

    private Vector2 dashDirection;
    private Vector2 dashDirectionInput;
    private Vector2 lastAfterImagePosition;

    public PlayerDashState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animationBoolName) : base(player, stateMachine, playerData, animationBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        CanDash = false;
        player.InputManager.UseDash();

        isHolding = true;
        dashDirection = Vector2.right * player.FacingDirection;

        Time.timeScale = playerData.holdTimeScale;
        startTime = Time.unscaledTime;

        player.DashDirectionIndicator.gameObject.SetActive(true);
    }

    public override void Exit()
    {
        base.Exit();

        if(player.CurrentVelocity.y > 0)
        {
            player.SetVelocityY(player.CurrentVelocity.y * playerData.dashEndYMultiplier);
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(!isExitingState)
        player.Animator.SetFloat("yVelocity", player.CurrentVelocity.y);
        player.Animator.SetFloat("xVelocity", Mathf.Abs(player.CurrentVelocity.x));
        {
            if(isHolding)
            {
                dashDirectionInput = player.InputManager.DashDirectionInput;
                dashInputStop = player.InputManager.DashInputStop;

                if(dashDirectionInput != Vector2.zero)
                {
                    dashDirection = dashDirectionInput;
                    dashDirection.Normalize();
                }

                float angle = Vector2.SignedAngle(Vector2.right, dashDirection);
                player.DashDirectionIndicator.rotation = Quaternion.Euler(0f, 0f, angle - 45f);

                if(dashInputStop || Time.unscaledTime >= startTime + playerData.maxHoldTime)
                {
                    isHolding = false;
                    Time.timeScale = 1f;
                    startTime = Time.time;
                    player.CheckIfShouldFlip(Mathf.RoundToInt(dashDirection.x));
                    player.RigidBody.drag = playerData.drag;
                    player.SetVelocity(playerData.dashVelocity, dashDirection);
                    player.DashDirectionIndicator.gameObject.SetActive(false);
                    // PlaceAfterImage();
                }
            }
            else
            {
                player.SetVelocity(playerData.dashVelocity, dashDirection);
                // CheckIfShouldPlaceAfterImage();
                
                if(Time.time >= startTime + playerData.dashTime)
                {
                    player.RigidBody.drag = 0f;
                    isAbilityDone = true;
                    lashDashtime = Time.time;
                }
            }
        }
    }

    // private void CheckIfShouldPlaceAfterImage()
    // {
    //     if(Vector2.Distance(player.transform.position, lastAfterImagePosition) >= playerData.distanceBetweenAfterImages)
    //     {
    //         PlaceAfterImage();
    //     }
    // }

    // private void PlaceAfterImage()
    // {
    //     PlayerAfterImagePool.Instance.GetFromPool();
    //     lastAfterImagePosition = player.transform.position;
    // }

    public bool CheckIfCanDash()
    {
        return CanDash && Time.time >= lashDashtime + playerData.dashCooldown;
    }

    public void ResetCanDash() => CanDash = true;
}
