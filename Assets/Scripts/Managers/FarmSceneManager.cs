using System;
using BaseCore;
using Character;
using CustomHelpers;
using NaughtyAttributes;
using Player;
using ScriptableObjectData;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    [Serializable]
    public enum FarmLoadType { NewGame, DungeonEntrance, NewDay, LoadGame, House }
    
    public class FarmSceneManager : Singleton<FarmSceneManager>
    {
        [field: SerializeField] public EventQueueData eventQueueData { get; private set; }
        [SerializeField] private SessionData sessionData;

        #region SpawnPoints

        [BoxGroup("SpawnPoints")] [SerializeField]
        private Transform newGameSpawnPoint;
        [BoxGroup("SpawnPoints")] [SerializeField]
        private Transform dungeonEntranceSpawnPoint;
        [BoxGroup("SpawnPoints")] [SerializeField]
        private Transform newDaySpawnPoint;
        [BoxGroup("SpawnPoints")] [SerializeField]
        private Transform loadGameSpawnPoint;
        [BoxGroup("SpawnPoints")] [SerializeField]
        private Transform houseSpawnPoint;

        #endregion

        public PlayerInputController player { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            player = PlayerInputController.Instance;
            SceneManager.activeSceneChanged += OnSceneChanged;
        }

        private void OnDestroy()
        {
            SceneManager.activeSceneChanged -= OnSceneChanged;
        }

        private void OnSceneChanged(Scene current_, Scene next_)
        {
            if(next_.name != gameObject.scene.name) return;
            
            SetSceneOnType();
        }

        private void OnEnable()
        {
            eventQueueData.ExecuteEvents(gameObject.scene.name);
        }
    
        private void OnDisable()
        {
            eventQueueData.ClearQueue(gameObject.scene.name);
            
            if(InputManager.Instance.IsEmptyOrDestroyed()) return;
            InputManager.Instance.enabled = true;
        }

        private void SetSceneOnType()
        {
            var _sceneType = sessionData.farmLoadType;

            switch (_sceneType)
            {
                case FarmLoadType.NewGame:
                    OnNewGame();
                    break;
                
                case FarmLoadType.DungeonEntrance:
                    OnDungeonEntrance();
                    break;
                
                case FarmLoadType.NewDay:
                    OnNewDay();
                    break;
                
                case FarmLoadType.LoadGame:
                    OnLoadGame();
                    break;
                
                case FarmLoadType.House:
                default:
                    
                    OnHouse();
                    break;
            }
        }
        
        private void OnNewGame()
        {
            player.transform.position = houseSpawnPoint.position;
            RefillPlayer();
        }
        
        private void OnDungeonEntrance()
        {
            player.transform.position = dungeonEntranceSpawnPoint.position;
        }
        
        private void OnNewDay()
        {
            player.transform.position = houseSpawnPoint.position;
            RefillPlayer();
        }
        
        private void OnLoadGame()
        {
            player.transform.position = houseSpawnPoint.position;
            RefillPlayer();
        }
        
        private void OnHouse()
        {
            player.transform.position = houseSpawnPoint.position;
            RefillPlayer();
        }
        
        private void RefillPlayer()
        {
            var _playerChar = player.GetComponent<PlayerCharacter>();
            _playerChar.Refill();
        }
    }
}
