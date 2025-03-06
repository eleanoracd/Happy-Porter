using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] protected Animator baseAnimator;
    [SerializeField] protected Animator weaponAnimator;

    protected PlayerAttackState state;

    private const string BASE_GAMEOBJECT = "Base";
    private const string WEAPON_GAMEOBJECT = "Weapon";
    private const string ATTACK_BOOL = "attack";

    protected virtual void Start()
    {
        baseAnimator = transform.Find(BASE_GAMEOBJECT).GetComponent<Animator>();
        weaponAnimator = transform.Find(WEAPON_GAMEOBJECT).GetComponent<Animator>();

        gameObject.SetActive(false);
    }

    public virtual void EnterWeapon()
    {
        gameObject.SetActive(true);

        baseAnimator.SetBool(ATTACK_BOOL, true);
        weaponAnimator.SetBool(ATTACK_BOOL, true);
    }

    public virtual void ExitWeapon()
    {
        baseAnimator.SetBool(ATTACK_BOOL, false);
        weaponAnimator.SetBool(ATTACK_BOOL, false);

        gameObject.SetActive(false);
    }

    #region Animation Triggers
    public virtual void AnimationFinishTrigger()
    {
        state.AnimationFinishTrigger();
    }

    #endregion

    public void InitializeWeapon(PlayerAttackState state)
    {
        this.state = state;
    }
}
