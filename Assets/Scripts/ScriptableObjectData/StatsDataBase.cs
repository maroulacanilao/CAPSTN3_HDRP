using System;
using System.Collections.Generic;
using BattleSystem;
using Character;
using Items.ItemData;
using UI.StatShop;
using UnityEngine;
using AYellowpaper.SerializedCollections;

namespace ScriptableObjectData
{
    public enum StatType { Health, Strength, Intelligence, Defense, Speed }
    
    [CreateAssetMenu(menuName = "ScriptableObjects/StatsDataBase", fileName = "StatsDataBase")]
    public class StatsDataBase : ScriptableObject
    {
        [field: SerializeField] public SerializedDictionary<StatType, Sprite> statSprites { get; private set; }

        [field: SerializeField] public SerializedDictionary<StatType, List<ConsumableData>> statConsumable { get; private set; }
        
        [field: SerializeField] public float intelligenceToManaRatio { get; private set; } = 1.5f;

        private void OnEnable()
        {
            StatsHelper.intelToManaMultiplier = intelligenceToManaRatio;
        }


        public StatType GetStatTypeByConsumable(ConsumableData consumableData_)
        {
            foreach (var stat in statConsumable)
            {
                if (stat.Value.Contains(consumableData_))
                {
                    return stat.Key;
                }
            }

            return StatType.Health;
        }
    }
}
