using System;
using System.Linq;
using BaseCore;
using Character;
using CustomEvent;
using CustomHelpers;
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

        private DungeonMap currDungeon;
        private PlayerInputController playerController;
        public int PlayerLevel => playerData.LevelData.CurrentLevel;
        
        public static readonly Evt<EnemyCharacter, EnemyStation> OnEnemySpawn = new Evt<EnemyCharacter, EnemyStation>();
        public static readonly Evt<EnemyCharacter> OnEnemyDeath = new Evt<EnemyCharacter>();

        public int currentLevel { get; private set; } = 0;

        protected override void Awake()
        {
            base.Awake();

            playerController = PlayerInputController.Instance;
            EnterNewDungeon(sessionData.dungeonLevel);
        }

        private void OnDestroy()
        {
            LootSpawner.RemoveAllLoots.Invoke();
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
        
        public void EnterNewDungeon(int level_)
        {
            currentLevel = level_;
            var _level = Mathf.Clamp(level_, 1, dungeonMapData.Length - 1);
            
            var _data = dungeonMapData.FirstOrDefault(d => d.dungeonLevel == _level);
            
            if (_data.willUseRandomMap)
            {
                var _index = _data.possibleMapRange.GetRandomItem();
                currDungeon = dungeonMaps[_index];
            }
            else
            {
                currDungeon = dungeonMaps[_data.mapID];
            }
            currDungeon.InitializeLevel(_data);
            
            playerController.transform.position = currDungeon.PlayerSpawnPoint;
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
    }
}
