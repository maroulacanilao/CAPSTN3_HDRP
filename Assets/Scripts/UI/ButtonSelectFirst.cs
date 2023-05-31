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

    private void Reset()
    {
        button = GetComponent<Button>();
    }
    private void Awake()
    {
        if(button == null) button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        Canvas.ForceUpdateCanvases();
        button.Select();
        InputUIManager.OnMove.AddListener(Move);
        Canvas.ForceUpdateCanvases();
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
