using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class CharacterAnimator : MonoBehaviour
{
    private Animator _animator;

    [SerializeField] private Rig ArmRig;

    //Hashed Animation names
    //Idle
    private int LH;
    private int LL;
    private int RH;
    private int RL;

    //Attacks
    private int RightAttack;
    private int LeftAttack;
    
    private int Attacking; //Bool

    public event EventHandler OnAttackFinished;
    private void Awake()
    {
        _animator = GetComponent<Animator>();

        LH = Animator.StringToHash("LeftSlash");
        LL = Animator.StringToHash("LeftUpswing");
        RH = Animator.StringToHash("RightSlash");
        RL = Animator.StringToHash("RightUpswing");
        Attacking = Animator.StringToHash("Attack");

        RightAttack = Animator.StringToHash("RightSlash");
        LeftAttack = Animator.StringToHash("LeftSlash");
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
    public void PlayLightAttack(bool rightSide)
    {

    }

    public void PlayAttack(float angle)
    {
        if (angle > 0 && angle <= 90)
        {
            _animator.Play(RH);
        }
        else if (angle > 90 && angle <= 179)
        {
            _animator.Play(LH);
        }
        else if (angle < -90 && angle >= -179)
        {
            _animator.Play(LL);
        }
        else if (angle < 0 && angle >= -90)
        {
            _animator.Play(RL);
        }
    }

    public void ATKAnimFinished()
    {
        Debug.Log("Finsihe called");

        ArmRig.weight = 1.0f;

        OnAttackFinished?.Invoke(this, EventArgs.Empty);
    }
}
