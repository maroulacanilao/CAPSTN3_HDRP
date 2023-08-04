using System;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using BaseCore;
using Character;
using CustomHelpers;
using Items;
using Items.ItemData;
using Managers;
using NaughtyAttributes;
using ScriptableObjectData.CharacterData;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Dungeon
{
    public class DungeonMap : MonoBehaviour
    {
        [SerializeField] private Transform playerSpawnPoint;
        [SerializeField] private QuestItemData questData;
        [SerializeField] private string postCombatMessage;
        [SerializeField] private EnemyStation[] stations;
        public Vector3 PlayerSpawnPoint => playerSpawnPoint.position;

        private readonly List<EnemyCharacter> enemiesSpawned = new List<EnemyCharacter>();

        protected virtual void Awake()
        {
            DungeonManager.OnEnemyDeath.AddListener(RemoveEnemy);
        }

        protected virtual void OnDestroy()
        {
            DungeonManager.OnEnemyDeath.RemoveListener(RemoveEnemy);
            LootSpawner.OnLootSpawned.RemoveListener(OnLootSpawned);
        }
        
        public virtual void InitializeLevel(DungeonMapData dungeonMapData_)
        {
            RemoveAllEnemy();
            
            Debug.Log(dungeonMapData_.name);
            
            var _stationList = new List<EnemyStation>(stations);
            var _count = dungeonMapData_.enemyCountRange.GetRandomInRange();
            _count = Mathf.Clamp(_count, 0, _stationList.Count);

            for (int i = 0; i < _count; i++)
            {
                if(i >= _stationList.Count) break;
                
                var _index = Random.Range(0, _stationList.Count);
                var _station = _stationList[_index];
                if(_station == null) continue;
                
                _stationList.RemoveAt(_index);
                
                var _enemyData = dungeonMapData_.possibleEnemies.GetWeightedRandom();
                var _level = dungeonMapData_.enemyLevelRange.GetRandomInRange();
                var _enemyInstance = _station.SpawnEnemy(_enemyData, _level);
                
                enemiesSpawned.Add(_enemyInstance);
                
                DungeonManager.OnEnemySpawn.Invoke(_enemyInstance, _station);
            }

            gameObject.SetActive(true);
            
            LootSpawner.OnLootSpawned.AddListener(OnLootSpawned);
        }
        
        public virtual void DeInitializeLevel()
        {
            RemoveAllEnemy();
            gameObject.SetActive(false);
            LootSpawner.OnLootSpawned.RemoveListener(OnLootSpawned);
        }

        protected virtual void RemoveEnemy(EnemyCharacter enemyCharacter_)
        {
            if (!enemiesSpawned.Contains(enemyCharacter_)) return;

            PurgeNulls();

            if (!enemiesSpawned.Remove(enemyCharacter_)) return;

            Destroy(enemyCharacter_.gameObject);

            Managers.GameManager.DelaySendToFungus(postCombatMessage, -1f, 1);
        }

        private void RemoveAllEnemy()
        {
            if(enemiesSpawned.Count == 0) return;
            
            PurgeNulls();
            
            foreach (var _enemy in enemiesSpawned)
            {
                Destroy(_enemy.gameObject);
            }
            
            enemiesSpawned.Clear();
        }
        
        private void PurgeNulls()
        {
            enemiesSpawned.RemoveAll(_enemy => _enemy.IsEmptyOrDestroyed());
        }
        
        private void OnLootSpawned(LootDropObject lootDropObject_)
        {
            Debug.Log("OnLootSpawned");
            if(enemiesSpawned.Count > 0) return;
            Debug.Log("OnLootSpawned 2");
            if(lootDropObject_ == null) return;
            Debug.Log("LootDropObject is not null");
            if (questData == null) throw new Exception("Quest Data is null");
            Debug.Log("QuestData is not null");

            var _questItem = questData.GetItemQuest();
            
            if(_questItem == null) return;
            Debug.Log("QuestItem is not null");

            lootDropObject_.lootDrop.itemsDrop.Add(_questItem);
        }
        
        [Button("Get All Stations")]
        private void GetAllStations()
        {
            stations = GetComponentsInChildren<EnemyStation>();
        }
    }
}
