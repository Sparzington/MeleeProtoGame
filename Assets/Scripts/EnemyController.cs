using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyAggro { NEUTRAL, CHASE, ENGAGING, ATTACK}
public class EnemyController : MonoBehaviour
{
    //Fighter Component
    private Fighter _fighter;

    //Animator Component
    private CharacterAnimator _characterAnimator;

    //Stance Widget
    private StanceRotate _stanceComponent;

    //Rigidbody componenet
    private Rigidbody _rigidBody;

    //Naviagation Component
    private Navigation _navigation;
    
    //Enemy state
    [SerializeField] private EnemyAggro _enemyAggro;

    //Targeting
    private Collider MyCollider; //For target checking
    private Vector3 TargetDirection;
    private Quaternion LookRot;
    private Transform TargetTransform;
    [SerializeField] private float TargetDistance;

    private Collider[] PotentialTargets;
    [SerializeField] private LayerMask TargetingMask;

    //Stance Aim - Animator setting
    [Range(-1,1)]
    public float Aim_X;
    [Range(-1, 1)]
    public float Aim_Y;

    [Header("Spot Radius")]
    [SerializeField] private float SpotRadius;
    [SerializeField] private float AttackRadius;
    [SerializeField] private float MaxAggroRadius;
    private int spotCount;

    
    public bool Engaged;

    private void Awake()
    {
        //Stance targeting
        PotentialTargets = new Collider[5];
        MyCollider = GetComponent<Collider>();

        //Rigidbody component
        _rigidBody = GetComponent<Rigidbody>();

        //Nav
        _navigation = GetComponent<Navigation>();

        //Fighter
        _fighter = GetComponent<Fighter>();

        //Stance Widget
        _stanceComponent = GetComponentInChildren<StanceRotate>();

        //Animator Manager
        _characterAnimator = GetComponent<CharacterAnimator>();

        PotentialTargets = new Collider[10];
    }

    private void Start()
    {
        _fighter.FighterInit();
        _navigation.InitAgent();

        _enemyAggro = EnemyAggro.NEUTRAL;
    }

    private void Update()
    {
        if (Engaged)
        {
            TargetDistance = Vector3.Distance(transform.position, TargetTransform.position);
            TargetDirection = (TargetTransform.position - transform.position).normalized;
            TargetDirection.y = 0;
        }

        _characterAnimator.Engaged = Engaged;

        switch (_enemyAggro)
        {
            case EnemyAggro.NEUTRAL:
                Engaged = false;
                _characterAnimator.SetLayerWeight(1, 1, 1);
                _characterAnimator.SetLayerWeight(0, 0, 1);

                if (LookForTarget())
                {
                    _enemyAggro = EnemyAggro.CHASE;
                }

                break;

            case EnemyAggro.CHASE:
                //Move to player
                Engaged = true;

                if (_navigation != null)
                {
                    _navigation.MoveTowards(TargetTransform.position);
                }

                if (TargetDistance <= AttackRadius)
                {
                    _enemyAggro = EnemyAggro.ENGAGING; 
                }
                else if (TargetDistance > MaxAggroRadius)
                {
                    _enemyAggro = EnemyAggro.NEUTRAL;
                }

                break;

            case EnemyAggro.ENGAGING:

                if (_navigation != null)
                {
                    _navigation.Stop();
                }
                _characterAnimator.UpdateHeadTarget(TargetTransform.position);
                transform.forward = Vector3.Lerp(transform.forward, TargetDirection, Time.deltaTime * 5);

                if (TargetDistance > AttackRadius)
                {
                    _enemyAggro = EnemyAggro.CHASE;
                }
                else
                {
                    AnimatorCombat();
                }
                break;

            case EnemyAggro.ATTACK:

                //Keep in place until attack done
                break;

            default:
                break;
        }
    }

    private bool LookForTarget()
    {
        for (int i = 0; i < PotentialTargets.Length; i++)
        {
            PotentialTargets[i] = null;
        }
        
        spotCount = 0;
        spotCount = Physics.OverlapSphereNonAlloc(transform.position, SpotRadius, PotentialTargets, TargetingMask);

        if (spotCount > 0)
        {
            for (int i = 0; i < spotCount; i++)
            {
                if (PotentialTargets[i].gameObject.CompareTag("Player"))
                {
                    TargetTransform = PotentialTargets[i].transform;
                    return true;
                }
            }
        }

        return false;
    }

    private void AnimatorCombat()
    {
        _characterAnimator.SetStanceAim(Aim_X, Aim_Y);

        switch (_fighter.AttackState)
        {
            case FightState.IDLE:
                _characterAnimator.SetLayerWeight(1, 1, 1);

                break;

            case FightState.WINDUP:
                _characterAnimator.SetLayerWeight(1, 0, 1);

                break;
            default:
                break;
        }
    }

    //Attack Calls
    private void LightAttack()
    {
        _fighter.LightAttack(false);

        if (_fighter.CanAttack)
        {
            _characterAnimator.PlayAttack(25, AttackTier.LIGHT);
        }
    }

    private void HeavyAttack()
    {
        _fighter.HeavyAttack();

        if (_fighter.CanAttack)
        {
            _characterAnimator.PlayAttack(25, AttackTier.HEAVY);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, SpotRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AttackRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, MaxAggroRadius);
    }
}
