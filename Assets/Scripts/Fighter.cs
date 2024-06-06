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
    [SerializeField]private int CurrentComboIndex = -1;
    private int MaxComboIndex;
    private int doingIndex=-1;

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
    private bool CanCombo;
    

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

        CurrentComboIndex = -1;
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
        CanCombo = !DoFullRecovery & currentBuffer < inputBuffer;
        UpdateBufferValue();
    }

    private void LateUpdate()
    {
        AttackSlider.instance.UpdateComboCount(CurrentComboIndex);
        AttackSlider.instance.UpdateCanComboColor(CanCombo);
        AttackSlider.instance.UpdateIndexTest(doingIndex+1);
    }

    //Actions
    public void LightAttack()
    {
        QueueNextAttack(AttackTier.LIGHT);
    }
    public void HeavyAttack()
    {
        QueueNextAttack(AttackTier.HEAVY);
    }

    private void QueueNextAttack(AttackTier newTier)
    {
        if (CanAttack)
        {
            QueuedAttack = new Attack(newTier);

            //Check if we are past the max input count
            if (CurrentComboIndex < MaxComboIndex)
            {
                //If we are queueing another attack
                if (CurrentComboIndex > -1 && CanCombo)
                {
                    if (CheckForCombo())
                    {
                        Debug.Log("IS valid");
                        PreviousAttack = QueuedAttack;
                        CurrentComboIndex++;
                    }
                    else
                    {
                        Debug.Log("NOT valid");
                        CanAttack = false;
                        PreviousAttack = null;
                        CurrentComboIndex = -1;
                    }
                }
                else
                {
                    //If we are starting a FRESH combo

                    PreviousAttack = QueuedAttack;
                    CurrentComboIndex++;
                }
            }
        }

        //AttackSlider.instance.UpdateComboCount(CurrentComboIndex);
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
                if (CurrentComboIndex>0)
                {
                    if (PossibleCombos[i].ComboInput[CurrentComboIndex].Tier == QueuedAttack.Tier &&
                    PreviousAttack == PossibleCombos[i].ComboInput[CurrentComboIndex - 1])
                    {
                        doingIndex = i;
                        return true;
                    }
                    else
                    {
                        if (PossibleCombos[i].ComboInput[CurrentComboIndex].Tier == QueuedAttack.Tier)
                        {
                            doingIndex = i;
                            return true;
                        }
                    }
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
        currentBuffer = 0.0f;

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
                CurrentComboIndex= -1;
                PreviousAttack = null;
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
