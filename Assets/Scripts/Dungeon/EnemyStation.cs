using System;
using Character;
using CustomHelpers;
using EnemyController;
using ScriptableObjectData.CharacterData;
using UnityEngine;

namespace Dungeon
{
    [DefaultExecutionOrder(5)]
    public class EnemyStation : MonoBehaviour
    {
        [SerializeField] private Transform[] patrolPoints;
        [SerializeField] private Transform spawnPoint;
        
        private EnemyAIController enemyController;

        public EnemyAIController EnemyController => enemyController.IsEmptyOrDestroyed() ? null : enemyController;

        public Transform[] PatrolPoints => patrolPoints;
        
        public EnemyCharacter SpawnEnemy(EnemyData enemyData_, int level_)
        {
            var _prefab = enemyData_.farmPrefab;
            
            var _enemy = Instantiate(_prefab, spawnPoint.position, Quaternion.identity);
            enemyController = _enemy.GetComponent<EnemyAIController>();
            _enemy.SetLevel(level_);
            enemyController.Initialize(this);
            
            enemyController.transform.SetParent(transform);

            return _enemy as EnemyCharacter;
        }
    }
}
