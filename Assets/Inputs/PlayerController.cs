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

    //Stance Aiming
    [Range(0f, 1f)]
    [SerializeField] private float DeadZone = 0.05f;
    private float StanceDeadZone;
    private Vector3 MousePos;
    private Vector3 ScreenCenter;
    public float Angle;

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

        StanceDeadZone = Screen.width * DeadZone;
        ScreenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
    }

    //Subscribed Events
    private void CurrentATKFinished(object sender, EventArgs e)
    {
        Debug.Log("Attack Done");
    }

    //Inputs - Stance Aiming
    public void OnStanceRotateGP(InputValue value)
    {
        Vector2 f = value.Get<Vector2>();
        Angle = GetAngle(f);

        _stanceComponent.UpdateRotation(Angle);
        _characterAnimator.SetStanceAnim(Angle);
    }

    private void OnStanceRotateMNK(InputValue value)
    {

        MousePos = value.Get<Vector2>();
        if (MouseOutDeadZone(MousePos))
        {
            Debug.Log("Outside");

            Angle = GetAngle(MousePos - ScreenCenter);
            MouseReset();

            _stanceComponent.UpdateRotation(Angle);
            _characterAnimator.SetStanceAnim(Angle);
        }
        else
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
    }
    
    //Attacks 
    private void OnLightAttack()
    {
        bool onRight = _stanceComponent.IsOnRightSide();
        _fighter.LightAttack();
        if (_fighter.CanAttack)
        {
            _characterAnimator.PlayAttack(Angle);
        }
        //_characterAnimator.PlayLightAttack(onRight);

    }
    private void OnHeavyAttack()
    {
        bool onRight = _stanceComponent.IsOnRightSide();
        _fighter.HeavyAttack();
    }

    //Helpers
    #region Helpers
    private float GetAngle(Vector2 incoming)
    {
        //float angle = Mathf.Atan2(incoming.y - Vector2.right.y, incoming.x - Vector2.right.x) * 180 / Mathf.PI;
        float angle = Mathf.Atan2(incoming.y, incoming.x) * 180 / Mathf.PI;

        return angle;
    }
    private void MouseReset()
    {
        if (Cursor.lockState != CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    private bool MouseOutDeadZone(Vector3 mouse)
    {
        Vector3 dist = mouse - ScreenCenter;

        return dist.magnitude > StanceDeadZone;
    }
    #endregion
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
