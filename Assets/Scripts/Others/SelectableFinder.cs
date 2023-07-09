using System;
using System.Collections;
using System.Collections.Generic;
using CustomHelpers;
using Fungus;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SelectableFinder : MonoBehaviour
{
    private Camera mainCamera;
    private void OnEnable()
    {
        InputUIManager.OnMove.AddListener(OnMove);
        mainCamera = gameObject.scene.GetFirstMainCameraInScene();
    }
    
    private void OnDisable()
    {
        InputUIManager.OnMove.RemoveListener(OnMove);
    }
    
    
    private void OnMove(InputAction.CallbackContext context_)
    {
        var _current = EventSystem.current.currentSelectedGameObject;
        if(_current != null) return;
        if(_current.gameObject.activeInHierarchy) return;

        var _newCurr = FindNearestButton();
        
        if (_newCurr == null) return;

        _newCurr.Select();
        EventSystem.current.SetSelectedGameObject(_newCurr.gameObject);
    }
    
    private void OnMove(Vector2 vector2)
    {
        var _btn = EventSystem.current.currentSelectedGameObject;
        if (_btn == null) return;
        var _btnComponent = _btn.GetComponent<Selectable>();
        if (_btnComponent == null) return;
        var _btnNavigation = _btnComponent.navigation;
        var _btnUp = _btnNavigation.selectOnUp;
        var _btnDown = _btnNavigation.selectOnDown;
        var _btnLeft = _btnNavigation.selectOnLeft;
        var _btnRight = _btnNavigation.selectOnRight;
        
        if (vector2 == Vector2.up)
        {
            if (_btnUp != null)
            {
                _btnUp.Select();
            }
        }
        else if (vector2 == Vector2.down)
        {
            if (_btnDown != null)
            {
                _btnDown.Select();
            }
        }
        else if (vector2 == Vector2.left)
        {
            if (_btnLeft != null)
            {
                _btnLeft.Select();
            }
        }
        else if (vector2 == Vector2.right)
        {
            if (_btnRight != null)
            {
                _btnRight.Select();
            }
        }
    }
    
    private Selectable FindNearestButton()
    {
        var _buttonArr = FindObjectsOfType<Selectable>();
        Selectable _nearestButton = null;
        var _nearestDistance = Mathf.Infinity;
        var _screenTopLeft = Vector2.zero;

        foreach (var _button in _buttonArr)
        {
            if(!_button.gameObject.activeInHierarchy) continue;
            
            var _worldToScreenPoint = RectTransformUtility.WorldToScreenPoint(mainCamera, _button.transform.position);
            var _distance = Vector2.Distance(_worldToScreenPoint, _screenTopLeft);

            if (_distance >= _nearestDistance) continue;
            
            _nearestDistance = _distance;
            _nearestButton = _button;
        }

        return _nearestButton;
    }
}
