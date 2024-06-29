using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class StanceRotate : MonoBehaviour
{
    private Vector3 MousePos;
    private Vector3 ScreenCenter;

    private Quaternion ParentQuaternion;

    private Vector3 GuardRotation;

    public float Angle;
    public float DebugAngle;

    [SerializeField] private GameObject arrowObj;

    private LineRenderer _linerender;

    [Range(0f, 1f)]
    [SerializeField] private float DeadZone = 0.05f;
    private float StanceDeadZone;
    
    private void Start()
    {
        GuardRotation = new Vector3();

        ParentQuaternion = transform.rotation;
        ParentQuaternion.x = 0.0f;
        ParentQuaternion.y = 0.0f;

        StanceDeadZone = Screen.width * DeadZone;
        ScreenCenter = new Vector2(Screen.width/2, Screen.height/2);
    }
    public bool IsOnRightSide()
    {
        return Angle < 90 && Angle > -90;
    }

    public void UpdateRotation(float angle)
    {
        GuardRotation.z = angle-90;
        arrowObj.transform.localEulerAngles = GuardRotation;
    }

    public void ToggleStanceUI(bool toggle)
    {
        Image[] i = transform.GetComponentsInChildren<Image>();
        foreach (var item in i)
        {
            item.enabled = toggle;
        }
    }
}
