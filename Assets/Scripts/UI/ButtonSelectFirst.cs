using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonSelectFirst : MonoBehaviour
{
    private Button button;
    private SelectableMenuButton selectableMenuButton;

    private void Reset()
    {
        button = GetComponent<Button>();
    }
    private void Awake()
    {
        if(button == null) button = GetComponent<Button>();
        TryGetComponent(out selectableMenuButton);
        EventSystem.current.firstSelectedGameObject = button.gameObject;
    }

    private void OnEnable()
    {
        button.Select();
        if(selectableMenuButton != null) selectableMenuButton.button.Select();
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
