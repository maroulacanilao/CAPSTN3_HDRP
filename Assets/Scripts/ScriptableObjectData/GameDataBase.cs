using System.Collections.Generic;
using BattleSystem;
using Farming;
using Items.Inventory;
using NaughtyAttributes;
using ScriptableObjectData.CharacterData;
using Settings;
using Shop;
using Trading;
using UnityEngine;

namespace ScriptableObjectData
{
    // [CreateAssetMenu(menuName = "ScriptableObjects/GameDataBase", fileName = "GameDataBase")]
    public class GameDataBase : ScriptableObject
    {
        [field: Header("Data Base")]
        [field: SerializeField] public ItemDatabase itemDatabase { get; private set; }
        [field: SerializeField] public AssetDataBase assetDataBase { get; private set; }
        [field: SerializeField] public AudioDataBase audioDataBase { get; private set; }
        [field: SerializeField] public EnemyDataBase enemyDataBase { get; private set; }
        [field: SerializeField] public CropDataBase cropDataBase { get; private set; }
        [field: SerializeField] public SpellDataBase spellDataBase { get; private set; }
        [field: SerializeField] public StatsDataBase statsDataBase { get; private set; }
        [field: SerializeField] public ToolDataBase toolDataBase { get; private set; }
        [field: SerializeField] public AllyDataBase allyDataBase { get; private set; }


        [field: Header("Game Data")]
        [field: SerializeField] public BattleData battleData { get; private set; }
        [field: SerializeField] public ShippingData shippingData { get; private set; }
        [field: SerializeField] public StatShopData statShopData { get; private set; }
        [field: SerializeField] public ShrineData shrineData { get; private set; }
        [field: SerializeField] public ProgressionData progressionData { get; private set; }
        [field: SerializeField] public SessionData sessionData { get; private set; }
        [field: SerializeField] public SettingsData settingsData { get; private set; }
        [field: SerializeField] public EventQueueData eventQueueData { get; private set; }

        [field: Header("Player Data")]
        [field: SerializeField] public PlayerData playerData { get; private set; }
        [field: SerializeField] public PlayerInventory playerInventory { get; private set; }

        [field: Header("SceneName")]
        [field: SerializeField] [field: Scene] 
        public string MainMenuSceneName { get; private set; }
        
        [field: SerializeField] [field: Scene] 
        public string FarmSceneName { get; private set; }
        
        [field: SerializeField] [field: Scene] 
        public string TutorialScene { get; private set; }
        
        [field: SerializeField] [field: Scene] 
        public string BattleSceneName { get; private set; }
        
        [field: SerializeField] [field: Scene] 
        public string DungeonSceneName { get; private set; }
        
        [field: Header("Layers")] 
        [field: SerializeField] public LayerMask farmTileLayer { get; private set; }
        [field: SerializeField] public LayerMask plowableAreaLayer { get; private set; }
        [field: SerializeField] public LayerMask foliageLayer { get; private set; }
        
        private bool hasInitialized;
        
        public void Initialize()
        {
            if(hasInitialized) return;
            settingsData.Initialize();
            playerData.Initialize(this);
            allyDataBase.InitalizeMembers(this);
            sessionData.InitializeSession();
            statShopData.Initialize();
            shrineData.Initialize(this);
            progressionData.Initialize();
            eventQueueData.ClearAllQueues();
            hasInitialized = true;
        }
        
        public void ReInitialize()
        {
            if(!hasInitialized) return;
            settingsData.Initialize();
            sessionData.InitializeSession();
            statShopData.Initialize();
            shrineData.Initialize(this);
            progressionData.Initialize();
            eventQueueData.ClearAllQueues();
        }
    
        public void DeInitialize()
        {
            playerData.DeInitialize();
            sessionData.DeInitialize();
            shrineData.DeInitialize();
            progressionData.DeInitialize();
            hasInitialized = false;
        }
    }
}
