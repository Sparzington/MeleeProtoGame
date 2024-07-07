using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FightState { IDLE, WINDUP, ATTACKING, COMBORECOVER, FULLRECOVER }

public class Fighter : MonoBehaviour, IDamageable, IFighter, ITargetable
{
    [Header("Fighter Data")]
    [SerializeField] private FighterSO FighterData;

    [Header ("Combo")]
    //Combos
    public Combo[] PossibleCombos;
    [SerializeField]private int CurrentComboIndex = 0;
    private int MaxComboIndex;
    private int doingIndex=-1;

    //Fighter Fields
    public int Health;
    private int MaxHealth = 5;
    private int LightDamage;
    private int HeavyDamage;
    public bool Invincible;
    public bool CanAttack;
    [SerializeField] private FightState CurrentState;

    [Header("Inputs")]
    //Input Buffers
    [SerializeField] private float inputBuffer = 0.6f;
    [SerializeField] private float fullRecoveryBuffer = 0.6f;
    private float currentBuffer;
    private bool DoFullRecovery;
    private bool DoComboRecovery;
    private bool CanCombo;
    
    //Attack stuff
    private Attack QueuedAttack;
    private Attack PreviousAttack;
    private ComboLevel _currentAttackLevel;

    //Movement 
    public bool EngageMovement { get; private set; }

    public FightState currentState
    {
        get
        {
            return CurrentState;
        }
    }

    [Header("Weapon")]
    //Weapon
    private HeldWeapon _heldWeapon; //Actual Weapon
    [SerializeField] private Collider HitBoxCollider;    //Set 'hitbox' collider

    public event EventHandler OnWeaponContact;

    public bool DebugWindow;
    private void Start()
    {
        if (HitBoxCollider != null)
        {
            DeactivateWeapon();
        }
    }
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

        ///A fighter cna be in x states:
        /// 
        /// IDLE
        ///     - Fighter can Attack
        ///     - All input timers start at 0.0f
        ///     - We check input for potential combo (even starting attack)
        ///         - Switch to attacking
        /// 
        /// ATTACKING
        ///     - Invoke attack
        ///     - Set input buffer timer
        ///     - Wait for 'Recover AnimEvent' switch to FighterState.RECOVERY
        ///     
        /// 
        /// RECOVERY
        ///     - Tick 'currentBuffer' timer
        ///     - Accept input during this window
        ///     
        ///     IF currentBuffer > inputBuffer
        ///         - reset currentBuffer timer
        ///         - Count up to FullRecoveryBuffer
        ///         - Accept no input
        ///         
        ///         - switch to idle when condition met
        /// 

        //Buffer input states
        switch (CurrentState)
        {
            case FightState.IDLE:
                currentBuffer = 0.0f;

                DoFullRecovery = false;
                DoComboRecovery = false;
                CanAttack = true;
                EngageMovement = true;

                break;
            case FightState.WINDUP:
                    //Nothing right now

                break;
            
            case FightState.ATTACKING:
                CanAttack = false;
                currentBuffer = 0.0f;
                EngageMovement = false;

                break;

            case FightState.COMBORECOVER:
                EngageMovement = true;

                CanCombo = currentBuffer < inputBuffer;

                if (!DoComboRecovery)
                {
                    DoComboRecovery = true;
                    AttackSlider.instance.SetTimer(inputBuffer);
                }

                if (currentBuffer > inputBuffer)
                {
                    currentBuffer = 0.0f;
                    CurrentState = FightState.FULLRECOVER;
                }
                else
                {
                    UpdateBufferValue();
                }
                break;
            
            case FightState.FULLRECOVER:
                
                CurrentComboIndex = 0;
                CanAttack = false;

                if (!DoFullRecovery)
                {
                    DoFullRecovery = true;
                    AttackSlider.instance.SetTimer(fullRecoveryBuffer);
                }

                if (currentBuffer > fullRecoveryBuffer)
                {
                    CurrentState = FightState.IDLE;
                }
                else
                {
                    
                    UpdateBufferValue(true);
                }

                break;
            default:
                break;
        }
    }

    private void LateUpdate()
    {
        if (DebugWindow)
        {
            AttackSlider.instance.UpdateComboCount(CurrentComboIndex);
            AttackSlider.instance.UpdateCanComboColor(CanCombo);
            AttackSlider.instance.UpdateIndexTest(doingIndex + 1);
        }
        
    }

    //Actions
    public void LightAttack(bool bash)
    {
        if (bash)
        {
            QueueNextAttack(AttackTier.LIGHT, ImpactType.BASH);
        }
        else
        {
            QueueNextAttack(AttackTier.LIGHT, ImpactType.BLADE);
        }
    }
    public void HeavyAttack()
    {
        QueueNextAttack(AttackTier.HEAVY, ImpactType.BLADE);
    }

    private void QueueNextAttack(AttackTier newTier, ImpactType type)
    {
        if (CanAttack && CurrentComboIndex < MaxComboIndex)
        {
            QueuedAttack = new Attack(newTier, type);

            //If we are queueing another attack
            if (CurrentComboIndex > 0 && CanCombo)
            {
                if (CheckForCombo())
                {
                    PreviousAttack = QueuedAttack;
                    CurrentComboIndex++;
                }
                else
                {
                    CanAttack = false;
                    PreviousAttack = null;
                    CurrentComboIndex = 0;
                }
            }
            else
            {
                //Starting fresh combo
                if (CheckForCombo())
                {
                    PreviousAttack = QueuedAttack;
                    CurrentComboIndex++;
                }
            }
        }
        else
        {
            CanAttack = false;
        }
    }
    public void Block()
    {
        throw new NotImplementedException();
    }

    private bool CheckForCombo()
    {
        //Take current [INDEX & TIER] and match to index 

        for (int i = 0; i < PossibleCombos.Length; i++)
        {
            if (PossibleCombos[i].ComboInput.Length-1 >= CurrentComboIndex)
            {
                if (PossibleCombos[i].ComboInput[CurrentComboIndex].Tier == QueuedAttack.Tier)
                {
                    if (CurrentComboIndex > 0)
                    {
                        if (PreviousAttack != null && 
                            PreviousAttack.Tier == PossibleCombos[i].ComboInput[CurrentComboIndex-1].Tier)
                        {
                            doingIndex = i;
                            _currentAttackLevel = PossibleCombos[i].ComboInput[CurrentComboIndex].Level;
                            return true;
                        }
                    }
                    else
                    {
                        doingIndex = i;
                        _currentAttackLevel = PossibleCombos[i].ComboInput[CurrentComboIndex].Level;
                        return true;
                    }
                }
            }            
        }

        doingIndex = -1;
        return false;
    }    

    //States
    public void WindUp() 
    {
        //Debug.Log("Windup");

        currentBuffer = 0.0f;
        CanAttack = false;
        CurrentState = FightState.WINDUP;


        AttackSlider.instance.SetTimer(0);

    }
    public void FollowThrough()
    {
        //Debug.Log("Attack");
        CurrentState = FightState.ATTACKING;

        switch (QueuedAttack.Type)
        {
            case ImpactType.BASH:
                
                break;
            case ImpactType.BLADE:
                ActivateWeapon();

                break;
            default:
                break;
        }

    }
    public void Recover()
    {
        //Debug.Log("Recover");

        CurrentState = FightState.COMBORECOVER;
        DeactivateWeapon();

        CanAttack = true;
        AttackSlider.instance.SetTimer(inputBuffer);        
    }

    public void FullRecover()
    {
        CanAttack = false;
        AttackSlider.instance.SetTimer(fullRecoveryBuffer);
    }


    private void UpdateBufferValue(bool fullRecover = false)
    {
        if (fullRecover)
        {
            if (currentBuffer < fullRecoveryBuffer)
            {
                currentBuffer += Time.deltaTime;
            }
            else
            {
                CanAttack = true;
                CurrentComboIndex= 0;
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

    #region Weapon stuff
    /// <summary>
    /// Used in Animation events to toggle On/Off weapon collider
    /// </summary>
    public void ActivateWeapon()
    {
        float x = transform.rotation.x;
        //_heldWeapon.ToggleCollider(true);
        HitBoxCollider.enabled = true;        
    }
    public void DeactivateWeapon()
    {
        //_heldWeapon.ToggleCollider(false);
        HitBoxCollider.enabled = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(HitBoxCollider == null)
        {
            return;
        }

        if (HitBoxCollider.enabled && other.TryGetComponent<IDamageable>(out IDamageable damageable))
        {
            Debug.Log($"Attacked: {other.name}");
            if (QueuedAttack.Type == ImpactType.BASH)
            {
                // TODO: (figure out what a 'bash' will do)
                return;
            }

            switch (QueuedAttack.Tier)
            {
                case AttackTier.LIGHT:
                    damageable.TakeDamage(LightDamage);

                    break;
                case AttackTier.HEAVY:
                    damageable.TakeDamage(HeavyDamage);

                    break;
                default:
                    break;
            }

            OnWeaponContact?.Invoke(this, EventArgs.Empty);
        }
    }
    #endregion

    //Health & Damage
    public void TakeDamage(int n)
    {
        Debug.Log($"Took {n} damage");
        Health -= n;
        if (Health < 0)
        {
            Health = -1;
            Die();
        }
    }
    public void Die()
    {
        Debug.Log("Died!!");
    }
}
