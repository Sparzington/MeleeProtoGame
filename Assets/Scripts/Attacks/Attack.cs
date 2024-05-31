using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackTier { LIGHT, HEAVY }
public enum ComboLevel { START, MIDDLEFINISHER, FINISHER }

[Serializable]
public class Attack
{
    public AttackTier Tier;
    public ComboLevel _ComboLevel;

    public Attack(AttackTier tier, ComboLevel comboLevel)
    {
        Tier = tier;
        _ComboLevel = comboLevel;
    }
}
