using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using CustomHelpers;
using Items.ItemData;
using NaughtyAttributes;
using UnityEngine;

namespace ScriptableObjectData
{
    [CreateAssetMenu(menuName = "ScriptableObjects/CropDataBase", fileName = "New CropDataBase")]
    public class CropDataBase : ScriptableObject
    {
        [field: SerializeField] public SerializedDictionary<string, ConsumableData> consumableDictionary { get; private set; }
        [field: SerializeField] public SerializedDictionary<ConsumableData, int> cropHarvestStats { get; private set; } = new SerializedDictionary<ConsumableData, int>();

        public void AddHarvest(ConsumableData consumableData_, int count_ = 1)
        {
            if (cropHarvestStats.TryGetValue(consumableData_, out var _harvests))
            {
                cropHarvestStats[consumableData_] = _harvests + count_;
            }
            else
            {
                cropHarvestStats.Add(consumableData_, count_);
            }
        }
        
#if true
        
        [Button("Set Consumable Dictionary")]
        private void SetConsumableDictionary()
        {
            consumableDictionary = new SerializedDictionary<string, ConsumableData>();
            
            foreach (var consumableData in Resources.LoadAll<ConsumableData>("Data/ItemData"))
            {
                consumableDictionary.Add(consumableData.ItemName, consumableData);
            }
        }
        
#endif
    }
}
