using System.Collections;
using System.Collections.Generic;
using CustomEvent;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class AnimationEventReceiver : MonoBehaviour
{
    private Dictionary<string, UnityEvent> eventDictionary;

    private void Awake()
    {
        eventDictionary = new Dictionary<string, UnityEvent>();
    }

    public void AddListener(string eventName, UnityAction listener)
    {
        if (eventDictionary.TryGetValue(eventName, out UnityEvent unityEvent))
        {
            unityEvent.AddListener(listener);
        }
        else
        {
            unityEvent = new UnityEvent();
            unityEvent.AddListener(listener);
            eventDictionary.Add(eventName, unityEvent);
        }
    }

    public void RemoveListener(string eventName, UnityAction listener)
    {
        if (eventDictionary.TryGetValue(eventName, out UnityEvent unityEvent))
        {
            unityEvent.RemoveListener(listener);
        }
    }

    public void TriggerEvent(string eventName)
    {
        
        if (eventDictionary.TryGetValue(eventName, out UnityEvent unityEvent))
        {
            unityEvent.Invoke();
        }
    }
}
