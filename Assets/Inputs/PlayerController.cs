using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum FightState { IDLE, ATTACKING}
public class PlayerController : MonoBehaviour
{
    private FightState CurrentState;

    //Our input action Asset
    private FightControls _fightControls;

    private CharacterAnimator _characterAnimator;

    private StanceRotate _stanceComponent;

    [SerializeField] private float attackCooldown = 0.6f;
    private float currentAttackDelay;
    private bool canAttack;

    private void Awake()
    {
        _fightControls = new FightControls();

        _stanceComponent = GetComponentInChildren<StanceRotate>();

        _characterAnimator = GetComponent<CharacterAnimator>();
    }
    private void Start()
    {
        CurrentState = FightState.IDLE;
    }
    private void OnLightAttack()
    {
        if(canAttack)
        {
            canAttack= false;
            currentAttackDelay = 0.0f;

            _characterAnimator.PlayLightAttack();
        }
    }

    private void OnHeavyAttack()
    {
        if(canAttack)
        {
            canAttack = false;
            currentAttackDelay = 0.0f;
        }
    }
   
    private void Update()
    {
        switch (CurrentState)
        {
            case FightState.IDLE:
                _characterAnimator.SetStanceAnim(_stanceComponent.Angle);

                break;
            case FightState.ATTACKING:

                if (canAttack)
                {
                    canAttack = false;
                    _characterAnimator.PlayLightAttack();
                }
                break;
            default:
                break;
        }

        if (currentAttackDelay < attackCooldown)
        {
            currentAttackDelay += Time.deltaTime;
        }
        else
        {
            if (!canAttack)
            {
                canAttack = true;
            }
        }
    }
    
}
