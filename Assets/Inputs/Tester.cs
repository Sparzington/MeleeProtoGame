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

    //Camera
    private Camera cam;

    //Fields
    [Range(0f, 1f)]
    [SerializeField] private float DeadZone = 0.05f;
    private float StanceDeadZone;

    private Vector3 MousePos;
    private Vector3 ScreenCenter;

    public float Angle;
    void Start()
    {
        //Inputs
        _fightControls = new FightControls();

        //Stance Widget
        _stanceComponent = GetComponentInChildren<StanceRotate>();
        StanceDeadZone = Screen.width * DeadZone;
        ScreenCenter = new Vector2(Screen.width / 2, Screen.height / 2);

        cam = Camera.main;
    }
    public void OnStanceRotateGP(InputValue value)
    {
        Vector2 f = value.Get<Vector2>();
        Angle = GetAngle(f);
        _stanceComponent.UpdateRotation(Angle);
    }

    private void OnStanceRotateMNK(InputValue value)
    {       

        MousePos = value.Get<Vector2>();
        if (MouseOutDeadZone(MousePos))
        {
            Debug.Log("Outside");

            Angle = GetAngle(MousePos-ScreenCenter);
            MouseReset();
            _stanceComponent.UpdateRotation(Angle); 
        }
        else
        {
            Cursor.lockState = CursorLockMode.Confined;
        }        
    }
    private bool MouseOutDeadZone(Vector3 mouse)
    {
        Vector3 dist = mouse - ScreenCenter;

        return dist.magnitude > StanceDeadZone;
    }
    private void MouseReset()
    {
        if (Cursor.lockState != CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    private float GetAngle(Vector2 incoming)
    {
        //float angle = Mathf.Atan2(incoming.y - Vector2.right.y, incoming.x - Vector2.right.x) * 180 / Mathf.PI;
        float angle = Mathf.Atan2(incoming.y, incoming.x) * 180 / Mathf.PI;

        return angle;
    }

    
}
