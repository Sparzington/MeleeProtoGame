using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideCursor : MonoBehaviour
{
    private void Awake()
    {
        Cursor.lockState= CursorLockMode.Locked; 
    }

    private void Update()
    {
        Cursor.visible = true;

    }
}
