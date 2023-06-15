using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using CustomHelpers;
using NaughtyAttributes;
using ScriptableObjectData.CharacterData;
using UnityEngine;

namespace ScriptableObjectData
{
    [CreateAssetMenu(fileName = "EnemyDataBase", menuName = "ScriptableObjects/EnemyDataBase", order = 99)]
    public class EnemyDataBase : ScriptableObject
    {
        [field: SerializedDictionary("level", "enemyData")]
        [field: SerializeField] public SerializedDictionary<string, EnemyData> enemyDataDictionary { get; private set; }

        [field: SerializeField] public AnimationCurve enemySpawnRate { get; private set; }

        public SerializedDictionary<EnemyData, int> enemyKillsStats;
        
        public List<EnemyData> GetEligibleEnemies(int level_)
        {
            return (from _enemyData in enemyDataDictionary where _enemyData.Value.minLevel <= level_ select _enemyData.Value).ToList();
        }
        
        public EnemyData GetRandomEnemy(int level_)
        {
            var _eligibleEnemies = GetEligibleEnemies(level_);
            return _eligibleEnemies.GetRandomItem();
        }
        
        public void AddKills(EnemyData enemyData_)
        {
            if (enemyKillsStats.TryGetValue(enemyData_, out var _kills))
            {
                enemyKillsStats[enemyData_] = _kills + 1;
            }
            else
            {
                enemyKillsStats.Add(enemyData_, 1);
            }
        }
        
        #if UNITY_EDITOR
        
        [Button("Get All EnemyData")]
        private void GetAllEnemyData()
        {
            var _enemiesData = Resources.LoadAll<EnemyData>("Data/EnemyData");
            foreach (var _data in _enemiesData)
            {
                enemyDataDictionary.Add(_data.name, _data);
            }
            UnityEditor.EditorUtility.SetDirty(this);
        }
        
        #endif
    }
}