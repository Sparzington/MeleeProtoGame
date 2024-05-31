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

    //Subscribed Events
    private void CurrentATKFinished(object sender, EventArgs e)
    {
        Debug.Log("Attack Done");
    }

    //Inputs
    private void OnLightAttack()
    {
        bool onRight = _stanceComponent.IsOnRightSide();
        _fighter.LightAttack();
        _characterAnimator.PlayLightAttack(onRight);
    }
    private void OnHeavyAttack()
    {
        bool onRight = _stanceComponent.IsOnRightSide();
        _fighter.HeavyAttack();
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
