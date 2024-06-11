using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackTier { LIGHT, HEAVY }
public enum ComboLevel { MID, FINISHER}

[Serializable]
public class Attack
{
    public AttackTier Tier;
    public ComboLevel Level;
    public Attack(AttackTier tier)
    {
        Tier = tier;
    }
}
