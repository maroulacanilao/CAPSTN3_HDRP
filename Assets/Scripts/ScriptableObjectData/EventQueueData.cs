using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjectData
{
    [CreateAssetMenu(menuName = "ScriptableObjects/EventQueueData", fileName = "EventQueueData")]
    public class EventQueueData : ScriptableObject
    {
        public Queue<Action> eventQueue { get; private set; } = new Queue<Action>();
        
        public void InitializeQueue()
        {
            eventQueue = new Queue<Action>();
        }
        
        public void AddEvent(Action action)
        {
            eventQueue.Enqueue(action);
        }
        
        public void ExecuteAllEvents()
        {
            if(eventQueue.Count <= 0) return;
            var _eventsNum = eventQueue.Count;
            
            for (int i = 0; i < _eventsNum; i++)
            {
                eventQueue.Dequeue()?.Invoke();
                if(eventQueue.Count <= 0) break;
            }
            ClearQueue();
        }
        
        public void ClearQueue()
        {
            eventQueue.Clear();
        }
    }
}
