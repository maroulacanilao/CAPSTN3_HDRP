using System;
using UnityEngine;

namespace ObjectPool
{
    public class Poolable : MonoBehaviour
    {
        
        private IPoolable[] poolables = Array.Empty<IPoolable>();
        private bool isInitialized;
        private Pool pool;
        
        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            poolables = GetComponentsInChildren<IPoolable>(true);
            isInitialized = true;
        }
        
        private void OnDestroy()
        {
            pool.Release(this);
        }
        
        public void SetPool(Pool pool_) => pool = pool_;

        public void OnSpawn()
        {
            if (!isInitialized) Initialize();

            foreach (var _t in poolables)
            {
                _t.OnSpawn();
            }
            gameObject.SetActive(true);
        }

        public void OnDeSpawn()
        {
            foreach (var _t in poolables)
            {
                _t.OnDeSpawn();
            }
            gameObject.SetActive(false);
        }

        public void Release()
        {
            pool.Release(this);
        }
    }
}