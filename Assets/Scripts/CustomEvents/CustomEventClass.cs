using System;
using System.Collections;
using System.Collections.Generic;

namespace CustomEvent
{
    public class Evt
    {
        private event Action _evt = delegate { };

        public void Invoke()
        {
            _evt?.Invoke();
        }

        public void AddListener(Action listener)
        {
            _evt -= listener;
            _evt += listener;
        }

        public void RemoveListener(Action listener)
        {
            _evt -= listener;
        }
        
        public IEnumerator WaitForEvent()
        {
            bool actionCalled = false;
            _evt += () => actionCalled = true;

            while (!actionCalled)
            {
                yield return null;
            }
        }
    }

    public class EvtParam<T> { public T param; }
    public class Evt<T>
    {
        private event Action<T> _evt = delegate { };

        public void Invoke(T param)
        {
            _evt?.Invoke(param);
        }

        public void AddListener(Action<T> listener)
        {
            _evt -= listener;
            _evt += listener;
        }

        public void RemoveListener(Action<T> listener)
        {
            _evt -= listener;
        }
        
        public IEnumerator WaitForEvent()
        {
            bool actionCalled = false;
            _evt += t => actionCalled = true;

            while (!actionCalled)
            {
                yield return null;
            }
        }

        public IEnumerator WaitForEventWithParam(EvtParam<T> param_)
        {
            bool actionCalled = false;
            _evt += t =>
            {
                param_.param = t;
                actionCalled = true;
            };

            while (!actionCalled)
            {
                yield return null;
            }
        }
    }

    public class EvtParam<T, U>
    {
        public T param1;
        public U param2;
    }
    
    public class Evt<T, U>
    {
        private event Action<T, U> _evt = delegate { };

        public void Invoke(T param1, U param2)
        {
            _evt?.Invoke(param1, param2);
        }

        public void AddListener(Action<T, U> listener)
        {
            _evt -= listener;
            _evt += listener;
        }

        public void RemoveListener(Action<T, U> listener)
        {
            _evt -= listener;
        }
        
        public IEnumerator WaitForEvent()
        {
            bool actionCalled = false;
            _evt += (t, u) => actionCalled = true;

            while (!actionCalled)
            {
                yield return null;
            }
        }
        
        public IEnumerator WaitForEventWithParam(EvtParam<T, U> param_)
        {
            bool actionCalled = false;
            _evt += (t, u) =>
            {
                param_.param1 = t;
                param_.param2 = u;
                actionCalled = true;
            };

            while (!actionCalled)
            {
                yield return null;
            }
        }
    }
}