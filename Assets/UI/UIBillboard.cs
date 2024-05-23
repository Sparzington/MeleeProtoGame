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
        transform.LookAt(FacingCamera.transform.position);
    }
}
