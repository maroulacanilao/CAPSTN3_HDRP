using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace ScriptableObjectData
{
    [CreateAssetMenu(menuName = "ScriptableObjects/EventQueueData", fileName = "EventQueueData")]
    public class EventQueueData : ScriptableObject
    {
        private SerializedDictionary<string, Queue<Action>> eventQueues = new SerializedDictionary<string, Queue<Action>>();

        public void InitializeQueue()
        {
            eventQueues = new SerializedDictionary<string, Queue<Action>>();
        }
        
        public void AddEvent(string key_, Action action)
        {
            if (eventQueues.TryGetValue(key_ , out var _queue))
            {
                _queue ??= new Queue<Action>();
                _queue.Enqueue(action);
                return;
            }
            
            var _newQueue = new Queue<Action>();
            _newQueue.Enqueue(action);
            eventQueues.Add(key_, _newQueue);
        }
        
        public void ExecuteEvents(string key_)
        {
            if (!eventQueues.TryGetValue(key_, out var _queue))
            {
                return;
            }

            if (_queue == null)
            {
                return;
            }
            
            while (_queue.Count > 0)
            {
                _queue.Dequeue().Invoke();
            }
        }

        public void ClearQueue(string key_)
        {
            if(!eventQueues.TryGetValue(key_, out var _queue)) return;
            
            if (_queue == null) _queue = new Queue<Action>();
            _queue.Clear();
        }
        
        public void ClearAllQueues()
        {
            eventQueues.Clear();
        }
    }
}
