using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private SO_WeaponData weaponData;
    [SerializeField] protected Animator baseAnimator;
    [SerializeField] protected Animator weaponAnimator;

    protected PlayerAttackState state;

    protected int attackCounter;

    private const string BASE_GAMEOBJECT = "Base";
    private const string WEAPON_GAMEOBJECT = "Weapon";
    private const string ATTACK_BOOL = "attack";
    private const string ATTACK_COUNTER = "attackCounter";

    protected virtual void Start()
    {
        baseAnimator = transform.Find(BASE_GAMEOBJECT).GetComponent<Animator>();
        weaponAnimator = transform.Find(WEAPON_GAMEOBJECT).GetComponent<Animator>();

        gameObject.SetActive(false);
    }

    public virtual void EnterWeapon()
    {
        gameObject.SetActive(true);

        if(attackCounter >= weaponData.movementSpeed.Length)
        {
            attackCounter = 0;
        }

        baseAnimator.SetBool(ATTACK_BOOL, true);
        weaponAnimator.SetBool(ATTACK_BOOL, true);

        baseAnimator.SetInteger(ATTACK_COUNTER, attackCounter);
        weaponAnimator.SetInteger(ATTACK_COUNTER, attackCounter);
    }

    public virtual void ExitWeapon()
    {
        baseAnimator.SetBool(ATTACK_BOOL, false);
        weaponAnimator.SetBool(ATTACK_BOOL, false);

        attackCounter++;

        gameObject.SetActive(false);
    }

    #region Animation Triggers
    public virtual void AnimationFinishTrigger()
    {
        state.AnimationFinishTrigger();
    }

    public virtual void AnimationStartMovementTrigger()
    {
        state.SetPlayerVelocity(weaponData.movementSpeed[attackCounter]);
    }

    public virtual void AnimationStopMovementTrigger()
    {
        state.SetPlayerVelocity(0f);
    }

    public virtual void AnimationTurnOffFlipTrigger()
    {
        state.SetFlipCheck(false);
    }

    public virtual void AnimationTurnOnFlipTrigger()
    {
        state.SetFlipCheck(true);
    }

    #endregion

    public void InitializeWeapon(PlayerAttackState state)
    {
        this.state = state;
    }
}
