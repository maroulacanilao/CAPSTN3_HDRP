using System;
using System.Collections.Generic;
using BaseCore;
using BattleSystem;
using NaughtyAttributes;
using ObjectPool;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ScriptableObjectData.CharacterData
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/CharacterData/EnemyData", order = 0)]
    public class EnemyData : CharacterData
    {
        [field: SerializeField] [field: BoxGroup("LootTable")]  public LootTable LootTable { get; private set; }

        [field: SerializeField] public EnemyCombatTendency combatTendency { get; private set; }
        
        [field: SerializeField] [field: MinMaxSlider(1,10)]
        public Vector2Int levelRange { get; private set; }
        
        [field: SerializeField] public WeightedDictionary<EnemyData> alliesDictionary { get; private set; }

        [field: BoxGroup("Description")]
        [field: SerializeField] public EncyclopediaInfo encyclopediaInfo { get; private set; }

        public int quantityNeededCount = 5;
    }
}
