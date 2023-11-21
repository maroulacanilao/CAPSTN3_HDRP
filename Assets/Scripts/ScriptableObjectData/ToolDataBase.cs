using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using CustomHelpers;
using NaughtyAttributes;
using ScriptableObjectData.CharacterData;
using UnityEngine;
using Items.ItemData.Tools;

namespace ScriptableObjectData
{
    [CreateAssetMenu(fileName = "ToolDataBase", menuName = "ScriptableObjects/ToolDataBase", order = 99)]
    public class ToolDataBase : ScriptableObject
    {
        [field: BoxGroup("Watering Can")] [field: SerializeField]
        [field: SerializedDictionary("Level", "WateringCanData")]
        public SerializedDictionary<int, WateringCanData> WateringCanDictionary { get; private set; }
    }
}
