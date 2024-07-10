using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
//using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class CharacterAnimator : MonoBehaviour
{
    //Animator component
    private Animator _animator;

    //Fighter component - Read Only
    private Fighter _fighter;

    [Header("Anim. Controller")]
    [SerializeField] private AnimatorController _controller;

    [Header("IK Rigs")]
    [SerializeField] private Rig LeftArmRig;
    [SerializeField] private Rig RightArmRig;
    [SerializeField] private Rig HeadRig;
    [SerializeField] private Transform HeadAimTarget;
    private float rigResetTimer;
    private float rigResetSpeed = 5f;

    //Hashed Animation names
        //Idle
    private int LeftHigh;
    private int LeftLow;
    private int RightHigh;
    private int _RightLow;

    private int _AngleX;
    private int _AngleY;
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
    private int _FreeWalk;
    private int _Engaged;

    public bool Engaged { get; set; }

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

    private int Attack; //Animator Bool

    private const float animTransitionTime = 0.08f;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _animator.runtimeAnimatorController = _controller;

        _fighter = GetComponent<Fighter>();

        InitHash(ref _LeftLightHigh, LeftLightHigh);
        InitHash(ref _LeftHeavyHigh, LeftHeavyHigh);
        InitHash(ref _LeftLightLow, LeftLightLower);
        InitHash(ref _LeftHeavyLow, LeftHeavyLower);
        InitHash(ref _RightLightHigh, RightLightHigh);
        InitHash(ref _RightHeavyHigh, RightHeavHigh);
        InitHash(ref _RightLightLow, RightLightLower);
        InitHash(ref _RightHeavyLow, RightHeavyLower);

        //Stance Aiming
        _AngleX = Animator.StringToHash("Angle_X");
        _AngleY = Animator.StringToHash("Angle_Y");

        //Legs Direction combat
        _WalkX = Animator.StringToHash("WalkX");
        _WalkY = Animator.StringToHash("WalkY");

        _FreeWalk = Animator.StringToHash("FreeWalk");
        _Engaged = Animator.StringToHash("Engaged");

        //anim bool
        Attack = Animator.StringToHash("Attack");
    }
    

    private void InitHash(ref int hash, string animName)
    {
        if (animName != null)
        {
            hash = Animator.StringToHash(animName);
        }
    }

    private void Update()
    {        
        _animator.SetBool(_Engaged, Engaged);

        if (_fighter.AttackState == FightState.ATTACKING)
        {
            _animator.SetBool(Attack, true);
        }
        
        if (!Engaged)
        {
            SetIKWeight(0);
        }
        else
        {
            UpdateCombatRig();

           //SetIKWeight(1);
        }        

        
    }

    /// <summary>
    /// Sets attatched Rig(s) weights accordingly (While attacking | Not attacking)
    /// </summary>
    private void UpdateCombatRig()
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

    

    /// <summary>
    /// Blend tree value for free roam walking.
    /// </summary>
    /// <param name="walkValue"></param>
    public void SetFreeWalkValues(float walkValue)
    {
        _animator.SetFloat(_FreeWalk, walkValue);
    }

    /// <summary>
    /// Blend tree values for Legs during combat.
    /// </summary>
    /// <param name="newX">Horizontal input</param>
    /// <param name="newY">Vertical input</param>
    /// <param name="time">Lerp time (Time.deltaTime or Time.fixedDeltaTime)</param>
    public void SetCombatWalkValues(float newX, float newY, float time)
    {
        float x = _animator.GetFloat(_WalkX);
        float y = _animator.GetFloat(_WalkY);

        x = Mathf.Lerp(x, newX, time * 5);
        y = Mathf.Lerp(y, newY, time * 5);

        _animator.SetFloat(_WalkX, x);
        _animator.SetFloat(_WalkY, y);
    }

    /// <summary>
    /// Blend tree aiming. Uses Vector2 direction values to set aim smoothly
    /// </summary>
    /// <param name="X"></param>
    /// <param name="Y"></param>
    public void SetStanceAim(float X, float Y)
    {
        X = Mathf.Clamp(X, -1, 1);
        Y = Mathf.Clamp(Y, -1, 1);

        _animator.SetFloat(_AngleX, X);
        _animator.SetFloat(_AngleY, Y);
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

    /// <summary>
    /// Used for switching between different animation layers: Base Layer, Legs Layer
    /// </summary>
    /// <param name="index"></param>
    /// <param name="weight"></param>
    /// <param name="time"></param>
    public void SetLayerWeight(int index, float weight, float time)
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

    public void UpdateHeadTarget(Vector3 newPos)
    {
        if (HeadRig != null && HeadAimTarget != null)
        {
            HeadAimTarget.position = newPos;
        }
    }

    /// <summary>
    /// Sets ALL attatched Rig weights to value
    /// </summary>
    /// <param name="weight">New rig weight</param>
    private void SetIKWeight(float weight)
    {
        if (weight <= 0)
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
}
