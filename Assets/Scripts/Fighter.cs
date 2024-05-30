using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FightState { IDLE, ATTACKING }

public class Fighter : MonoBehaviour, IDamageable, IFighter
{
    public int Health;
    private int MaxHealth;

    public bool Invincible;
    public float AttackDelay;
    public bool CanAttack;
    public FightState CurrentState;


    public void FighterInit()
    {
        Health = MaxHealth;
        Invincible = false;
        CurrentState = FightState.IDLE;
        CanAttack = true;
    }

    //Actions
    public void LightAttack()
    {
        CanAttack = false;
    }
    public void HeavyAttack()
    {
        CanAttack = false;

    }
    public void Block()
    {

    }

    //Health & Damage
    public void TakeDamage(int n)
    {

    }
    public void Die()
    {

    }
}
