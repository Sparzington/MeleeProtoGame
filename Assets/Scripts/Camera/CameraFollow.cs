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
    }

    private void PlayerController_OnEngage(object sender, Transform e)
    {
        _camera.LookAt = e;
    }
}
