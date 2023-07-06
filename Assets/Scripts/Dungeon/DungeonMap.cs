using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using BaseCore;
using Character;
using CustomHelpers;
using NaughtyAttributes;
using ScriptableObjectData.CharacterData;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Dungeon
{
    public class DungeonMap : MonoBehaviour
    {
        [SerializeField] 
        private Transform playerSpawnPoint;

        [SerializeField]
        private GameObject mapExit;
        
        [SerializeField] 
        private EnemyStation[] stations;
        public Vector3 PlayerSpawnPoint => playerSpawnPoint.position;

        private readonly List<EnemyCharacter> enemiesSpawned = new List<EnemyCharacter>();

        private void Awake()
        {
            DungeonManager.OnEnemyDeath.AddListener(RemoveEnemy);
        }

        private void OnDestroy()
        {
            DungeonManager.OnEnemyDeath.RemoveListener(RemoveEnemy);
        }
        
        public void InitializeLevel(DungeonMapData dungeonMapData_)
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
                _stationList.RemoveAt(_index);
                
                var _enemyData = dungeonMapData_.possibleEnemies.GetWeightedRandom();
                var _level = dungeonMapData_.enemyLevelRange.GetRandomInRange();
                var _enemyInstance = _station.SpawnEnemy(_enemyData, _level);
                
                enemiesSpawned.Add(_enemyInstance);
                
                DungeonManager.OnEnemySpawn.Invoke(_enemyInstance, _station);
            }
            
            mapExit.SetActive(false);
            
            gameObject.SetActive(true);
        }
        
        public void DeInitializeLevel()
        {
            RemoveAllEnemy();
            gameObject.SetActive(false);
        }
        
        private void RemoveEnemy(EnemyCharacter enemyCharacter_)
        {
            if(!enemiesSpawned.Contains(enemyCharacter_)) return;
            
            PurgeNulls();
            
            if(!enemiesSpawned.Remove(enemyCharacter_)) return;
            
            Destroy(enemyCharacter_.gameObject);
            
            if(enemiesSpawned.Count > 0) return;
            
            OnLevelComplete();
        }
        
        private void OnLevelComplete()
        {
            //TODO: Add event for level complete
            
            Debug.Log("<color=red>Level Complete!</color>");
            mapExit.SetActive(true);
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
        
        [Button("Get All Stations")]
        private void GetAllStations()
        {
            stations = GetComponentsInChildren<EnemyStation>();
        }
    }
}
