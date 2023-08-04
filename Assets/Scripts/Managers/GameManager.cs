using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using BaseCore;
using BattleSystem;
using Character;
using CustomEvent;
using CustomHelpers;
using Dungeon;
using EnemyController;
using Farming;
using Managers.SceneLoader;
using NaughtyAttributes;
using Player;
using ScriptableObjectData;
using ScriptableObjectData.CharacterData;
using UnityEngine;
using UnityEngine.SceneManagement;
using LoadSceneParameters = Managers.SceneLoader.LoadSceneParameters;

namespace Managers
{
    [DefaultExecutionOrder(-999)]
    public class GameManager : SingletonPersistent<GameManager>
    {
        [field: BoxGroup("Data")] [field: SerializeField] 
        public GameDataBase GameDataBase { get; private set; }
        
        [field: BoxGroup("Data")] [field: SerializeField] 
        public BattleData BattleData { get; private set; }
        
        [field: BoxGroup("Data")] [field: SerializeField] 
        public EventQueueData EventQueueData { get; private set; }
        
        
        [field: BoxGroup("SceneName")] [field: SerializeField] [field: NaughtyAttributes.Scene]
        public string FarmSceneName { get; private set; }
        
        [field: BoxGroup("SceneName")] [field: SerializeField] [field: NaughtyAttributes.Scene]
        public string TutorialSceneName { get; private set; }
        
        [field: BoxGroup("SceneName")] [field: SerializeField] [field: NaughtyAttributes.Scene]
        public string BattleSceneName { get; private set; }
        
        [field: BoxGroup("SceneName")] [field: SerializeField] [field: NaughtyAttributes.Scene]
        public string DungeonSceneName { get; private set; }


        public PlayerData PlayerData => GameDataBase.playerData;
        
        private PlayerCharacter mPlayer;
        private PlayerCharacter mPLayerOnBattle;
        private EnemyCharacter mCurrentEnemy;
        public PlayerCharacter Player
        {
            get
            {
                if(mPlayer == null)
                {
                    mPlayer = GameObject.FindGameObjectsWithTag("Player")
                        .FirstOrDefault(g => g.TryGetComponent(out PlayerInputController _))
                        .GetComponent<PlayerCharacter>();
                }
                if(mPlayer == null)
                {
                    mPlayer = SceneHelper.FindComponentInPersistentScene<PlayerCharacter>();
                }
                return mPlayer;
            }
        }
        
        public PlayerCharacter PlayerOnBattle
        {
            get
            {
                if(mPLayerOnBattle== null)
                {
                    var _scene = UnityEngine.SceneManagement.SceneManager.GetSceneByName(BattleSceneName);
                    
                    if (!_scene.isLoaded || !_scene.IsValid()) return null;
                    
                    mPLayerOnBattle = _scene.FindFirstComponentInScene<PlayerCharacter>();
                }
                
                return mPLayerOnBattle;
            }
        }
        
        public EnemyCharacter CurrentEnemy => mCurrentEnemy.IsEmptyOrDestroyed() ? null : mCurrentEnemy;

        public static readonly Evt<EnemyCharacter, bool> OnEnterBattle = new Evt<EnemyCharacter, bool>();
        public static readonly Evt<BattleResultType> OnExitBattle = new Evt<BattleResultType>();

        protected override void Awake()
        {
            base.Awake();
            ItemHelper.Initialize(GameDataBase);
            AssetHelper.Initialize(GameDataBase);
            GameDataBase.Initialize();
            EventQueueData.InitializeQueue();
            
            OnEnterBattle.AddListener(EnterBattle);
            OnExitBattle.AddListener(ExitBattle);

            var _player = GameObject.FindWithTag("Player");
            
            if(_player == null) return;
            mPlayer = _player.GetComponent<PlayerCharacter>();
            Fungus.FungusManager.Instance.useUnscaledTime = true;
        }

        private void Start()
        {
            TimeManager.OnEndDay.AddListener(OnEndDay);
            Fungus.FungusManager.Instance.useUnscaledTime = true;
        }

        protected void OnDestroy()
        {
            TimeManager.OnEndDay.RemoveListener(OnEndDay);
            GameDataBase.DeInitialize();
            EventQueueData.DeInitializeQueue();
            OnEnterBattle.RemoveListener(EnterBattle);
            OnExitBattle.RemoveListener(ExitBattle);
        }
        
        private void EnterBattle(EnemyCharacter enemyCharacter_, bool isPlayerFirst_)
        {
            if (SceneLoader.SceneLoader.IsSceneAlreadyActive(BattleSceneName))
            {
                Time.timeScale = 1;
                return;
            }
            if (enemyCharacter_.IsEmptyOrDestroyed())
            {
                Time.timeScale = 1;
                return;
            }
            
            StartCoroutine(Co_EnterBattle(enemyCharacter_, isPlayerFirst_));
        }
        
        private IEnumerator Co_EnterBattle(EnemyCharacter enemyCharacter_, bool isPlayerFirst_)
        {
            BattleData.EnterBattle(enemyCharacter_, isPlayerFirst_);
            mCurrentEnemy = enemyCharacter_;
            Time.timeScale = 0;
            var _transform = Player.transform;

            yield return null;

            var _sceneParameter = new LoadSceneParameters(LoadSceneType.LoadAdditive,BattleSceneName , BattleSceneName, false, 3);
            SceneLoader.SceneLoader.OnLoadScene.Invoke(_sceneParameter);

            yield return new WaitForSeconds(0.1f);
        }
        
        private void ExitBattle(BattleResultType result_)
        {
            switch (result_)
            {
                case BattleResultType.Win:
                    OnBattleWon();
                    break;
                case BattleResultType.Lose:
                    OnBattleLost();
                    break;
                case BattleResultType.Flee:
                    OnBattleFlee();
                    break;
            }
        }

        private void OnBattleWon()
        {
            if(!BattleData.currentEnemyData) return;
            
            Debug.Log($"<color=red>BATTLE WON</color>");
            var _data = CurrentEnemy.characterData as EnemyData;
            var _lootData = new LootDropData()
            {
                level = CurrentEnemy.level,
                lootTable = BattleData.currentEnemyData.LootTable,
                position = CurrentEnemy.transform.position
            };
            
            DungeonManager.OnEnemyDeath.Invoke(CurrentEnemy);
            
            void SpawnLoot()
            {
                GameDataBase.enemyDataBase.AddKills(_data);
                
                LootSpawner.OnSpawnLoot.Invoke(_lootData);
                TimeManager.AddMinutes(BattleData.minutesJumped);
            }
            
            EventQueueData.AddEvent(DungeonSceneName, SpawnLoot);

            BattleData.ResetData();
            //Load Scene Farm
            
            var _sceneParameter = new LoadSceneParameters(LoadSceneType.Unload, BattleSceneName, DungeonSceneName, false, 3);
            SceneLoader.SceneLoader.OnLoadScene.Invoke(_sceneParameter);
        }

        private void OnBattleLost()
        {
            GameDataBase.sessionData.farmLoadType = FarmLoadType.NewDay;
            
            void AdvanceDay()
            {
                TimeManager.EndDay();
            }
            
            EventQueueData.AddEvent(FarmSceneName, AdvanceDay);
            
            var _sceneParameter = new LoadSceneParameters(LoadSceneType.UnloadAllExcept, DungeonSceneName, FarmSceneName, false, 3);
            SceneLoader.SceneLoader.OnLoadScene.Invoke(_sceneParameter);
        }

        private void OnBattleFlee()
        {
            void Flee()
            {
                if(CurrentEnemy.IsEmptyOrDestroyed()) return;
                
                var _enemyController = CurrentEnemy.GetComponent<EnemyAIController>();
                
                if(_enemyController == null) return;
                
                var _station = _enemyController.station;
                
                if(_station == null) return;
                
                _enemyController.Initialize(_station);
            }
            
            EventQueueData.AddEvent(FarmSceneName, Flee);
            
            var _sceneParameter = new LoadSceneParameters(LoadSceneType.Unload, BattleSceneName, DungeonSceneName, false, 3);
            SceneLoader.SceneLoader.OnLoadScene.Invoke(_sceneParameter);
        }

        public static bool IsBattleSceneActive()
        {
            return UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == Instance.BattleSceneName;
        }

        public static bool IsFarmSceneActive()
        {
            var _active = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == Instance.FarmSceneName;
            return SceneManager.sceneCount == 1 && _active;
        }
        
        public static bool IsDungeonSceneActive()
        {
            var _active = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == Instance.DungeonSceneName;
            return SceneManager.sceneCount == 1 && _active;
        }
        
        public static bool IsFarmOrTutorialSceneActive()
        {
            var _active = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == Instance.FarmSceneName;
            var _active2 = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == Instance.TutorialSceneName;
            return SceneManager.sceneCount == 1 && (_active || _active2);
        }

        private void OnEndDay()
        {
            GameDataBase.sessionData.farmLoadType = FarmLoadType.NewDay;
            
            if (SceneManager.sceneCount > 1 && SceneManager.GetActiveScene().name != FarmSceneName)
            {
                var _sceneParameter = new LoadSceneParameters(LoadSceneType.UnloadAllExcept, DungeonSceneName, FarmSceneName, false, 3);
                SceneLoader.SceneLoader.OnLoadScene.Invoke(_sceneParameter);
            }
            else
            {
                FarmSceneManager.Instance.SetSceneOnType();
            }
        }
        
        public static void DoAction(Action action_)
        {
            action_?.Invoke();
        }

        public static void Save()
        {
            if(IsInstanceNull()) return;
            
            Instance.GameDataBase.progressionData.SaveProgression();
        }

        public static void DelaySendToFungus(string message_, float delaySeconds_, int delayFrames_ = 0)
        {
            if(Instance.IsEmptyOrDestroyed()) return;
            Instance.StartCoroutine(Co_DelaySendToFungus(message_, delaySeconds_,delayFrames_));
        }
        
        public static IEnumerator Co_DelaySendToFungus(string message_, float delay_, int delayFrames_ = 0)
        {
            if(delay_ > 0 ) yield return new WaitForSeconds(delay_);
            yield return new WaitForFrames(delayFrames_);
            Fungus.Flowchart.BroadcastFungusMessage(message_);
        }
    }
}
