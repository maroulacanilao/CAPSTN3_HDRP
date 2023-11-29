using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using CustomHelpers;
using Items.ItemData;
using NaughtyAttributes;
using UnityEngine;

namespace ScriptableObjectData
{
    [CreateAssetMenu(menuName = "ScriptableObjects/FishDataBase", fileName = "New FishDataBase")]
    public class FishDataBase : ScriptableObject
    {
        [field: SerializeField] public SerializedDictionary<string, ConsumableData> consumableDictionary { get; private set; }
        [field: SerializeField] public SerializedDictionary<ConsumableData, int> fishCatchStats { get; private set; } = new SerializedDictionary<ConsumableData, int>();

        public void AddCatch(ConsumableData consumableData_, int count_ = 1)
        {
            if (fishCatchStats.TryGetValue(consumableData_, out var _harvests))
            {
                fishCatchStats[consumableData_] = _harvests + count_;
            }
            else
            {
                fishCatchStats.Add(consumableData_, count_);
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
