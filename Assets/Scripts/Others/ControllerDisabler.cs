using System;
using System.Collections;
using System.Collections.Generic;
using CustomHelpers;
using Managers;
using Player;
using UnityEngine;

public class ControllerDisabler : MonoBehaviour
{
    private bool isEnabled;
    private void OnEnable()
    {
        isEnabled = true;
        InputManager.DisableInput();
    }

    private void OnDisable()
    {
        isEnabled = false;
        InputManager.EnableInput();
    }

    private void Update()
    {
        if(isEnabled) InputManager.DisableInput();
        else InputManager.EnableInput();
    }
}
