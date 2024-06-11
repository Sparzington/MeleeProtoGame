using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HeldWeapon : MonoBehaviour
{
    private Collider WeaponCollider;

    public event EventHandler<Fighter> OnWeaponStrike;
    private void Awake()
    {
        WeaponCollider = GetComponent<Collider>();

        //Physics.IgnoreCollision(gameObject.GetComponentInParent<Collider>(), WeaponCollider, true);

        //ToggleCollider(false);
    }

    public void ToggleCollider(bool toggle)
    {
        if (WeaponCollider != null)
        {
            WeaponCollider.enabled = toggle;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Fighter>(out Fighter f))
        {
            OnWeaponStrike?.Invoke(this, f);
        }
    }
}
