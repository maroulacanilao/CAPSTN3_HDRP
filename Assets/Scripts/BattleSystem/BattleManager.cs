using System.Collections.Generic;
using BaseCore;
using BattleSystem.BattleState;
using Character;
using CustomEvent;
using ScriptableObjectData.CharacterData;
using UnityEngine;

namespace BattleSystem
{
    public class BattleManager : Singleton<BattleManager>
    {
        [field: SerializeField] public BattleCharacter player { get; private set; }
        [field: SerializeField] public BattleCharacter enemy { get; private set; }
        [field: SerializeField] public BattleData battleData { get; private set; }

        public BattleStateMachine BattleStateMachine { get; private set; }

        public static readonly Evt<BattleCharacter> OnPlayerTurnStart = new Evt<BattleCharacter>();
        public static readonly Evt OnPlayerEndDecide = new Evt();
        public static readonly Evt<string> OnBattleEvent = new Evt<string>();
        public static readonly Evt<bool> OnBattleEnd = new Evt<bool>();

        public List<BattleCharacter> characterList; 

        protected override void Awake()
        {
            base.Awake();
            BattleStateMachine = new BattleStateMachine(this);
        }
        private void Start()
        {
            StartCoroutine(BattleStateMachine.Initialize());
        }

        public void End(bool hasWon_)
        {
            StopAllCoroutines();
            OnBattleEnd.Invoke(hasWon_);
        }
    }
}