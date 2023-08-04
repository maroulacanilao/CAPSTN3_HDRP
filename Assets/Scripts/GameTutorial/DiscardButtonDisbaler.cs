using System;
using System.Collections;
using System.Collections.Generic;
using CustomHelpers;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(99)]
public class DiscardButtonDisbaler : MonoBehaviour
{
    private void OnEnable()
    {
        if(DiscardButton.discardButton.IsEmptyOrDestroyed()) return;
        DiscardButton.discardButton.button.interactable = false;
    }
    
    private void OnDisable()
    {
        if(DiscardButton.discardButton.IsEmptyOrDestroyed()) return;
        DiscardButton.discardButton.button.interactable = true;
    }
    
    private void OnDestroy()
    {
        if(DiscardButton.discardButton.IsEmptyOrDestroyed()) return;
        DiscardButton.discardButton.button.interactable = true;
    }
}
