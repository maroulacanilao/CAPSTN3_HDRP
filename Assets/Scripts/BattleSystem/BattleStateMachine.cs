using System;
using System.Collections;
using System.Collections.Generic;
using BattleSystem.BattleState;
using ScriptableObjectData.CharacterData;
using System.Linq;

namespace BattleSystem
{
    public class BattleStateMachine
    {
        public BattleManager battleManager;
        public BattleData battleData;
        public BattleStateBase currentState;
        public PlayerTurnState playerTurn;
        public EnemyTurnState enemyTurn;
        
        public BattleStateMachine(BattleManager battleManager_)
        {
            battleManager = battleManager_;
            battleData = battleManager.battleData;
        }

        public IEnumerator Initialize()
        {
            playerTurn = new PlayerTurnState(this, battleManager.player);
            enemyTurn = new EnemyTurnState(this, battleManager.enemy);
            yield return ChangeState(new BattleStartState(this));
        }

        public IEnumerator ChangeState(BattleStateBase _newState)
        {
            yield return currentState?.Exit();
            currentState = _newState;
            if (currentState == null) throw new Exception("State is null");
            yield return currentState.Enter();
        }
        
        private BattleStateBase GetTurnState(BattleCharacter character_)
        {
            
            return null;
        }
    }
}