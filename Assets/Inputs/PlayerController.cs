using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Our input action Asset
    private FightControls _fightControls;

    private void Awake()
    {
        _fightControls = new FightControls();
    }
    private void OnLightAttack()
    {
        Debug.Log("Light attack pressed");
    }

    private void OnHeavyAttack()
    {
        Debug.Log("Heavy attack pressed");
    }
}
