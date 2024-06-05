using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackTier { LIGHT, HEAVY }

[Serializable]
public class Attack
{
    public AttackTier Tier;

    public Attack(AttackTier tier)
    {
        Tier = tier;
    }
}
