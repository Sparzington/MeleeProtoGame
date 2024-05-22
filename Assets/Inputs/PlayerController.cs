using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //Our input action Asset
    private FightControls _fightControls;

    [SerializeField] private float attackCooldown = 0.6f;
    private float currentAttackDelay;
    private bool canAttack;

    private void Awake()
    {
        _fightControls = new FightControls();
    }
    private void OnLightAttack()
    {
        if(canAttack)
        {
            canAttack= false;
            currentAttackDelay = 0.0f;

            Debug.Log("Light attack pressed");
        }
    }

    private void OnHeavyAttack()
    {
        if(canAttack)
        {
            canAttack = false;
            currentAttackDelay = 0.0f;

            Debug.Log("Heavy attack pressed");
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
            if (!canAttack)
            {
                canAttack = true;
            }
        }
    }
}
