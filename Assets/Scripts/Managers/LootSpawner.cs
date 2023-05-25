using BaseCore;
using CustomEvent;
using Items;
using UnityEngine;
using ObjectPool;
using ScriptableObjectData;
using ScriptableObjectData.CharacterData;

namespace Managers
{
    public class LootSpawner : MonoBehaviour
    {
        [SerializeField] private LootDropObject lootPrefab;
        [SerializeField] private ItemDatabase itemDatabase;
        
        [Header("For Debugging")]
        [SerializeField] private EnemyData enemyData;
        [SerializeField] private Vector3 lootPosition;

        public static readonly Evt<LootTable, Vector3> OnSpawnLoot = new Evt<LootTable, Vector3>();

        private LootDropObject lootObj;
        private void Awake()
        {
            OnSpawnLoot.AddListener(SpawnLoot);
        }
        
        private void OnDestroy()
        {
            OnSpawnLoot.RemoveListener(SpawnLoot);
        }

        private void SpawnLoot(LootTable lootTable_, Vector3 lootPosition_)
        {
            lootObj = lootPrefab.gameObject
                .GetInstance<LootDropObject>(lootPosition_, Quaternion.identity, transform)
                .Initialize(lootTable_.GetDrop(itemDatabase));
        }

        [ContextMenu("TestSpawn")]
        private void TestSpawn()
        {
            SpawnLoot(enemyData.LootTable, lootPosition);
        }
        
        [ContextMenu("DeSpawn Test")]
        private void TestDeSpawn()
        {
            lootObj.gameObject.ReturnInstance();
        }
    }
}
