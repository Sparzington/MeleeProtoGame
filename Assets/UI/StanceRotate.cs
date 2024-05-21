using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StanceRotate : MonoBehaviour
{
    private Vector3 MousePos;
    private Vector3 ScreenCenter;

    [SerializeField] GameObject arrowObj;

    private LineRenderer _linerender;

    private void Start()
    {
        ScreenCenter = new Vector2(Screen.width/2, Screen.height/2);
    }

    private void Update()
    {
        MouseReset();

        MousePos = Input.mousePosition;

        if (Input.GetAxisRaw("Mouse X") != 0 ||
            Input.GetAxisRaw("Mouse Y") != 0 )
        {
            Cursor.lockState = CursorLockMode.Confined;
            MouseHandling(MousePos);
        }

        Debug.Log(Cursor.lockState);
    }    

    private void MouseReset()
    {
        if (Cursor.lockState != CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    private void MouseHandling(Vector3 mousePos)
    {
        Vector3 dirFromCenter = mousePos - ScreenCenter;
        dirFromCenter = dirFromCenter.normalized;

        float angle = Vector3.Angle(transform.up, dirFromCenter);
        //Debug.Log(angle);

        Quaternion newRot = Quaternion.AngleAxis(angle, transform.forward);
        
        arrowObj.transform.rotation = newRot;
    }

    
}
