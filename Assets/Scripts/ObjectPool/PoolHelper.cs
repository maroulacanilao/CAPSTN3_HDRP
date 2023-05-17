using UnityEngine;

namespace ObjectPool
{
    public static class PoolHelper
    {
        public static void CreatePool(this GameObject prefab_, int count_ = 1)
        {
            PoolManager.Instance.CreatePool(prefab_, count_);
        }
        
        public static GameObject GetInstance(this GameObject prefab_)
        {
            return PoolManager.GetInstance(prefab_);
        }
        
        public static GameObject GetInstance(this GameObject prefab_, Transform parent_)
        {
            return PoolManager.GetInstance(prefab_,parent_);
        }
        
        public static GameObject GetInstance(this GameObject prefab_, Vector3 position_, Quaternion rotation_)
        {
            return PoolManager.GetInstance(prefab_,position_,rotation_);
        }
        
        public static GameObject GetInstance(this GameObject prefab_, Vector3 position_, Quaternion rotation_, Transform parent_)
        {
            return PoolManager.GetInstance(prefab_,position_,rotation_,parent_);
        }
        
        public static T GetInstance <T>(this GameObject prefab_) where T : Component
        {
            return PoolManager.GetInstance<T>(prefab_);
        }
        
        public static T GetInstance <T>(this GameObject prefab_, Transform parent_) where T : Component
        {
            return PoolManager.GetInstance<T>(prefab_,parent_);
        }

        public static T GetInstance<T>(this GameObject prefab_, Vector3 position_, Quaternion rotation_) where T : Component
        {
            return PoolManager.GetInstance<T>(prefab_,position_,rotation_);
        }

        public static T GetInstance<T>(this GameObject prefab_, Vector3 position_, Quaternion rotation_, Transform parent_) where T : Component
        {
            return PoolManager.GetInstance<T>(prefab_,position_,rotation_,parent_);
        }

        public static void ReturnInstance(this GameObject obj_)
        {
            PoolManager.Instance.Return(obj_);
        }
    }
}