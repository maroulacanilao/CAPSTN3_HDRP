using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(-1)]
public class DiscardButton : MonoBehaviour
{
    public static DiscardButton discardButton;
    public Button button;
    
    private void Awake()
    {
        discardButton = this;
        button = GetComponent<Button>();
    }
}
