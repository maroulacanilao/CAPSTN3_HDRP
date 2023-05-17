using BaseCore;
using NaughtyAttributes;
using UnityEngine;

namespace ScriptableObjectData.CharacterData
{
    public abstract class CharacterData : ScriptableObject
    {
        [field: SerializeField] public string characterName { get; private set; }
        
        [field: SerializeField] [field: ShowAssetPreview()] public GameObject farmPrefab { get; protected set; }
        
        [field: SerializeField] [field: ShowAssetPreview()] public GameObject battlePrefab { get; protected set; }
        [field: SerializeField] [field: BoxGroup("Stats")] public StatsGrowth statsData { get; protected set; }

        public int characterID { get; private set; }

        private void OnValidate()
        {
            characterID = characterName.GetHashCode();
        }

    }
}
