using System;
using System.Collections;
using System.Collections.Generic;
using CustomHelpers;
using Managers;
using Player;
using UnityEngine;

public class ControllerDisabler : MonoBehaviour
{
    private void OnEnable()
    {
        if(InputManager.Instance.IsEmptyOrDestroyed()) return;
        InputManager.Instance.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        if(InputManager.Instance.IsEmptyOrDestroyed()) return;
        InputManager.Instance.gameObject.SetActive(true);
    }
}
