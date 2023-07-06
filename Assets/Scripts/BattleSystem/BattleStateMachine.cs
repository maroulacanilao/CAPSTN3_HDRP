using System;
using System.Collections;
using System.Collections.Generic;
using BattleSystem.BattleState;
using ScriptableObjectData.CharacterData;
using System.Linq;
using CustomHelpers;
using UnityEngine;

namespace BattleSystem
{
    public class BattleStateMachine
    {
        public BattleManager battleManager;
        public BattleData battleData;
        public TurnBaseState currentTurn;
        public PlayerTurnState playerTurn;
        public EnemyTurnState enemyTurn;
        
        public Queue<TurnBaseState> turnQueue = new Queue<TurnBaseState>();
        
        public BattleStateMachine(BattleManager battleManager_)
        {
            battleManager = battleManager_;
            battleData = battleManager.battleData;
        }

        public IEnumerator Initialize()
        {
            enemyTurn = new EnemyTurnState(this, battleManager.enemyParty[0]);

            turnQueue = new Queue<TurnBaseState>();
            var _characters = battleManager.playerParty.Concat(battleManager.enemyParty).ToList();

            _characters.Shuffle();
            
            Debug.Log(battleManager.battleData.isPlayerFirst);
            
            if (battleManager.battleData.isPlayerFirst)
            {
                var _player = _characters.Find(c => c.character.characterData is PlayerData);
                
                _characters.Remove(_player);
                _characters.Insert(0, _player);
            }

            else
            {
                var _enemy = _characters.Find(c => c.character.characterData is EnemyData);
                
                _characters.Remove(_enemy);
                _characters.Insert(0, _enemy);
            }

            yield return CoroutineHelper.GetWait(.5f);
            
            var _startPhase = new BattleStartState(this);
            foreach (var _character in _characters)
            {
                var _turnState = GetTurnState(_character);
                turnQueue.Enqueue(_turnState);
            }
            yield return _startPhase.Enter();
        }

        private TurnBaseState GetTurnState(BattleCharacter character_)
        {
            switch (character_.character.characterData)
            {
                case PlayerData _:
                    return new PlayerTurnState(this, character_);
                case EnemyData _: 
                    return new EnemyTurnState(this, character_);
                case AllyData _:
                    return new AllyTurnState(this, character_);
                default:
                    throw new Exception("NO TYPE");
            }
        }

        public IEnumerator NextTurnState()
        {
            if (currentTurn != null && currentTurn.battleCharacter.character.IsAlive)
            {
                yield return currentTurn.Exit();
                turnQueue.Enqueue(currentTurn);
            }
            
            do
            {
                if (turnQueue.Count == 0) throw new Exception("NO MORE TURN");
                currentTurn = turnQueue.Dequeue();
                
            } while(currentTurn == null || !currentTurn.battleCharacter.character.IsAlive);
            
            yield return currentTurn.Enter();
        }
    }
}