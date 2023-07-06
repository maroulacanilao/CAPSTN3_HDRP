using AYellowpaper.SerializedCollections;
using Farming;
using NaughtyAttributes;
using UnityEngine;

namespace ScriptableObjectData
{
    [CreateAssetMenu(menuName = "ScriptableObjects/AssetDataBase", fileName = "AssetDataBase")]
    public class AssetDataBase : ScriptableObject
    {
        [field: BoxGroup("Stats Icon")]
        [field: SerializeField] public Sprite vitalityIcon { get; private set; }
        [field: BoxGroup("Stats Icon")]
        [field: SerializeField] public Sprite phyDmgIcon { get; private set; }
        [field: BoxGroup("Stats Icon")]
        [field: SerializeField] public Sprite intelligenceIcon { get; private set; }
        [field: BoxGroup("Stats Icon")]
        [field: SerializeField] public Sprite defIcon { get; private set; }
        [field: BoxGroup("Stats Icon")]
        [field: SerializeField] public Sprite spdIcon { get; private set; }

        [field: BoxGroup("Item Icon")]
        [field: SerializeField] public Sprite goldIcon { get; private set; }
        [field: BoxGroup("Item Icon")]
        [field: SerializeField] public Sprite weaponIcon { get; private set; }
        [field: BoxGroup("Item Icon")]
        [field: SerializeField] public Sprite armorIcon { get; private set; }
        [field: BoxGroup("Item Icon")]
        [field: SerializeField] public Sprite consumableIcon { get; private set; }
        [field: BoxGroup("Item Icon")]
        [field: SerializeField] public Sprite seedIcon { get; private set; }
        [field: BoxGroup("Item Icon")]
        [field: SerializeField] public Sprite toolIcon { get; private set; }
        
        [field: BoxGroup("Pool Objects")] [field: SerializeField]
        [field: SerializedDictionary("Prefab", "Pool Size")]
        public SerializedDictionary<GameObject,int> poolObjects { get; private set; }
        
        [field: BoxGroup("Prefabs")] [field: SerializeField]
        [field: SerializedDictionary("key", "Prefab")]
        public SerializedDictionary<string,GameObject> prefabs { get; private set; }
    }
}
