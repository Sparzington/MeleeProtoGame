using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreColliders : MonoBehaviour
{
    private void Awake()
    {
        Collider caster = transform.parent.GetComponent<Collider>();
        Collider col = caster.GetComponent<Collider>();
        Physics.IgnoreCollision(caster, col);
    }
}
