using AYellowpaper.SerializedCollections;
using Farming;
using Items;
using NaughtyAttributes;
using UnityEngine;

namespace ScriptableObjectData
{
    [CreateAssetMenu(menuName = "ScriptableObjects/AssetDataBase", fileName = "AssetDataBase")]
    public class AssetDataBase : ScriptableObject
    {
        [field: BoxGroup("Item Icon")] [field: SerializeField]
        [field: SerializedDictionary("Item Type", "Sprite")]
        public SerializedDictionary<ItemType, Sprite> itemTypeIcons { get; private set; }

        [field: BoxGroup("FarmTile Assets")] [field: SerializeField]
        [field: SerializedDictionary("Tile State", "Mesh")]
        public SerializedDictionary<TileState,Mesh> tileStateMesh { get; private set; }
        
        [field: BoxGroup("FarmTile Assets")] [field: SerializeField]
        public SerializedDictionary<bool,Material> tileMaterial { get; private set; }
        
        [field: BoxGroup("Pool Objects")] [field: SerializeField]
        [field: SerializedDictionary("Prefab", "Pool Size")]
        public SerializedDictionary<GameObject,int> poolObjects { get; private set; }
        
        [field: BoxGroup("Prefabs")] [field: SerializeField]
        [field: SerializedDictionary("key", "Prefab")]
        public SerializedDictionary<string,GameObject> prefabs { get; private set; }
        
        [field: BoxGroup("Sprites")] [field: SerializeField]
        public Sprite selectedButtonSprite { get; private set; }
        [field: BoxGroup("Sprites")] [field: SerializeField]
        public Sprite deselectedButtonSprite { get; private set; }
    }
}
