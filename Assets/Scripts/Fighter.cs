using System;
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

    //Input Buffers
    [SerializeField] private float inputBuffer = 0.6f;
    [SerializeField] private float fullRecoveryBuffer = 0.6f;
    private float currentBuffer;
    private bool DoFullRecovery;

    private Attack QueuedAttack;
    private Attack PreviousAttack;

    //Weapon
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

    private void Update()
    {
        UpdateBufferValue();
    }

    //Actions
    public void LightAttack()
    {
        if (CanAttack)
        {
            QueuedAttack = new Attack(AttackTier.LIGHT);
            if (CurrentComboIndex < MaxComboIndex)
            {

                if (CurrentComboIndex > 0 && currentBuffer < inputBuffer)
                {
                    if (CheckForCombo())
                    {
                        Debug.Log("IS valid");
                        CurrentComboIndex++;
                        currentBuffer = 0.0f;
                    }
                    else
                    {
                        Debug.Log("NOT valid");
                    }
                }
                else
                {
                    //INITIAL attack
                    currentBuffer = 0.0f;
                }
            }
        }
        
    }
    public void HeavyAttack()
    {
        QueuedAttack = new Attack(AttackTier.HEAVY);

        //If Valid input
        if (true)
        {
            PreviousAttack = QueuedAttack;
            CurrentComboIndex++;
        }
    }
    public void Block()
    {
        throw new NotImplementedException();
    }

    private bool CheckForCombo()
    {
        for (int i = 0; i < PossibleCombos.Length; i++)
        {
            if (PossibleCombos[i].ComboInput.Length-1 >= CurrentComboIndex)
            {
                if (PossibleCombos[i].ComboInput[CurrentComboIndex].Tier == QueuedAttack.Tier)
                {
                    return true;
                }
            }            
        }

        return false;
    }

    //States
    public void WindUp() 
    {
        Debug.Log("Windup");
        CanAttack = false;
    }
    public void FollowThrough()
    {
        Debug.Log("Attack");

    }
    public void Recover()
    {
        Debug.Log("Recover");

        CanAttack = true;
        AttackSlider.instance.SetTimer(inputBuffer);        
    }

    public void FullRecover()
    {
        CanAttack = false;
        DoFullRecovery = true;
        currentBuffer = 0.0f;
        AttackSlider.instance.SetTimer(fullRecoveryBuffer);
    }


    private void UpdateBufferValue()
    {
        if (DoFullRecovery)
        {
            if (currentBuffer < fullRecoveryBuffer)
            {
                currentBuffer += Time.deltaTime;
            }
            else
            {
                CanAttack = true;
                DoFullRecovery = false;
            }
        }
        else
        {
            if (currentBuffer < inputBuffer)
            {
                currentBuffer += Time.deltaTime;
            }
        }        
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
        throw new NotImplementedException();
    }
    public void Die()
    {
        throw new NotImplementedException();
    }
}
