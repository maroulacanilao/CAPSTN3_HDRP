using System.Collections;
using System.Collections.Generic;
using BaseCore;
using Managers.SceneLoader;
using NaughtyAttributes;
using UnityEngine;

public class InteractableSceneLoader : InteractableObject
{
    [Scene] [SerializeField] private string sceneName;
    [SerializeField] private LoadSceneParameters loadSceneParameters;
    
    protected override void Interact()
    {
        SceneLoader.OnLoadScene.Invoke(loadSceneParameters);
    }
    
    protected override void Enter()
    {
        
    }
    
    protected override void Exit()
    {
        
    }
}
