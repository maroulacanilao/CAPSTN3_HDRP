using System;
using System.Collections.Generic;
using System.Linq;
using BaseCore;
using CustomHelpers;
using EnemyController;
using Farming;
using ScriptableObjectData;
using UnityEngine;

namespace Managers
{
    public class EnemySpawner : Singleton<EnemySpawner>
    {
        [SerializeField] private Transform[] enemySpawnPoints;
        [SerializeField] private GameDataBase gameDataBase;
        [SerializeField] private int tickRate = 3;
        [SerializeField] private int maxEnemyCount = 3;
        
        private EnemyDataBase enemyDataBase => gameDataBase.enemyDataBase;
        private int playerLevel => gameDataBase.playerData.LevelData.CurrentLevel;

        private List<EnemyAIController> enemyControllers = new List<EnemyAIController>();
        private Dictionary<FarmTile, EnemyAIController> targetTileDictionary;

        protected override void Awake()
        {
            base.Awake();
            enemyControllers = new List<EnemyAIController>();
            targetTileDictionary = new Dictionary<FarmTile, EnemyAIController>();
            
            TimeManager.OnMinuteTick.AddListener(Tick);
            FarmTileManager.OnAddFarmTile.AddListener(OnAddTile);
            FarmTileManager.OnRemoveTile.AddListener(OnRemoveTile);
        }

        private void OnDestroy()
        {
            TimeManager.OnMinuteTick.RemoveListener(Tick);
        }

        private void OnAddTile(FarmTile tile_)
        {
            if(targetTileDictionary.ContainsKey(tile_)) return;
            
            targetTileDictionary.Add(tile_, null);
        }

        private void OnRemoveTile(FarmTile tile_)
        {
            if(!targetTileDictionary.TryGetValue(tile_, out var _enemy)) return;
            
            // Get another Target For Enemy
            if (_enemy.IsValid())
            {
                
            }
            targetTileDictionary.Remove(tile_);
        }

        private void Tick()
        {
            if(TimeManager.EndingHour - 1 <= TimeManager.CurrentHour) return;
            if(TimeManager.CurrentMinute % tickRate != 0) return;
            if(enemyControllers.Count >= maxEnemyCount) return;
            
            

            var _spawnRate = enemyDataBase.enemySpawnRate.Evaluate(TimeManager.ScaledGameTime);
            if(!RandomHelper.RandomBool(_spawnRate)) return;
            
            SpawnEnemy();
        }
        
        private void SpawnEnemy()
        {
            var _target = GetViableTile();
            if(_target == null) return;
            
            var _spawnPoint = enemySpawnPoints.GetRandomItem();
            var _enemy = enemyDataBase.GetRandomEnemy(playerLevel);
            Debug.LogWarning("Spawning Enemy! " + _enemy.characterName);
            var _enemyInstance = Instantiate(_enemy.farmPrefab, _spawnPoint.position, Quaternion.identity).GetComponent<EnemyAIController>();
            
            enemyControllers.Add(_enemyInstance);
            _enemyInstance.SetTileTarget(_target);
        }

        private FarmTile GetViableTile()
        {
            return targetTileDictionary.FirstOrDefault(td_ => 
                td_.Key.tileState != TileState.Empty && td_.Value.IsEmptyOrDestroyed())
                .Key;   
        }
        
        public void RemoveEnemy(GameObject enemy_)
        {
            if(!enemy_.TryGetComponent(out EnemyAIController _aiController)) return;
            
            enemyControllers.Remove(_aiController);
            
            if (_aiController.IsEmptyOrDestroyed()) return;
            
            Destroy(enemy_);
        }
        
        public FarmTile GetNewTileTarget(EnemyAIController enemy_, FarmTile previousTile_)
        {
            if(targetTileDictionary.Count == 0) return null;
            
            if(targetTileDictionary.TryGetValue(previousTile_, out var _enemy))
            {
                if (enemy_ == _enemy) targetTileDictionary[previousTile_] = null;
            }

            var _tile = GetViableTile();
            
            if(_tile == null) return null;
            
            targetTileDictionary[_tile] = enemy_;
            return _tile;
        }
    }
}
