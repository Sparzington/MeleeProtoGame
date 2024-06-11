using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFighter : MonoBehaviour
{
    //Fighter Component
    private Fighter _fighter;

    //Stance Widget
    private StanceRotate _stanceComponent;

    //Stance angle
    public float Angle;


    private void Awake()
    {
        //Fighter
        _fighter = GetComponent<Fighter>();
        _fighter.FighterInit();

        //Stance Widget
            //_stanceComponent = GetComponentInChildren<StanceRotate>();
    }

    public void EnemyLightAttack()
    {

    }

    public void EnemyHeavyAttack()
    {

    }


}
