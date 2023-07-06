using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using BaseCore;
using BattleSystem;
using Character;
using CustomEvent;
using CustomHelpers;
using Dungeon;
using Managers.SceneLoader;
using NaughtyAttributes;
using Player;
using ScriptableObjectData;
using ScriptableObjectData.CharacterData;
using UnityEngine;

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
        public string BattleSceneName { get; private set; }
        
        [field: BoxGroup("SceneName")] [field: SerializeField] [field: NaughtyAttributes.Scene]
        public string DungeonSceneName { get; private set; }
        
        [field: BoxGroup("Camera")] [field: SerializeField] 
        
        private ToBattleCamera toBattleCamera { get; set; }
        
        
        public PlayerData PlayerData => GameDataBase.playerData;
        
        private PlayerCharacter mPlayer;
        private PlayerCharacter mPLayerOnBattle;
        private EnemyCharacter mCurrentEnemy;

        private float lastTimeBattle = -1;
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
        public static readonly Evt<bool> OnExitBattle = new Evt<bool>();

        protected override void Awake()
        {
            base.Awake();
            ItemHelper.Initialize(GameDataBase);
            GameDataBase.Initialize();
            EventQueueData.InitializeQueue();
            
            OnEnterBattle.AddListener(EnterBattle);
            OnExitBattle.AddListener(ExitBattle);

            var _player = GameObject.FindWithTag("Player");
            
            if(_player == null) return;
            mPlayer = _player.GetComponent<PlayerCharacter>();
        }

        protected void OnDestroy()
        {
            GameDataBase.DeInitialize();
            OnEnterBattle.RemoveListener(EnterBattle);
            OnExitBattle.RemoveListener(ExitBattle);
        }
        
        private void EnterBattle(EnemyCharacter enemyCharacter_, bool isPlayerFirst_)
        {
            StartCoroutine(Co_EnterBattle(enemyCharacter_, isPlayerFirst_));
        }
        
        private IEnumerator Co_EnterBattle(EnemyCharacter enemyCharacter_, bool isPlayerFirst_)
        {
            if(lastTimeBattle + 2f > Time.time) yield break; 
            if(enemyCharacter_.IsEmptyOrDestroyed()) yield break;
            
            lastTimeBattle = Time.time;
            
            BattleData.EnterBattle(enemyCharacter_, isPlayerFirst_);
            mCurrentEnemy = enemyCharacter_;
            Time.timeScale = 0;
            var _transform = Player.transform;

            yield return null;

            var _sceneParameter = new LoadSceneParameters(LoadSceneType.LoadAdditive,BattleSceneName , BattleSceneName, false, 3);
            SceneLoader.SceneLoader.OnLoadScene.Invoke(_sceneParameter);

            yield return new WaitForSeconds(1f);
        }
        
        private void ExitBattle(bool didPlayerWin_)
        {
            if(didPlayerWin_) OnBattleWon();
            else OnBattleLost();
        }

        private void OnBattleWon()
        {
            if(!BattleData.currentEnemyData) return;
            
            // Debug.Log($"<color=red>BATTLE WON</color>");

            var _lootData = new LootDropData()
            {
                level = CurrentEnemy.level,
                lootTable = BattleData.currentEnemyData.LootTable,
                position = CurrentEnemy.transform.position
            };
            
            void SpawnLoot()
            {
                lastTimeBattle = Time.time;
                GameDataBase.enemyDataBase.AddKills(CurrentEnemy.characterData as EnemyData);
                
                LootSpawner.OnSpawnLoot.Invoke(_lootData);
                DungeonManager.OnEnemyDeath.Invoke(CurrentEnemy);
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
            GameDataBase.sessionData.farmLoadType = FarmLoadType.House;
            
            void AdvanceDay()
            {
                TimeManager.EndDay();
            }
            
            EventQueueData.AddEvent(FarmSceneName, AdvanceDay);
            
            var _sceneParameter = new LoadSceneParameters(LoadSceneType.Unload, DungeonSceneName, DungeonSceneName, false, 3);
            SceneLoader.SceneLoader.OnLoadScene.Invoke(_sceneParameter);
        }
    }
}
