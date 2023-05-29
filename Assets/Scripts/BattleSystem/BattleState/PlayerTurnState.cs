using System.Collections;
using CustomHelpers;
using UnityEngine;

namespace BattleSystem.BattleState
{
    public class PlayerTurnState : TurnBaseState
    {
        public PlayerTurnState(BattleStateMachine stateMachine_, BattleCharacter character_) : base(stateMachine_, character_)
        {
            
        }

        protected override IEnumerator TurnLogic()
        {
            BattleManager.OnPlayerTurnStart.Invoke(battleCharacter);
            yield return BattleManager.OnPlayerEndDecide.WaitForEvt();
            yield return EndTurn();
        }
    }
}