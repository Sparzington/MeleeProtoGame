using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //Our input action Asset
    private FightControls _fightControls;

    //Fighter Component
    private Fighter _fighter;

    //Animator Component
    private CharacterAnimator _characterAnimator;


    //Stance Widget
    private StanceRotate _stanceComponent;

    private void Awake()
    {
        //Inputs
        _fightControls = new FightControls();

        //Fighter
        _fighter = GetComponent<Fighter>();
        _fighter.FighterInit();

        //Stance Widget
        _stanceComponent = GetComponentInChildren<StanceRotate>();

        //Animator Manager
        _characterAnimator = GetComponent<CharacterAnimator>();
        _characterAnimator.OnAttackFinished += CurrentATKFinished;
    }

    private void CurrentATKFinished(object sender, EventArgs e)
    {
        Debug.Log("Attack Done");
        _fighter.CurrentState = FightState.IDLE;
    }

    private void Start()
    {
        _fighter.CurrentState = FightState.IDLE;
    }
    private void OnLightAttack()
    {
        if (_fighter.CanAttack)
        {
            _fighter.LightAttack();
            _characterAnimator.PlayLightAttack();
        }
    }
    private void OnHeavyAttack()
    {

    }   
    private void Update()
    {
        switch (_fighter.CurrentState)
        {
            case FightState.IDLE:
                _characterAnimator.SetStanceAnim(_stanceComponent.Angle);

                break;
            case FightState.ATTACKING:

                break;
            default:
                break;
        }

        
    }
    
}
