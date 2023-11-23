using System;
using System.Collections.Generic;
using System.Linq;
using BaseCore;
using Character;
using CustomEvent;
using CustomHelpers;
using Items;
using Items.Inventory;
using Items.ItemData;
using Managers;
using NaughtyAttributes;
using Player;
using ScriptableObjectData;
using ScriptableObjectData.CharacterData;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Dungeon
{
    public class DungeonManager : Singleton<DungeonManager>
    {
        [SerializeField] private ProgressionData progressionData;
        [SerializeField] private PlayerData playerData;
        [SerializeField] private EventQueueData eventQueueData;
        [SerializeField] private DungeonMapData[] dungeonMapData;
        [SerializeField] private DungeonMap[] dungeonMaps;
        [SerializeField] private SessionData sessionData;

        public DungeonMap currDungeon { get; private set; }
        private PlayerInputController playerController;
        public int PlayerLevel => playerData.LevelData.CurrentLevel;
        private PlayerInventory inventory => playerData.inventory;
        
        public static readonly Evt<EnemyCharacter, EnemyStation> OnEnemySpawn = new Evt<EnemyCharacter, EnemyStation>();
        public static readonly Evt<EnemyCharacter> OnEnemyDeath = new Evt<EnemyCharacter>();

        public int currentLevel { get; private set; } = 0;

        protected override void Awake()
        {
            base.Awake();

            playerController = PlayerInputController.Instance;
        }

        private void Start()
        {
            EnterNewDungeon(sessionData.dungeonLevel);
        }

        private void OnDestroy()
        {
            LootSpawner.RemoveAllLoots.Invoke();
            RemoveQuestItems();
        }

        private void OnEnable()
        {
            eventQueueData.ExecuteEvents(gameObject.scene.name);
        }
        
        private void OnDisable()
        {
            eventQueueData.ClearQueue(gameObject.scene.name);
        }

        public void GoToNextLevel()
        {
            currentLevel++;
            if(currentLevel > progressionData.highestDungeonLevel) progressionData.highestDungeonLevel = currentLevel;
            EnterNewDungeon(currentLevel);
        }
        
        public void GoToPrevDungeon()
        {
            EnterNewDungeon(currentLevel - 1);
        }
        
        public void EnterNewDungeon(int level_)
        {
            currentLevel = level_;
            var _level = Mathf.Clamp(level_, 1, dungeonMapData.Length);
            
            var _data = dungeonMapData.FirstOrDefault(d => d.dungeonLevel == _level);
            
            DeInitializeAllDungeon();
            
            if(_data == null)
            {
                _data = dungeonMapData[0];
            }
            else if (_data.willUseRandomMap)
            {
                var _index = _data.possibleMapRange.GetRandomItem();
                currDungeon = dungeonMaps[_index];
            }
            else
            {
                currDungeon = dungeonMaps[_data.mapID];
            }
            currDungeon.InitializeLevel(_data);
            Debug.Log(currDungeon.PlayerSpawnPoint);
            playerController.transform.position = currDungeon.PlayerSpawnPoint;
        }
        
        private void DeInitializeAllDungeon()
        {
            foreach (var _dungeon in dungeonMaps)
            {
                _dungeon.DeInitializeLevel();
            }
            RemoveQuestItems();
        }

        private void RemoveQuestItems()
        {
            var _questItems = inventory.itemsLookup.Where(i => i.Key is QuestItemData).ToList();
            
            if(_questItems.Count <= 0) return;
            
            var _list = new List<Item>();
            
            foreach (var _keyValuePair in _questItems)
            {
                if(_keyValuePair.Value == null) continue;
                if(_keyValuePair.Value.Count <= 0) continue;
                _list.AddRange(_keyValuePair.Value);
            }
            
            foreach (var _item in _list)
            {
                inventory.RemoveItem(_item);
            }
        }

        [Button("Get All Dungeon Maps")]
        public void GetAllDungeonMaps()
        {
            dungeonMaps = FindObjectsOfType<DungeonMap>();
        }
        
        [Button("Get All Dungeon Map Data")]
        public void GetAllDungeonMapData()
        {
            dungeonMapData = Resources.LoadAll<DungeonMapData>("Data/DungeonMaps");
        }

        public void PlayerDied()
        {
            // GameManager.OnExitBattle.Invoke(BattleResultType.Lose);
        }
    }
}
