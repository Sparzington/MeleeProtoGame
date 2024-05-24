using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBillboard : MonoBehaviour
{
    private GameObject FacingCamera;

    private void Awake()
    {
        FacingCamera = GetComponent<Canvas>().worldCamera.gameObject;
    }

    private void OnEnable()
    {
        Vector3 faceDirection = transform.position - FacingCamera.transform.position;

        transform.transform.rotation = Quaternion.LookRotation(faceDirection.normalized);
    }
}
