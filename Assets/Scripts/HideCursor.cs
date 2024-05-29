using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideCursor : MonoBehaviour
{
    private void LateUpdate()
    {
        if (Cursor.visible == true)
        {
            Cursor.visible = false;
        }
    }
}
