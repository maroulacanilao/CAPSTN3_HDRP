using System;
using BaseCore;
using Character;
using CustomEvent;
using Managers;
using ScriptableObjectData;
using UnityEngine;

public class FarmSceneManager : Singleton<FarmSceneManager>
{
    [field: SerializeField] public GameObject player { get; private set; }
    [field: SerializeField] public EventQueueData eventQueueData { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        eventQueueData.InitializeQueue();
    }

    protected void OnEnable()
    {
        eventQueueData.ExecuteAllEvents();
        InputManager.Instance.enabled = false;
    }
    
    protected void OnDisable()
    {
        eventQueueData.ClearQueue();
        InputManager.Instance.enabled = true;
    }
}
