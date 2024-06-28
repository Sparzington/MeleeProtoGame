using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum AttackingStage { NONE, WINDUP, ACTIVE }
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

    private Rigidbody _rigidBody;
    private AttackingStage _attackingStage;

    //Stance Aiming
    [Range(0f, 1f)]
    [SerializeField] private float DeadZone = 0.05f;
    private float StanceDeadZone;
    private Vector3 MousePos;
    private Vector3 ScreenCenter;
    public float Angle;

    //Movement - Attacks
    [SerializeField] private float WindupMoveForce;
    [SerializeField] private float LightMoveForce;
    [SerializeField] private float HeavyMoveForce;
    private bool IsHeavyAttack;

    //Movment - Walking
    public Vector3 MovementVector;
    public float MovementAccel = 5;

    private void Awake()
    {
        //Rigidbody component
        _rigidBody = GetComponent<Rigidbody>();

        //Inputs
        _fightControls = new FightControls();

        //Fighter
        _fighter = GetComponent<Fighter>();
        _fighter.FighterInit();

        //Stance Widget
        _stanceComponent = GetComponentInChildren<StanceRotate>();

        //Animator Manager
        _characterAnimator = GetComponent<CharacterAnimator>();

        StanceDeadZone = Screen.width * DeadZone;
        ScreenCenter = new Vector2(Screen.width / 2, Screen.height / 2);

        _attackingStage = AttackingStage.NONE;
    }

    //Inputs - Movement
    private void OnMovementMNK(InputValue value)
    {
        Debug.Log("Movement on MNK");   
        MovementVector = value.Get<Vector2>();
        MovementVector.z = MovementVector.y;
        MovementVector.y = 0;
    }
    private void OnMovementGP(InputValue value)
    {
        Debug.Log("Movement on GP");
        MovementVector = value.Get<Vector2>();
        MovementVector.z = MovementVector.y;
        MovementVector.y = 0;
    }


    //Inputs - Stance Aiming
    private void OnStanceRotateGP(InputValue value)
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
            IsHeavyAttack = false;
            //_rigidBody.AddForce(Vector3.forward*LightMoveForce,ForceMode.VelocityChange);
            _characterAnimator.PlayAttack(Angle,AttackTier.LIGHT );
        }
    }
    private void OnHeavyAttack()
    {
        bool onRight = _stanceComponent.IsOnRightSide();
        _fighter.HeavyAttack();
        if (_fighter.CanAttack)
        {
            IsHeavyAttack = true;
            //_rigidBody.AddForce(Vector3.forward*HeavyMoveForce, ForceMode.VelocityChange);
            _characterAnimator.PlayAttack(Angle, AttackTier.HEAVY);
        }
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

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W))
        {
            _rigidBody.AddForce(transform.forward * MovementAccel, ForceMode.Acceleration);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            _rigidBody.AddForce(-transform.forward * MovementAccel, ForceMode.Acceleration);
        }
    }
    private void Update()
    {
        switch (_fighter.currentState)
        {
            case FightState.IDLE:
                _characterAnimator.SetStanceAnim(_stanceComponent.Angle);

                

                break;
            case FightState.ATTACKING:

                break;
            default:
                break;
        }

        ///IF character is being animated from an attack input
        /// move character by [LIGHT,HEAVY] move values over time for SMOOTH movement
        /// 
        switch (_attackingStage)
        {
            case AttackingStage.NONE:
                Debug.Log("None");

                if (_fighter.currentState == FightState.WINDUP)
                {
                    _attackingStage = AttackingStage.WINDUP; 
                }
                break;
            case AttackingStage.WINDUP:
                Debug.Log("Windup");

                //_rigidBody.AddForce(transform.forward * WindupMoveForce, ForceMode.Force);
                
                if (_fighter.currentState == FightState.ATTACKING)
                {
                    _attackingStage = AttackingStage.ACTIVE;
                }
                break;

            case AttackingStage.ACTIVE:
                Debug.Log("Active");

                //RigidBodyAttackingMovement(IsHeavyAttack);

                if (_fighter.currentState == FightState.COMBORECOVER)
                {
                    _attackingStage = AttackingStage.NONE;
                }
                break;
            default:
                break;
        }

    }

    private void RigidBodyAttackingMovement(bool isHeavy)
    {
        if (isHeavy)
        {
            _rigidBody.AddForce(transform.forward * HeavyMoveForce, ForceMode.Force);
            return;
        }
        
        _rigidBody.AddForce(transform.forward * LightMoveForce, ForceMode.Force);
    }

}
