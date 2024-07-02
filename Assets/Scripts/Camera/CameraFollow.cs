using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private CinemachineVirtualCamera _camera;

    private void Awake()
    {
        _camera = GetComponent<CinemachineVirtualCamera>();

        PlayerController.OnEngage += PlayerController_OnEngage;
        PlayerController.OnDisengage += PlayerController_OnDisengage;
    }

    private void PlayerController_OnDisengage(object sender, System.EventArgs e)
    {
        _camera.LookAt = null;
    }

    private void PlayerController_OnEngage(object sender, Transform e)
    {
        _camera.LookAt = e;
    }
    private void OnDestroy()
    {
        PlayerController.OnEngage -= PlayerController_OnEngage;
        PlayerController.OnDisengage -= PlayerController_OnDisengage;
    }
}
