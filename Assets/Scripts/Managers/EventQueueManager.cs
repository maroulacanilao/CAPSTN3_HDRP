using System;
using System.Collections.Generic;
using BaseCore;
using CustomEvent;

namespace Managers
{
    public class EventQueueManager : SingletonPersistent<EventQueueManager>
    {
        private readonly Queue<Action> eventQueues = new Queue<Action>();
    
        private static readonly Evt<Action> OnAddEventToQueue = new Evt<Action>();

        protected override void Awake()
        {
            base.Awake();
            OnAddEventToQueue.AddListener(AddEvent);
        }

        private void OnDestroy()
        {
            OnAddEventToQueue.RemoveListener(AddEvent);
        }

        private void AddEvent(Action action_) => eventQueues.Enqueue(action_);

        private void InvokeQueuedEvents()
        {
            int _count = eventQueues.Count;

            for (int i = 0; i < _count; i++)
            {
                eventQueues.Dequeue().Invoke();
            }
        }
    }
}