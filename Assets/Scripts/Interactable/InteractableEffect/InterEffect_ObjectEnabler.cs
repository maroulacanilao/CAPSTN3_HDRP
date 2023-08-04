using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterEffect_ObjectEnabler : MonoBehaviour,IInteractableEffect
{
    [SerializeField] private List<GameObject> objectToEnable;

    private void Reset()
    {
        objectToEnable = new List<GameObject>();
        var _ghost = transform.Find("Interactable Prompt");
        if (_ghost != null) objectToEnable.Add(_ghost.gameObject);
    }

    private void OnEnable()
    {
        EnableObjects(false);
    }
    
    public void OnInteract()
    {
        
    }
    public void OnEnter()
    {
        EnableObjects(true);
    }
    public void OnExit()
    {
        EnableObjects(false);
    }

    private void EnableObjects(bool willEnable_)
    {
        foreach (var obj in objectToEnable)
        {
            obj.SetActive(willEnable_);
        }
    }
}
