using System;
using BaseCore;
using BattleSystem;
using Character;
using CustomEvent;
using Items.Inventory;
using ScriptableObjectData;
using UnityEngine;

namespace Managers
{
    [DefaultExecutionOrder(-999)]
    public class GameManager : SingletonPersistent<GameManager>
    {
        [field: SerializeField] public GameDataBase GameDataBase { get; private set; }
        [field: SerializeField] public PlayerInventory playerInventory { get; private set; }
        [field: SerializeField] public BattleData BattleData { get; private set; }

        public static readonly Evt<PlayerCharacter, CharacterBase> OnEnterBattle = new Evt<PlayerCharacter, CharacterBase>();
        public static readonly Evt<PlayerCharacter, CharacterBase> OnExitBattle = new Evt<PlayerCharacter, CharacterBase>();
        
        protected override void Awake()
        {
            base.Awake();
            playerInventory.InitializeInventory();
            ItemHelper.Initialize(GameDataBase);
            OnEnterBattle.AddListener(EnterBattle);
            OnExitBattle.AddListener(ExitBattle);
        }

        protected void OnDestroy()
        {
            OnEnterBattle.RemoveListener(EnterBattle);
            OnExitBattle.RemoveListener(ExitBattle);
            playerInventory.DeInitializeInventory();
        }
        
        private void EnterBattle(PlayerCharacter playerCharacter_, CharacterBase enemyCharacter_)
        {
            BattleData.EnterBattle(playerCharacter_, enemyCharacter_);
            //Load Scene Battle
        }
        
        private void ExitBattle(PlayerCharacter playerCharacter_, CharacterBase enemyCharacter_)
        {
            BattleData.ResetData();
            //Load Scene Farm
        }
    }
}
