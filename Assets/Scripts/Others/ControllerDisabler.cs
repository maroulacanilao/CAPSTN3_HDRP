using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using Player;
using UnityEngine;

public class ControllerDisabler : MonoBehaviour
{
    private void OnEnable()
    {
        InputManager.Instance.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        InputManager.Instance.gameObject.SetActive(true);
    }
}
