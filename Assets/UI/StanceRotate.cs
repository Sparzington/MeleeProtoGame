using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class StanceRotate : MonoBehaviour
{
    private Vector3 MousePos;
    private Vector3 ScreenCenter;

    public float Angle;

    [SerializeField] private GameObject UI_Parent;
    [SerializeField] private GameObject arrowObj;

    private LineRenderer _linerender;

    [Range(0f, 1f)]
    [SerializeField] private float DeadZone = 0.05f;
    private float StanceDeadZone;
    
    private void Start()
    {
        Cursor.visible = false;
        StanceDeadZone = Screen.width * DeadZone;
        ScreenCenter = new Vector2(Screen.width/2, Screen.height/2);
    }
    private void Update()
    {
        MousePos = Input.mousePosition;
        Cursor.lockState = CursorLockMode.Confined;
        //-----------------------------

        if (MouseOutDeadZone(MousePos))
        {
            MouseReset();
            arrowObj.transform.rotation = MouseAngleFromLastPos(MousePos, ScreenCenter);
        } 

        //Debug.Log(MouseOutDeadZone(MousePos));
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
    private Quaternion MouseAngleFromLastPos(Vector3 mousePos, Vector3 center)
    {
        Vector3 dirFromCenter = mousePos - center;
        dirFromCenter = dirFromCenter.normalized;
        dirFromCenter.z = 0.0f;

        //float angle = Vector3.Angle(transform.up, dirFromCenter);
        //Angle = angle;
        
        Quaternion newRot = Quaternion.FromToRotation(transform.up, dirFromCenter);

        return newRot;
    }
   
}
