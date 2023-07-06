using System.Collections.Generic;
using BaseCore;
using CustomEvent;
using CustomHelpers;
using Items;
using Managers.SceneLoader;
using UnityEngine;
using ObjectPool;
using ScriptableObjectData;
using ScriptableObjectData.CharacterData;

namespace Managers
{
    public struct LootDropData
    {
        public LootTable lootTable;
        public Vector3 position;
        public int level;
    }
    
    public class LootSpawner : MonoBehaviour
    {
        [SerializeField] private LootDropObject lootPrefab;
        [SerializeField] private ItemDatabase itemDatabase;
        
        [Header("For Debugging")]
        [SerializeField] private EnemyData enemyData;
        [SerializeField] private Vector3 lootPosition;

        public static readonly Evt<LootDropData> OnSpawnLoot = new Evt<LootDropData>();
        public static readonly Evt RemoveAllLoots = new Evt();

        private Transform mParent;
        private Transform parent
        {
            get
            {
                var _activeScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
                if (mParent.IsValid() && mParent.gameObject.scene == _activeScene) return mParent;
                mParent = SceneEnabler.FindSceneEnabler(_activeScene.name).InstanceParent;
                return mParent;
            }
        }
        
        private HashSet<LootDropObject> lootDropObjects = new HashSet<LootDropObject>();
        
        private void Awake()
        {
            OnSpawnLoot.AddListener(SpawnLoot);
            RemoveAllLoots.AddListener(RemoveAll);
        }
        
        private void OnApplicationQuit()
        {
            OnSpawnLoot.RemoveListener(SpawnLoot);
            RemoveAllLoots.RemoveListener(RemoveAll);
        }

        private void SpawnLoot(LootDropData lootDropData_)
        {
            var _lootObj = lootPrefab.gameObject
                .GetInstance<LootDropObject>(lootDropData_.position, Quaternion.identity, parent)
                .Initialize(lootDropData_.lootTable.GetDrop(itemDatabase,lootDropData_.level));
            
            lootDropObjects.Add(_lootObj);
        }
        
        private void RemoveAll()
        {
            foreach (var _lootObj in lootDropObjects)
            {
                if(_lootObj.IsEmptyOrDestroyed()) continue;
                _lootObj.gameObject.ReturnInstance();
            }
            
            lootDropObjects.Clear();
        }
    }
}
