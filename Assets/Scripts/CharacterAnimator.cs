using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class CharacterAnimator : MonoBehaviour
{
    private Animator _animator;

    [SerializeField] private Rig ArmRig;
    private float rigResetTimer;
    private float rigResetSpeed = 5f;

    //Hashed Animation names
    //Idle
    private int LeftHeavyHigh;
    private int LeftLow;
    private int RightHeavyHigh;
    private int RightLow;

    //Attacks
    private int RightLight;
    private int LeftLight;
    
    private int Attacking; //Bool

    private  const float animTransitionTime = 0.08f;

    
    private void Awake()
    {
        _animator = GetComponent<Animator>();

        LeftHeavyHigh = Animator.StringToHash("LeftSlash");
        LeftLow = Animator.StringToHash("LeftUpswing");
        LeftLight = Animator.StringToHash("LeftLight");

        RightHeavyHigh = Animator.StringToHash("RightSlash");
        RightLow = Animator.StringToHash("RightUpswing");
        RightLight = Animator.StringToHash("RightLight");

        //anim bool
        Attacking = Animator.StringToHash("Attack");
    }
    private void LateUpdate()
    {
        UpdateIKRig();
    }

    private void UpdateIKRig()
    {
        if (_animator.GetBool(Attacking)) 
        {
            ArmRig.weight = 0;
            rigResetTimer = 0.0f;
        }
        else
        {
            if (ArmRig.weight  != 1)
            {
                rigResetTimer += Time.deltaTime * rigResetSpeed;
                ArmRig.weight = Mathf.MoveTowards(0, 1, rigResetTimer);
            }            
        }
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

    public void PlayAttack(float angle, AttackTier tier)
    {       
        if (angle > 0 && angle <= 90)
        {
            if (tier == AttackTier.HEAVY)
            {
                //_animator.Play(RightHeavyHigh);
                _animator.CrossFade(RightHeavyHigh, animTransitionTime);
                return;
            }
            _animator.CrossFade(RightLight, animTransitionTime);
            //_animator.Play(RightLight);
        }
        else if (angle > 90 && angle <= 179)
        {
            if (tier == AttackTier.HEAVY)
            {
                _animator.CrossFade(LeftHeavyHigh, animTransitionTime);
                return;
            }
            _animator.CrossFade(LeftLight, animTransitionTime);
        }
        else if (angle < -90 && angle >= -179)
        {
            if (tier == AttackTier.HEAVY)
            {
                _animator.CrossFade(LeftLow, animTransitionTime);
                return;
            }
            _animator.CrossFade(LeftLow, animTransitionTime);
        }
        else if (angle < 0 && angle >= -90)
        {
            if (tier == AttackTier.HEAVY)
            {
                _animator.CrossFade(RightLow, animTransitionTime);
                return;
            }
            _animator.CrossFade(RightLow, animTransitionTime);
        }
    }
}
