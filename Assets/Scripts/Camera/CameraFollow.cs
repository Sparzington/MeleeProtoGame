using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private CinemachineVirtualCamera _camera;

    //Targeting aim
    private Transform TargetTransform;


    private void Awake()
    {
        _camera = GetComponent<CinemachineVirtualCamera>();

        PlayerController.OnEngage += PlayerController_OnEngage;
        PlayerController.OnDisengage += PlayerController_OnDisengage;
    }

    private void PlayerController_OnDisengage(object sender, System.EventArgs e)
    {
        TargetTransform = null;
        _camera.LookAt = null;
    }

    private void PlayerController_OnEngage(object sender, Transform e)
    {
        TargetTransform = e;
        _camera.LookAt = TargetTransform;
    }
    private void OnDestroy()
    {
        PlayerController.OnEngage -= PlayerController_OnEngage;
        PlayerController.OnDisengage -= PlayerController_OnDisengage;
    }    
}
