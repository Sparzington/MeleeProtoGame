using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    private Animator _animator;

    //Hashed Animation names
    private int LH;
    private int LL;
    private int RH;
    private int RL;
    private int ATK;

    public event EventHandler OnAttackFinished;
    private void Awake()
    {
        _animator = GetComponent<Animator>();

        LH = Animator.StringToHash("LeftAimHigh");
        LL = Animator.StringToHash("LeftAimLow");
        RH = Animator.StringToHash("RightAimHigh");
        RH = Animator.StringToHash("RightAimLow");
        ATK = Animator.StringToHash("Attack");
    }
    public void SetStanceAnim(float angle)
    {
        if (angle > 0 && angle <= 90)
        {
            _animator.SetInteger("Aim", 3);
        }
        else if (angle > 90 && angle <= 179)
        {
            _animator.SetInteger("Aim", 0);

        }
        else if (angle < -90 && angle >= -179)
        {
            _animator.SetInteger("Aim", 2);

        }
        else if (angle < 0 && angle >= -90)
        {
            _animator.SetInteger("Aim", 1);

        }
    }
    public void PlayLightAttack()
    {
        _animator.SetBool(ATK, true);
    }

    public void ATKAnimFinished()
    {
        OnAttackFinished?.Invoke(this, EventArgs.Empty);
        _animator.SetBool(ATK, false);
    }
}
