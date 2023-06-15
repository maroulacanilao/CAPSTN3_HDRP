using System.Collections.Generic;
using BattleSystem;
using Items.Inventory;
using NaughtyAttributes;
using ScriptableObjectData.CharacterData;
using Trading;
using UnityEngine;

namespace ScriptableObjectData
{
    // [CreateAssetMenu(menuName = "ScriptableObjects/GameDataBase", fileName = "GameDataBase")]
    public class GameDataBase : ScriptableObject
    {
        [field: SerializeField] public ItemDatabase itemDatabase { get; private set; }
        [field: SerializeField] public AssetDataBase assetDataBase { get; private set; }
        [field: SerializeField] public EnemyDataBase enemyDataBase { get; private set; }
        
        [field: SerializeField] public BattleData battleData { get; private set; }

        [field: Header("PlayerFields")]
        [field: SerializeField] public PlayerData playerData { get; private set; }
        [field: SerializeField] public PlayerInventory playerInventory { get; private set; }
        [field: SerializeField] public SeedDataBase seedDataBase { get; private set; }

        private Dictionary<int, EnemyData> enemyDataDictionary = new Dictionary<int, EnemyData>();

        private bool hasInitialized;
        public void Initialize()
        {
            if(hasInitialized) return;
            enemyDataDictionary = new Dictionary<int, EnemyData>();
            playerData.Initialize(this);
            //seedDataBase.Initialize(playerData.playerLevelData.CurrentLevel);
            hasInitialized = true;
        }
    
        public void DeInitialize()
        {
            playerData.DeInitialize();
            hasInitialized = false;
        }
    }
}
