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

        [field: SerializeField] [field: CurveRange(0,0,2.4f,1,EColor.Green)]
        public AnimationCurve enemySpawnRate { get; private set; }

        [field: SerializeField] public SerializedDictionary<EnemyData, int> enemyKillsStats { get; private set; }
        
        public List<EnemyData> GetEligibleEnemies(int level_)
        {
            return (from _enemyData in enemyDataDictionary where _enemyData.Value.levelRange.x <= level_ select _enemyData.Value).ToList();
        }
        
        public EnemyData GetRandomEnemy(int level_)
        {
            var _eligibleEnemies = GetEligibleEnemies(level_);
            return _eligibleEnemies.GetRandomItem();
        }
        
        public void AddKills(EnemyData enemyData_, int count_ = 1)
        {
            if (enemyKillsStats.TryGetValue(enemyData_, out var _kills))
            {
                enemyKillsStats[enemyData_] = _kills + count_;
            }
            else
            {
                enemyKillsStats.Add(enemyData_, count_);
            }
        }
        
        #if UNITY_EDITOR
        
        [Button("Get All EnemyData")]
        private void GetAllEnemyData()
        {
            var _enemiesData = Resources.LoadAll<EnemyData>("Data/EnemyData");
            foreach (var _data in _enemiesData)
            {
                enemyDataDictionary.Add(_data.characterName, _data);
            }
            UnityEditor.EditorUtility.SetDirty(this);
        }
        
        #endif
    }
}