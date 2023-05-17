using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseEnabler : MonoBehaviour
{
    private void OnEnable()
    {
        Cursor.visible = true;
    }

    private void OnDisable()
    {
        Cursor.visible = false;
    }
}
