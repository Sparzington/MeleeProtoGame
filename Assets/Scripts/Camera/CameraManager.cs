using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    //Seperate
    [SerializeField] private CinemachineFreeLook _FreeLookCamera;
    [SerializeField] private CinemachineVirtualCamera _CombatCamera;

    private bool UpdateFreelokcPos;

    private void Awake()
    {
        if (_FreeLookCamera == null)
        {
            Debug.LogWarning("! Freelook camera not refenreced !");
        }

        if (_CombatCamera == null)
        {
            Debug.LogWarning("! Combat camera not refenreced !");
        }

        PlayerController.OnEngage += PlayerController_OnEngage;
        PlayerController.OnDisengage += PlayerController_OnDisengage;
    }

    private void PlayerController_OnDisengage(object sender, System.EventArgs e)
    {
        if (_FreeLookCamera != null && _CombatCamera != null)
        {
            ActivateThirdPersonCamera();
            _FreeLookCamera.ForceCameraPosition(_CombatCamera.transform.position, _CombatCamera.transform.rotation);
        }
    }

    private void PlayerController_OnEngage(object sender, Transform e)
    {
        if (_FreeLookCamera != null && _CombatCamera != null)
        {
            ActivateCombatCamera();

        }
    }
    private void ActivateCombatCamera()
    {
        _FreeLookCamera.Priority = 1;
        _CombatCamera.Priority = 2;
    }

    private void ActivateThirdPersonCamera()
    {
        _FreeLookCamera.Priority = 2;
        _CombatCamera.Priority = 1;
    }

}
