using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BaseCore;
using CustomEvent;
using CustomHelpers;
using ScriptableObjectData.CharacterData;
using UnityEngine;

namespace BattleSystem
{
    public class BattleManager : Singleton<BattleManager>
    {
        [field: SerializeField] public List<BattleCharacter> playerParty { get; private set; }
        [field: SerializeField] public List<BattleCharacter> enemyParty { get; private set; }
        [field: SerializeField] public BattleData battleData { get; private set; }
        [field: SerializeField] public List<BattleStation> playerPartyStation { get; private set; }
        [field: SerializeField] public List<BattleStation> enemyPartyStation { get; private set; }
        
        [SerializeField] private GameObject eventSystem;

        public BattleStateMachine BattleStateMachine { get; private set; }

        public static readonly Evt<BattleCharacter> OnPlayerTurnStart = new Evt<BattleCharacter>();
        public static readonly Evt OnPlayerEndDecide = new Evt();
        public static readonly Evt<string> OnBattleEvent = new Evt<string>();
        public static readonly Evt<bool> OnBattleEnd = new Evt<bool>();

        protected override void Awake()
        {
            base.Awake();
            SpawnCharacters();
            BattleStateMachine = new BattleStateMachine(this);
        }
        
        private IEnumerator Start()
        {
            eventSystem.SetActive(false);
            yield return null;
            eventSystem.SetActive(true);
            yield return BattleStateMachine.Initialize();
        }

        private void OnEnable()
        {
            Cursor.visible = true;
        }

        public void End(bool hasWon_)
        {
            StopAllCoroutines();
            OnBattleEnd.Invoke(hasWon_);
        }
        
        public bool IsEnemyPartyStillAlive()
        {
            return enemyParty.Any(e => e.character.IsAlive);
        }
        
        public bool IsPlayerPartyStillAlive()
        {
            return playerParty.Any(e => e.character.IsAlive);
        }
        
        public BattleCharacter GetFirstAliveEnemy()
        {
            return enemyParty.FirstOrDefault(e => e.character.IsAlive);
        }
        
        public void SpawnCharacters()
        {
            playerParty = new List<BattleCharacter>();
            enemyParty = new List<BattleCharacter>();
            
            var _level = battleData.playerData.LevelData.CurrentLevel;
            var _player = playerPartyStation[0].Initialize(battleData.playerData, _level);
            playerParty.Add(_player);
            
            var _index = 1;
            foreach (var _allyData in battleData.playerData.alliesData)
            {
                if(_allyData == null) continue;
                
                var _ally = playerPartyStation[_index].Initialize(_allyData, _level);
                playerParty.Add(_ally);
                _index++;
            }
            
            var _mainEnemy = enemyPartyStation[0].Initialize(battleData.currentEnemyData, _level);
            enemyParty.Add(_mainEnemy);

            var _count = UnityEngine.Random.Range(0, 3);
            _index = 1;
            for (var i = 0; i < _count; i++)
            {
                var _enemyAllyData = battleData.currentEnemyData.alliesDictionary.GetWeightedRandom();
                if(_enemyAllyData == null) continue;
                
                if(enemyPartyStation.Count <= _index) break;
                
                var _enemy = enemyPartyStation[_index].Initialize(_enemyAllyData, _level);
                
                enemyParty.Add(_enemy);
                _index++;
            }
        }

        public int GetTotalExp()
        {
            var _totalExp = 0;
            
            foreach (var _enemyData in enemyParty.Select(e => e.characterData as EnemyData))
            {
                if(_enemyData == null) continue;
                _totalExp += _enemyData.LootTable.possibleExperienceDrop.GetRandomInRange();
            }

            return _totalExp;
        }
    }
}