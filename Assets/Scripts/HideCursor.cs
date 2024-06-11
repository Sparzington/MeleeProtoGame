using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideCursor : MonoBehaviour
{
    private void Awake()
    {
        Cursor.lockState= CursorLockMode.Locked;
        Application.targetFrameRate = 60;
    }

    private void Update()
    {
        Cursor.visible = true;

    }
}
