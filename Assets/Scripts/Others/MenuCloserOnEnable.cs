using System;
using System.Collections;
using System.Collections.Generic;
using UI.Farming;
using UnityEngine;

public class MenuCloserOnEnable : MonoBehaviour
{
    public bool onEnable = true;
    public bool onDisable;
    
    private void OnEnable()
    {
        if (onEnable)
        {
            //PlayerMenuManager.OnCloseAllUI.Invoke();
            RevisedPlayerMenuManager.OnCloseAllUIRevised.Invoke();
        }
    }
    
    private void OnDisable()
    {
        if (onDisable)
        {
            //PlayerMenuManager.OnCloseAllUI.Invoke();
            RevisedPlayerMenuManager.OnCloseAllUIRevised.Invoke();
        }
    }
}
