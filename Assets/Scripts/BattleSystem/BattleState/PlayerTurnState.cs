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

        public override IEnumerator Enter()
        {
            Debug.Log($"{character.gameObject}'s Enter Turn");
            yield return StartTurn();
        }

        public override IEnumerator StartTurn()
        {
            Debug.Log($"{character.gameObject}'s Turn");
            // Open UI
            yield return CoroutineHelper.GetWait(0.1f);
            yield return TurnLogic();
        }

        public override IEnumerator TurnLogic()
        {
            BattleManager.OnPlayerTurnStart.Invoke(character);
            yield return BattleManager.OnPlayerEndDecide.WaitForEvt();
            yield return EndTurn();
        }

        public override IEnumerator EndTurn()
        {
            yield return CoroutineHelper.GetWait(1f);
            Debug.Log($"{character.gameObject}'s End Turn");
            if (BattleManager.enemy.HealthComponent.CurrentHp <= 0)
            {
                yield return StateMachine.ChangeState(new BattleEndState(StateMachine, true));
                yield break;
            }
            else yield return StateMachine.ChangeState(StateMachine.enemyTurn);
        }

        public override IEnumerator Exit()
        {
            Debug.Log($"{character.gameObject}'s Exit State");
            yield break;
        }
    }
}