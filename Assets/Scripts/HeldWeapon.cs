using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeldWeapon : MonoBehaviour
{
    private Collider WeaponCollider;
    private void Awake()
    {
        WeaponCollider = GetComponent<Collider>();
        ToggleCollider(false);
    }

    public void ToggleCollider(bool toggle)
    {
        if (WeaponCollider != null)
        {
            WeaponCollider.enabled = toggle;
        }
    }
}
