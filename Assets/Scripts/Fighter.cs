using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FightState { IDLE, ATTACKING }

public class Fighter : MonoBehaviour, IDamageable, IFighter
{

    [SerializeField] private FighterSO FighterData;

    //Fighter Fields
    public int Health;
    private int MaxHealth = 5;
    private int LightDamage;
    private int HeavyDamage;
    public bool Invincible;
    public bool CanAttack;
    public FightState CurrentState;

    private HeldWeapon _heldWeapon;
    public void FighterInit()
    {
        _heldWeapon = GetComponentInChildren<HeldWeapon>();

        CanAttack = true;
        if (FighterData != null)
        {
            MaxHealth = FighterData.MaxHealth;
            Health = MaxHealth;

            LightDamage = FighterData.LightDamage;
            HeavyDamage = FighterData.HeavyDamage;
        }
        else
        {
            Debug.LogWarning("! No Fighter Data Set !");
        }

        CurrentState = FightState.IDLE;
    }

    //Actions
    public void LightAttack()
    {
        CurrentState = FightState.ATTACKING;
        ToggleWeapon(true);
    }
    public void HeavyAttack()
    {
        CurrentState = FightState.ATTACKING;
        ToggleWeapon(true);
    }
    public void Block()
    {

    }

    //States
    public void WindUp() 
    {
        Debug.Log("Windup");
    }
    public void FollowThrough()
    {
        Debug.Log("Attack");

    }
    public void Recover()
    {
        Debug.Log("Recover");

    }

    //Weapon
    public void ToggleWeapon(bool toggle)
    {
        _heldWeapon.ToggleCollider(toggle);
    }

    //Health & Damage
    public void TakeDamage(int n)
    {

    }
    public void Die()
    {

    }
}
