using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainEventSystem : MonoBehaviour
{
    [SerializeField] private EventSystem mainEventSystem;

    private void Awake()
    {
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
    }

    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= OnActiveSceneChanged;
    }

    private void OnActiveSceneChanged(Scene current, Scene next)
    {
        var _systems = FindObjectsOfType<EventSystem>();

        for (int i = _systems.Length - 1; i >= 0; i--)
        {
            if(_systems[i] == mainEventSystem) continue;
            
            Destroy(_systems[i].gameObject);
        }
        
        EventSystem.current = mainEventSystem;
    }
}
