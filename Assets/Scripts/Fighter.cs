using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FightState { IDLE, ATTACKING }

public class Fighter : MonoBehaviour, IDamageable, IFighter
{

    [SerializeField] private FighterSO FighterData;

    //Combos
    public Combo[] PossibleCombos;
    private int CurrentComboIndex;
    private int MaxComboIndex;

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

        CurrentComboIndex = 0;
        MaxComboIndex = 0;

        for (int i = 0; i < PossibleCombos.Length; i++)
        {
            int max = PossibleCombos[i].ComboInput.Length;
            if (i==0)
            {
                MaxComboIndex = max;
                continue;
            }

            if (max > MaxComboIndex)
            {
                MaxComboIndex = max;
            }

        }

        CurrentState = FightState.IDLE;
    }

    //Actions
    public void LightAttack()
    {
        CurrentComboIndex++;
    }
    public void HeavyAttack()
    {
        CurrentComboIndex++;
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
    /// <summary>
    /// Used in Animation events to toggle On/Off weapon collider
    /// </summary>
    public void ActivateWeapon()
    {
        _heldWeapon.ToggleCollider(true);
    }
    public void DeactivateWeapon()
    {
        _heldWeapon.ToggleCollider(false);
    }
    
    //Health & Damage
    public void TakeDamage(int n)
    {

    }
    public void Die()
    {

    }
}
