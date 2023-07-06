using System.Collections.Generic;
using BaseCore;
using NaughtyAttributes;
using ScriptableObjectData.CharacterData;
using UnityEngine;

namespace Dungeon
{
    [CreateAssetMenu(fileName = "DungeonMapData", menuName = "ScriptableObjects/DungeonMapData", order = 0)]
    public class DungeonMapData : ScriptableObject
    {
        [field: SerializeField] 
        public int dungeonLevel { get; private set; }
        
        [field: SerializeField]
        public int minPlayerLevel { get; private set; }

        [field: SerializeField] [field: MinMaxSlider(1,10)] 
        public Vector2Int enemyLevelRange { get; private set; }
        
        [field: SerializeField] [field: MinMaxSlider(1,10)] 
        public Vector2Int enemyCountRange { get; private set; }
        
        [field: SerializeField] 
        public WeightedDictionary<EnemyData> possibleEnemies { get; private set; }
        
        [field: SerializeField] [field: BoxGroup("Map")]
        public bool willUseRandomMap { get; private set; }

        [field: SerializeField] [field: BoxGroup("Map")] [field: ShowIf("willUseRandomMap")]
        public List<int> possibleMapRange;
        
        [field: SerializeField] [field: BoxGroup("Map")] [field: HideIf("willUseRandomMap")]
        public int mapID { get; private set; }
    }
}