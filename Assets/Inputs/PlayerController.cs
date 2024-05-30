using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(StanceRotate))]
public class PlayerController : MonoBehaviour
{
    //Our input action Asset
    private FightControls _fightControls;

    //Fighter Component
    private Fighter _fighter;

    private CharacterAnimator _characterAnimator;

    //Stance Widget
    private StanceRotate _stanceComponent;

    [SerializeField] private float attackCooldown = 0.6f;
    private float currentAttackDelay;
    private bool canAttack;

    private void Awake()
    {
        //Inputs
        _fightControls = new FightControls();

        //Stance Widget
        _stanceComponent = GetComponentInChildren<StanceRotate>();

        //Animator Manager
        _characterAnimator = GetComponent<CharacterAnimator>();
        _characterAnimator.OnAttackFinished += CurrentATKFinished;
    }

    private void CurrentATKFinished(object sender, EventArgs e)
    {
        Debug.Log("Attack Done");
        canAttack = true;
    }

    private void Start()
    {
        _fighter.CurrentState = FightState.IDLE;
    }
    private void OnLightAttack()
    {
        if (_fighter.CanAttack)
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
        switch (_fighter.CurrentState)
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
