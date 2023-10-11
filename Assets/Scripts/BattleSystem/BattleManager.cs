using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BaseCore;
using Character;
using CustomEvent;
using CustomHelpers;
using Managers;
using ScriptableObjectData.CharacterData;
using UI.Battle;
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

        public BattleStateMachine BattleStateMachine { get; private set; }

        public static readonly Evt<BattleCharacter> OnPlayerTurnStart = new Evt<BattleCharacter>();
        public static readonly Evt OnPlayerEndDecide = new Evt();
        public static readonly Evt<string> OnBattleEvent = new Evt<string>();
        public static readonly Evt<BattleResultType> OnBattleEnd = new Evt<BattleResultType>();

        protected override void Awake()
        {
            base.Awake();
            SpawnCharacters();
            BattleStateMachine = new BattleStateMachine(this);
        }
        
        private IEnumerator Start()
        {
            yield return null;
            yield return BattleStateMachine.Initialize();
        }

        private void OnEnable()
        {
            Cursor.visible = true;
        }

        public void End(BattleResultType result_)
        {
            StopAllCoroutines();
            OnBattleEnd.Invoke(result_);
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
            
            var _playerLevel = battleData.playerData.LevelData.CurrentLevel;
            var _player = playerPartyStation[0].Initialize(battleData.playerData, _playerLevel);
            
            playerParty.Add(_player);
            var _partyIndex = 1;
            battleData.playerData.alliesData.ForEach(a =>
            {
                var _allyLevel = a.LevelData.CurrentLevel;
                var _ally = playerPartyStation[_partyIndex].Initialize(a, _allyLevel);
                playerParty.Add(_ally);
                _partyIndex++;
            });
            
            var _enemyLevel = battleData.currentEnemyLevel;
            
            var _mainEnemy = enemyPartyStation[0].Initialize(battleData.currentEnemyData, _enemyLevel);
            enemyParty.Add(_mainEnemy);

            var _count = Random.Range(0, 3);
            var _index = 1;
            

            for (var i = 0; i < _count; i++)
            {
                var _enemyAllyData = battleData.currentEnemyData.alliesDictionary.GetWeightedRandom();
                if(_enemyAllyData == null) continue;
                
                if(enemyPartyStation.Count <= _index) break;
                
                var _lvl = _enemyAllyData.levelRange.GetRandomInRange();
                _lvl = Mathf.Clamp(_lvl, 1, _playerLevel);

                var _enemy = enemyPartyStation[_index].Initialize(_enemyAllyData, _lvl);
                
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

        public void TryFlee()
        {
            StartCoroutine(Co_TryFlee());
        }

        IEnumerator Co_TryFlee()
        {
            var _baseChance = 1f;

            var _mult = 0.35f;

            foreach (var enemy in enemyParty)
            {
                if(enemy.IsEmptyOrDestroyed()) continue;
                if(!enemy.character.IsAlive) continue;
                
                _baseChance *= _mult;
            }
            
            var _chance = Mathf.Clamp(_baseChance, 0.05f, 0.9f);
            
            var _roll = Random.Range(0f, 1f);
            
            var _success = _roll <= _chance;
            
            var _msg = _success ? "Flee Successful" : "Flee Failed";

            yield return BattleTextManager.DoWrite(_msg);
            
            if(_success) GameManager.OnExitBattle.Invoke(BattleResultType.Flee);
            else OnPlayerEndDecide.Invoke();
        }

        public List<BattleCharacter> GetOppositePartyOf(CharacterBase character_, bool aliveOnly_ = true)
        {
            var _party = new List<BattleCharacter>();
            
            _party = playerParty.Any(p => p.character == character_) ? enemyParty : playerParty;
            
            return !aliveOnly_ ? _party : _party.Where(c => c.character.IsAlive).ToList();
        }
        
        public List<BattleCharacter> GetPartyOf(CharacterBase character_, bool aliveOnly_ = true)
        {
            var _party = new List<BattleCharacter>();
            
            _party = playerParty.Any(p => p.character == character_) ? playerParty : enemyParty;
            
            return !aliveOnly_ ? _party : _party.Where(c => c.character.IsAlive).ToList();
        }
    }
}