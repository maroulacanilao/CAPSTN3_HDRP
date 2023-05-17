using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonSelectFirst : MonoBehaviour
{
    private Button button;
    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        button.Select();
        InputUIManager.OnMove.AddListener(Move);
    }

    private void OnDisable()
    {
        InputUIManager.OnMove.RemoveListener(Move);
    }

    private void Move(InputAction.CallbackContext context_)
    {
        if(!gameObject.activeSelf) return;
        if(EventSystem.current.currentSelectedGameObject !=null) return;
        button.Select();
    }
}
