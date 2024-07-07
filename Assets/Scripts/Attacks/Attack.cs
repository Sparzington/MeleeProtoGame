using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackTier { LIGHT, HEAVY }
public enum ComboLevel { MID, FINISHER}

public enum ImpactType { BASH, BLADE}

[Serializable]
public class Attack
{
    public AttackTier Tier;
    public ComboLevel Level;
    public ImpactType Type;
    public Attack(AttackTier tier, ImpactType type)
    {
        Tier = tier;
        Type = type;    
    }
}
