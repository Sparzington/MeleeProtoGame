using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class StanceRotate : MonoBehaviour
{
    private Vector3 MousePos;
    private Vector3 ScreenCenter;

    private Quaternion ParentQuaternion;

    public float Angle;

    [SerializeField] private GameObject arrowObj;

    private LineRenderer _linerender;

    [Range(0f, 1f)]
    [SerializeField] private float DeadZone = 0.05f;
    private float StanceDeadZone;

    public bool BBB;
    
    private void Start()
    {
        ParentQuaternion = transform.rotation;
        ParentQuaternion.x = 0.0f;
        ParentQuaternion.y = 0.0f;

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

            //
            //MouseAngleFromLastPos(MousePos, ScreenCenter);
            //arrowObj.transform.Rotate(transform.forward, Angle);
            //

            arrowObj.transform.localRotation = MouseAngleFromLastPos(MousePos, ScreenCenter);
        } 

        //Debug.Log(MouseOutDeadZone(MousePos));
    }    
    
    /// <summary>
    /// Checks if mouse is outside of set center deadzone.
    /// </summary>
    /// <param name="mouse"></param>
    /// <returns></returns>
    private bool MouseOutDeadZone(Vector3 mouse)
    {
        Vector3 dist = mouse - ScreenCenter;

        return dist.magnitude > StanceDeadZone;
    }

    /// <summary>
    /// Brings mouse back to center of scren when out of deadzone
    /// </summary>
    private void MouseReset()
    {
        if (Cursor.lockState != CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    /// <summary>
    /// Generates Quaternion rotation from screen center to current mouse position, based from transform.up
    /// </summary>
    /// <param name="mousePos"></param>
    /// <param name="center"></param>
    /// <returns></returns>
    private Quaternion MouseAngleFromLastPos(Vector3 mousePos, Vector3 center)
    {
        Vector3 dirFromCenter = mousePos - center;
        dirFromCenter = dirFromCenter.normalized;
        dirFromCenter.z = 0.0f;

        Angle = Vector3.Angle(transform.up, dirFromCenter);
        //Angle = angle;

        Quaternion newRot = Quaternion.FromToRotation(transform.up, dirFromCenter);
        newRot.x= 0.0f;
        newRot.y= 0.0f;

        return newRot;
    }
   
}
