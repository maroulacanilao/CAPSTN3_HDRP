using System.Collections;
using System.Collections.Generic;
using BaseCore;
using Managers;
using Managers.SceneLoader;
using NaughtyAttributes;
using UnityEngine;

public class InteractableSceneLoader : InteractableObject
{
    [SerializeField] private LoadSceneParameters loadSceneParameters;
    [SerializeField] private bool changeFarmLoadType;
    [SerializeField] [ShowIf("changeFarmLoadType")] private FarmLoadType farmLoadType;
    
    protected override void Interact()
    {
        if(changeFarmLoadType) GameManager.Instance.GameDataBase.sessionData.farmLoadType = farmLoadType;
        SceneLoader.OnLoadScene.Invoke(loadSceneParameters);
    }
    
    protected override void Enter()
    {
        
    }
    
    protected override void Exit()
    {
        
    }
}
