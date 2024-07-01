using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class CharacterAnimator : MonoBehaviour
{
    //Animator component
    private Animator _animator;

    //Fighter component - Read Only
    private Fighter _fighter;

    [Header("IK Rigs")]
    [SerializeField] private Rig LeftArmRig;
    [SerializeField] private Rig RightArmRig;
    [SerializeField] private Rig HeadRig;
    private float rigResetTimer;
    private float rigResetSpeed = 5f;

    //Hashed Animation names
        //Idle
    private int LeftHigh;
    private int LeftLow;
    private int RightHigh;
    private int _RightLow;
        //Attacks
    private int _LeftLightHigh;
    private int _LeftHeavyHigh;
    private int _LeftLightLow;
    private int _LeftHeavyLow;
    private int _RightLightHigh;
    private int _RightHeavyHigh;
    private int _RightLightLow;
    private int _RightHeavyLow;
    //Movement
    private int _WalkX;
    private int _WalkY;

    //String Hash names
    [Header("Anim Hash Names")]
    [SerializeField] private string LeftLightHigh;
    [SerializeField] private string LeftHeavyHigh;
    [SerializeField] private string LeftLightLower;
    [SerializeField] private string LeftHeavyLower;
    [SerializeField] private string RightLightHigh;
    [SerializeField] private string RightHeavHigh;
    [SerializeField] private string RightLightLower;
    [SerializeField] private string RightHeavyLower;

    private int Attack; //Bool

    private const float animTransitionTime = 0.08f;

    
    private void Awake()
    {
        _animator = GetComponent<Animator>();

        _fighter = GetComponent<Fighter>();

        InitHash(ref _LeftLightHigh, LeftLightHigh);
        InitHash(ref _LeftHeavyHigh, LeftHeavyHigh);
        InitHash(ref _LeftLightLow, LeftLightLower);
        InitHash(ref _LeftHeavyLow, LeftHeavyLower);
        InitHash(ref _RightLightHigh, RightLightHigh);
        InitHash(ref _RightHeavyHigh, RightHeavHigh);
        InitHash(ref _RightLightLow, RightLightLower);
        InitHash(ref _RightHeavyLow, RightHeavyLower);

        _WalkX = Animator.StringToHash("WalkX");
        _WalkY = Animator.StringToHash("WalkY");

        //anim bool
        Attack = Animator.StringToHash("Attack");
    }
    public void SetIKWeight(float weight)
    {
        if (weight < 0)
        {
            weight = 0;
        }
        else if (weight > 1.0f) 
        {
            weight = 1.0f; 
        }

        HeadRig.weight = weight;
        LeftArmRig.weight = weight;
        RightArmRig.weight = weight;
    }

    private void InitHash(ref int hash, string animName)
    {
        if (animName != null)
        {
            hash = Animator.StringToHash(animName);
        }
    }
    private void LateUpdate()
    {
        if (_fighter.currentState == FightState.ATTACKING)
        {
            _animator.SetBool(Attack, true);
        }
        UpdateIKRig();
    }

    private void UpdateIKRig()
    {
        if (_animator.GetBool(Attack)) 
        {
            RightArmRig.weight = 0;
            LeftArmRig.weight = 0;
            rigResetTimer = 0.0f;
        }
        else
        {
            if (RightArmRig.weight != 1)
            {
                rigResetTimer += Time.deltaTime * rigResetSpeed;
                RightArmRig.weight = Mathf.MoveTowards(0, 1, rigResetTimer);
                LeftArmRig.weight = Mathf.MoveTowards(0, 1, rigResetTimer);
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
                _animator.CrossFade(_RightHeavyHigh, animTransitionTime);
                return;
            }
            _animator.CrossFade(_RightLightHigh, animTransitionTime);
            //_animator.Play(RightLight);
        }
        else if (angle > 90 && angle <= 179)
        {
            if (tier == AttackTier.HEAVY)
            {
                _animator.CrossFade(_LeftHeavyHigh, animTransitionTime);
                return;
            }
            _animator.CrossFade(_LeftLightHigh, animTransitionTime);
        }
        else if (angle < -90 && angle >= -179)
        {
            if (tier == AttackTier.HEAVY)
            {
                _animator.CrossFade(_LeftHeavyLow, animTransitionTime);
                return;
            }
            _animator.CrossFade(_LeftLightLow, animTransitionTime);
        }
        else if (angle < 0 && angle >= -90)
        {
            if (tier == AttackTier.HEAVY)
            {
                _animator.CrossFade(_RightHeavyLow, animTransitionTime);
                return;
            }
            _animator.CrossFade(_RightLightLow, animTransitionTime);
        }
    }

    public void SetAnimatorWeight(int index, float weight, float time)
    {
        if (Mathf.Approximately(_animator.GetLayerWeight(index), weight))
        {
            return;
        }

        float t = 0;
        if (weight == 0)
        {
            t = Mathf.Lerp(1, 0, time);
        }
        else if (weight == 1)
        {
            t = Mathf.Lerp(0, 1, time);
        }

        _animator.SetLayerWeight(index, t);
    }
}
