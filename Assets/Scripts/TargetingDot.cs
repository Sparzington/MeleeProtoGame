using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetingDot : MonoBehaviour
{
    [SerializeField] private GameObject DotUI;
    private Transform TargetTransform;

    public static TargetingDot _instance;
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (DotUI != null)
        {
            DotUI.SetActive(false);
        }
    }

    private void LateUpdate()
    {
        if (TargetTransform != null)
        {
            DotUI.transform.position = TargetTransform.position;
        }
    }
    public void SetTarget(Transform target)
    {
        TargetTransform = target;
        DotUI.SetActive(true);
    }

    public void DisableTarget()
    {
        TargetTransform = null;
        DotUI.transform.position = Vector3.zero;
        DotUI.SetActive(false);
    }
}
