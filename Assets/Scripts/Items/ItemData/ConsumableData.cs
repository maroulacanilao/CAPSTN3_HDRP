using System;
using NaughtyAttributes;
using UnityEngine;

namespace Items.ItemData
{
    [CreateAssetMenu(menuName = "ScriptableObjects/ItemData/ConsumableData", fileName = "ConsumableData")]
    public class ConsumableData : ItemData
    {
        [field: NaughtyAttributes.InfoBox("Effect on player/ally when consumed")]
        [field: SerializeField] public StatusEffectBase ConsumeEffect { get; private set; }
        
        [field: NaughtyAttributes.InfoBox("Effect on Enemy when used")]
        [field: SerializeField] public StatusEffectBase UseEffect { get; private set; }

        [field: SerializeField] [field: MinValue(1)] [field: ShowIf("IsStackable")]
        public int maxPossibleDropCount { get; private set; } = 1;
        
        [field: BoxGroup("Description")]
        [field: SerializeField] public EncyclopediaInfo encyclopediaInfo { get; private set; }

        private void Reset()
        {
            ItemType = ItemType.Consumable;
            IsStackable = true;
        }

        protected override void OnValidate()
        {
            ItemType = ItemType.Consumable;
            IsStackable = true;
        }
    }
}