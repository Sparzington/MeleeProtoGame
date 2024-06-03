using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Tester : MonoBehaviour
{
    //Our input action Asset
    private FightControls _fightControls;

    //Stance Widget
    private StanceRotate _stanceComponent;

    private Camera cam;

    public float Angle;
    void Start()
    {
        //Inputs
        _fightControls = new FightControls();

        //Stance Widget
        _stanceComponent = GetComponentInChildren<StanceRotate>();

        cam = Camera.main;
    }

    public void OnStanceRotateGP(InputValue value)
    {
        Vector2 f = value.Get<Vector2>();
        Angle = GetAngle(f.normalized);
    }

    void Update()
    {
        //Vector2 f = _fightControls.PlayerStance.TestStance.ReadValue<Vector2>();
        //Vector2 ff = cam.ScreenToWorldPoint(f);

        //Debug.Log(ff);
    }

    private float GetAngle(Vector2 incoming)
    {
        float angle = Mathf.Atan2(incoming.y - Vector2.right.y, incoming.x - Vector2.right.x) * 180 / Mathf.PI;


        return angle;
    }
}
