using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New Fighter")]
public class FighterSO : ScriptableObject
{
    public int MaxHealth;
    public int LightDamage;
    public int HeavyDamage;
}
