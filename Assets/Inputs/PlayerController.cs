using System;
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
        HandleInput();

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

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            _animator.SetInteger("Aim", 0);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {

            _animator.SetInteger("Aim", 1);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            _animator.SetInteger("Aim", 2);

        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            _animator.SetInteger("Aim", 3);

        }
    }
}
