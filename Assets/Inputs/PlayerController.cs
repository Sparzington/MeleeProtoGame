using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //Our input action Asset
    private FightControls _fightControls;

    private Animator _animator;

    [SerializeField] private float attackCooldown = 0.6f;
    private float currentAttackDelay;
    private bool canAttack;

    private void Awake()
    {
        _fightControls = new FightControls();
    
        _animator = GetComponent<Animator>();
    }
    private void OnLightAttack()
    {
        if(canAttack)
        {
            canAttack= false;
            currentAttackDelay = 0.0f;

            _animator.SetBool("Attack", true);
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
        if (currentAttackDelay < attackCooldown)
        {
            currentAttackDelay += Time.deltaTime;
        }
        else
        {
            _animator.SetBool("Attack", false);
            if (!canAttack)
            {
                canAttack = true;
            }
        }
    }
}
