using System.Collections.Generic;
using BaseCore;
using BattleSystem;
using Character;
using NaughtyAttributes;
using Spells.Base;
using UnityEngine;

namespace ScriptableObjectData.CharacterData
{
    public abstract class CharacterData : ScriptableObject
    {
        [field: SerializeField] public string characterName { get; private set; }
        
        [field: SerializeField] [field: ShowAssetPreview()] public CharacterBase farmPrefab { get; protected set; }
        
        [field: SerializeField] [field: ShowAssetPreview()] public BattleCharacter battlePrefab { get; protected set; }
        [field: SerializeField] [field: BoxGroup("Stats")] public StatsGrowth statsData { get; protected set; }
        
        [field: SerializeField] public List<SpellData> spells { get; private set; }

        public int characterID { get; private set; }

        private void OnValidate()
        {
            characterID = characterName.GetHashCode();
        }
    }
}
