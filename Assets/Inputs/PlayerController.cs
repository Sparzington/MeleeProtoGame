using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum AttackingStage { NONE, WINDUP, ACTIVE }
public class PlayerController : MonoBehaviour, IController
{
    //Our input action Asset
    private FightControls _fightControls;

    //Fighter Component
    private Fighter _fighter;

    //Animator Component
    private CharacterAnimator _characterAnimator;

    //Stance Widget
    private StanceRotate _stanceComponent;

    //Rigidbody componenet
    private Rigidbody _rigidBody;
    private AttackingStage _attackingStage;

    //Facing Camera
    [SerializeField] private CinemachineFreeLook _FreeLookCamera;
    
    //Mesh Object
    [SerializeField] private Transform PlayerBody;
    [SerializeField] private Transform Orientation;
    
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
    public Vector3 PrevMovementVector;
    private Vector3 FinalMoveVector;
    public float MovementAccel = 5;
    [SerializeField] private float WalkMax = 3;
    [SerializeField] private float RunMax = 5;
    public bool Run;
    private InputValue RunButton;

    //Bool for if locking on to target
    public bool Engaged
    {
        get; private set;
    }

    //Targeting 
    private Collider MyCollider; //For target checking
    private Vector3 TargetDirection;
    private Transform TargetTransform;
    private const float ScanRadius = 10;
    private Collider[] PotentialTargets;
    [SerializeField] private LayerMask TargetingMask;

    public static event EventHandler<Transform> OnEngage;
    public static event EventHandler OnDisengage;

    private void Awake()
    {
        //Camera
        _FreeLookCamera = GameObject.Find("Freelook Camera").GetComponent<CinemachineFreeLook>();
        if (_FreeLookCamera == null)
        {
            Debug.LogWarning("! No Player camera refernced !");
        }

        //Stance targeting
        PotentialTargets = new Collider[5];
        MyCollider = GetComponent<Collider>();

        //Rigidbody component
        _rigidBody = GetComponent<Rigidbody>();

        //Inputs
        _fightControls = new FightControls();

        //Fighter
        _fighter = GetComponent<Fighter>();

        //Stance Widget
        _stanceComponent = GetComponentInChildren<StanceRotate>();

        //Animator Manager
        _characterAnimator = GetComponent<CharacterAnimator>();


        StanceDeadZone = Screen.width * DeadZone;
        ScreenCenter = new Vector2(Screen.width / 2, Screen.height / 2);

        _attackingStage = AttackingStage.NONE;
    }

    private void Start()
    {
        _fighter.FighterInit();
        _characterAnimator.SetIKWeight(0);
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

    private void OnRun(InputValue value)
    {
        if (_fighter.currentState == FightState.IDLE)
        {
            Run = !Run;
        }
    }

    //Inputs - Stance Aiming
    private void OnStanceLock()
    {
        Debug.Log("Doing Stance");
        if (!Engaged)
        {
            TargetTransform = ScanForTarget();

            if (TargetTransform != transform)
            {                
                OnEngage?.Invoke(this, TargetTransform);
                Engage();

                TargetingDot._instance.SetTarget(TargetTransform);

                _stanceComponent.ToggleStanceUI(true);

                _characterAnimator.SetIKWeight(1);
            }
            else
            {
                _characterAnimator.SetIKWeight(0);
            }
        }
        else
        {           
            OnDisengage?.Invoke(this, EventArgs.Empty);
            Disengage();

            TargetingDot._instance.DisableTarget();

            _stanceComponent.ToggleStanceUI(false);

            _characterAnimator.SetIKWeight(0);
        }
    }
    private void OnStanceRotateGP(InputValue value)
    {
        if (Engaged)
        {
            Vector2 f = value.Get<Vector2>();
            Angle = GetAngle(f);

            _stanceComponent.UpdateRotation(Angle);
            _characterAnimator.SetStanceAnim(Angle);
        }        
    }

    private void OnStanceRotateMNK(InputValue value)
    {
        if (Engaged)
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
    }
    
    //Attacks 
    private void OnLightAttack()
    {
        if (Engaged)
        {
            bool onRight = _stanceComponent.IsOnRightSide();
            _fighter.LightAttack();
            if (_fighter.CanAttack)
            {
                IsHeavyAttack = false;
                //_rigidBody.AddForce(Vector3.forward*LightMoveForce,ForceMode.VelocityChange);
                _characterAnimator.PlayAttack(Angle, AttackTier.LIGHT);
            }
        }        
    }
    private void OnHeavyAttack()
    {
        if (Engaged)
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

    private Transform ScanForTarget()
    {
        Transform newTarget = transform;

        int n = Physics.OverlapSphereNonAlloc(transform.position, ScanRadius, PotentialTargets, TargetingMask);
        if (n-1 > 0)
        {
            for (int i = 0; i < n; i++)
            {
                if (PotentialTargets[i] != MyCollider)
                {
                    newTarget = PotentialTargets[i].gameObject.transform;
                    break;
                }
            }                
        }

        return newTarget;
    }
    private void FixedUpdate()
    {
        if (Engaged)
        {
            CombatMovement(WalkMax * 0.85f);
        }
        else
        {
            if (Run)
            {
                FreeMovement(RunMax);
            }
            else
            {
                FreeMovement(WalkMax);
            }

        }
    }
    private void Update()
    {
        if (Engaged)
        {
            TargetDirection = (TargetTransform.position - transform.position).normalized;
            TargetDirection.y = 0;
            transform.forward = TargetDirection;


            //Animator State mmachine
            switch (_fighter.currentState)
            {
                case FightState.IDLE:
                    _characterAnimator.SetStanceAnim(_stanceComponent.Angle);
                    _characterAnimator.SetAnimatorWeight(1, 1, 1);

                    break;

                case FightState.WINDUP:
                    _characterAnimator.SetAnimatorWeight(1, 0, 1);

                    break;

                case FightState.ATTACKING:

                    break;
                default:
                    break;
            }
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
    private void CombatMovement(float maxspeed)
    {
        if (_fighter.currentState == FightState.IDLE)
        {
            if (MovementVector != Vector3.zero)
            {
                PrevMovementVector = MovementVector;

                FinalMoveVector = (transform.forward * MovementVector.z) + (transform.right * MovementVector.x);
                FinalMoveVector.Normalize();

                _rigidBody.AddForce(FinalMoveVector * MovementAccel , ForceMode.Force);

                if (_rigidBody.velocity.magnitude > maxspeed)
                {
                    _rigidBody.velocity = _rigidBody.velocity.normalized * maxspeed;
                }
            }
            else
            {
                _rigidBody.velocity = Vector3.Lerp(_rigidBody.velocity, Vector3.zero, MovementAccel*Time.fixedDeltaTime);
            }
        }
    }

    //Help from https://www.youtube.com/watch?v=UCwwn2q4Vys&t=332s
    private void FreeMovement(float maxspeed)
    {
        ///Get direction from camera to player.
        
        Vector3 CameraForward = (transform.position - _FreeLookCamera.transform.position).normalized;
        CameraForward.y = 0;
        Orientation.forward = CameraForward;

        float horiz = MovementVector.x;
        float vert = MovementVector.z;

        Vector3 inputDir = Orientation.forward*vert + Orientation.right*horiz;
        if (inputDir != Vector3.zero)
        {
            transform.forward = Vector3.Slerp(transform.forward, inputDir, Time.deltaTime * 2);
        }

        if (MovementVector != Vector3.zero)
        {
            PrevMovementVector = MovementVector;

            FinalMoveVector = (Orientation.transform.forward * MovementVector.z) + (Orientation.transform.right * MovementVector.x);
            FinalMoveVector.Normalize();

            _rigidBody.AddForce(FinalMoveVector * MovementAccel, ForceMode.Force);

            if (_rigidBody.velocity.magnitude > maxspeed)
            {
                _rigidBody.velocity = _rigidBody.velocity.normalized * maxspeed;
            }
        }
        else
        {
            _rigidBody.velocity = Vector3.Lerp(_rigidBody.velocity, Vector3.zero, MovementAccel * Time.fixedDeltaTime);
        }

        if (_rigidBody.velocity.magnitude < 1)
        {
            _characterAnimator.SetWalkValues(0);
        }
        else if (_rigidBody.velocity.magnitude > 0 && !Run)
        {
            _characterAnimator.SetWalkValues(0.8f);
        }
        else if (_rigidBody.velocity.magnitude > 0 && Run)
        {
            _characterAnimator.SetWalkValues(1.0f);
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, ScanRadius);
    }

    public void Engage()
    {
        Engaged = true;
    }

    public void Disengage()
    {
        Engaged = false;
    }
}
