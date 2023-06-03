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
        
        [field: SerializeField] public int minLevel { get; private set; }
        
        [field: SerializeField] public WeightedDictionary<EnemyData> alliesDictionary { get; private set; }

        [ContextMenu("Sort Spells")]
        public void SortSpells() => combatTendency.SortSpells();
        
        private void OnValidate()
        {
            combatTendency?.SortSpells();
        }
    }
}
