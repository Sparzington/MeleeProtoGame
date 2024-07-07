using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    //Targeting
    private Collider MyCollider; //For target checking
    private Vector3 TargetDirection;
    private Transform TargetTransform;
    private const float ScanRadius = 10;
    private Collider[] PotentialTargets;
    [SerializeField] private LayerMask TargetingMask;

    public float Aim_X;
    public float Aim_Y;

    private void Awake()
    {
        //Stance targeting
        PotentialTargets = new Collider[5];
        MyCollider = GetComponent<Collider>();

        //Rigidbody component
        _rigidBody = GetComponent<Rigidbody>();

        //Fighter
        _fighter = GetComponent<Fighter>();

        //Stance Widget
        _stanceComponent = GetComponentInChildren<StanceRotate>();

        //Animator Manager
        _characterAnimator = GetComponent<CharacterAnimator>();
    }

    private void Start()
    {
        _fighter.FighterInit();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            _fighter.LightAttack(false);
            _characterAnimator.PlayAttack(150, AttackTier.LIGHT);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            _fighter.LightAttack(false);

            _characterAnimator.PlayAttack(-90, AttackTier.LIGHT);
        }
    }

    private void UpdateAnimator()
    {
        _characterAnimator.SetStanceAim(Aim_X, Aim_Y);
    }
}
