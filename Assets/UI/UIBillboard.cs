using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBillboard : MonoBehaviour
{
    //Main camera
    private GameObject FacingCamera;

    private Vector3 FaceDirection;

    private void Awake()
    {
        FacingCamera = GetComponent<Canvas>().worldCamera.gameObject;
    }

    private void Update()
    {
        FaceDirection = transform.position - FacingCamera.transform.position;
        transform.rotation = Quaternion.LookRotation(FaceDirection.normalized);

    }
}
