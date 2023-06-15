using System;
using BaseCore;
using BattleSystem;
using Character;
using CustomEvent;
using CustomHelpers;
using Items.Inventory;
using Managers.SceneManager;
using ScriptableObjectData;
using ScriptableObjectData.CharacterData;
using UnityEngine;

namespace Managers
{
    [DefaultExecutionOrder(-999)]
    public class GameManager : SingletonPersistent<GameManager>
    {
        [field: SerializeField] public GameDataBase GameDataBase { get; private set; }
        [field: SerializeField] public BattleData BattleData { get; private set; }
        [field: SerializeField] public EventQueueData EventQueueData { get; private set; }
        
        [field: SerializeField] [field: NaughtyAttributes.Scene]
        public string FarmSceneName { get; private set; }
        
        [field: SerializeField] [field: NaughtyAttributes.Scene]
        public string BattleSceneName { get; private set; }
        
        public PlayerData PlayerData => GameDataBase.playerData;
        
        private PlayerCharacter mPlayerOnFarm;
        private EnemyCharacter mCurrentEnemy;
        public PlayerCharacter PlayerOnFarm
        {
            get
            {
                if(mPlayerOnFarm == null)
                {
                    mPlayerOnFarm = SceneHelper.FindComponentInActiveScene<PlayerCharacter>();
                }
                return mPlayerOnFarm;
            }
        }
        public EnemyCharacter CurrentEnemy => mCurrentEnemy.IsEmptyOrDestroyed() ? null : mCurrentEnemy;

        public static readonly Evt<EnemyCharacter> OnEnterBattle = new Evt<EnemyCharacter>();
        public static readonly Evt<bool> OnExitBattle = new Evt<bool>();

        protected override void Awake()
        {
            base.Awake();
            ItemHelper.Initialize(GameDataBase);
            GameDataBase.Initialize();
            OnEnterBattle.AddListener(EnterBattle);
            OnExitBattle.AddListener(ExitBattle);

            var _player = GameObject.FindWithTag("Player");
            
            if(_player == null) return;
            mPlayerOnFarm = _player.GetComponent<PlayerCharacter>();
        }

        protected void OnDestroy()
        {
            GameDataBase.DeInitialize();
            OnEnterBattle.RemoveListener(EnterBattle);
            OnExitBattle.RemoveListener(ExitBattle);
        }
        
        private void EnterBattle(EnemyCharacter enemyCharacter_)
        {
            if(enemyCharacter_.IsEmptyOrDestroyed()) return;
            
            BattleData.EnterBattle(enemyCharacter_);
            mCurrentEnemy = enemyCharacter_;
            //Load Scene Battle
            Debug.Log("ENTER BATTLE");
            Time.timeScale = 0;
            SceneLoader.OnLoadScene.Invoke(BattleSceneName, LoadSceneType.LoadAdditive);
        }
        
        private void ExitBattle(bool didPlayerWin_)
        {
            if(didPlayerWin_) OnBattleWon();
            else OnBattleLost();
        }

        private void OnBattleWon()
        {
            var _pos = CurrentEnemy.transform.position;

            if(!BattleData.currentEnemyData) return;
        
            var _lootTable = BattleData.currentEnemyData.LootTable;
            
            void SpawnLoot()
            {
                GameDataBase.enemyDataBase.AddKills(CurrentEnemy.characterData as EnemyData);
                
                LootSpawner.OnSpawnLoot.Invoke(_lootTable, _pos);
                EnemySpawner.Instance.RemoveEnemy(CurrentEnemy.gameObject);
            }
            EventQueueData.AddEvent(SpawnLoot);

            BattleData.ResetData();
            //Load Scene Farm
            SceneLoader.OnLoadScene.Invoke(BattleSceneName, LoadSceneType.Unload);
        }

        private void OnBattleLost()
        {
            
        }
    }
}
