using System;
using Managers;
using UnityEngine;

public class InteractableInputDisabler : MonoBehaviour
{
    private void OnEnable()
    {
        InputManager.InteractAction.Disable();
    }

    private void OnDisable()
    {
        InputManager.InteractAction.Enable();
    }
}