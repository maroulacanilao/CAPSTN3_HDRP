using System.Collections.Generic;
using CustomHelpers;
using UnityEngine;

namespace ObjectPool
{
    public class Pool
    {
        private readonly Queue<Poolable> standbyQueue = new Queue<Poolable>();
        private readonly HashSet<Poolable> activeSet = new HashSet<Poolable>();
        private readonly Poolable prefab;
        private readonly Transform root;
        
        public int standbyCount => standbyQueue.Count;
        public int activeCount => activeSet.Count;
        
        public Pool(Poolable prefab_, Transform root_, int count_ = 1)
        {
            prefab = prefab_;
            root = root_;
            for(int i = 0; i < count_; i++)
            {
                CreateInstance();
            }
        }

        private Poolable CreateInstance()
        {
            var _obj = Object.Instantiate(prefab, root);
            _obj.OnDeSpawn();
            standbyQueue.Enqueue(_obj);
            _obj.SetPool(this);
            return _obj;
        }
        
        #region Getters

        public GameObject Get()
        {
            while (true)
            {
                var _obj = standbyQueue.Count > 0 ? standbyQueue.Dequeue() : Object.Instantiate(prefab, root);

                if (_obj == null) continue;
                if (_obj.IsDestroyed()) continue;
                
                activeSet.Add(_obj);
                _obj.OnSpawn();
                return _obj.gameObject;
            }
        }

        public GameObject Get(Transform parent_)
        {
            var _obj = Get();
            _obj.transform.SetParent(parent_);
            return _obj;
        }
        
        public GameObject Get(Vector3 position_, Quaternion rotation_)
        {
            var _obj = Get();
            _obj.transform.SetPositionAndRotation(position_, rotation_);
            return _obj;
        }
        
        public GameObject Get(Vector3 position_, Quaternion rotation_, Transform parent_)
        {
            var _obj = Get();
            _obj.transform.SetPositionAndRotation(position_, rotation_);
            _obj.transform.SetParent(parent_);
            return _obj;
        }
        
        public T Get<T>() where T : Component
        {
            return Get().GetComponent<T>();
        }
        
        public T Get<T>(Transform parent_) where T : Component
        {
            return Get(parent_).GetComponent<T>();
        }
        
        public T Get<T>(Vector3 position_, Quaternion rotation_) where T : Component
        {
            return Get(position_, rotation_).GetComponent<T>();
        }
        
        public T Get<T>(Vector3 position_, Quaternion rotation_, Transform parent_) where T : Component
        {
            return Get(position_, rotation_, parent_).GetComponent<T>();
        }
        
        #endregion

        public void Release(Poolable poolableObject_)
        {
            if(poolableObject_ == null) return;
            if(poolableObject_.IsDestroyed()) return;
            if (!activeSet.Contains(poolableObject_)) return;
            
            activeSet.Remove(poolableObject_);
            poolableObject_.OnDeSpawn();
            standbyQueue.Enqueue(poolableObject_);
            poolableObject_.transform.SetParent(root);
        }
        
        public void Release(GameObject poolableObject_)
        {
            if(poolableObject_ == null) return;
            Release(poolableObject_.GetComponent<Poolable>());
        }

        public void DestroyPool(bool willDestroyActive = true)
        {
            int _standbyQueueCount = standbyQueue.Count;

            for (int i = 0; i < _standbyQueueCount; i++)
            {
                var _obj = standbyQueue.Dequeue();
                if (_obj == null || _obj.IsDestroyed()) continue;
                Object.Destroy(_obj);
            }
            standbyQueue.Clear();
            
            if(!willDestroyActive) return;
            
            foreach (var _obj in activeSet)
            {
                if (_obj == null || _obj.IsDestroyed()) continue;
                Object.Destroy(_obj.gameObject);
            }
            activeSet.Clear();
        }
    }
}