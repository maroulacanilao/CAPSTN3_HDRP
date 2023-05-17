using System.Collections;
using CustomHelpers;
using UnityEngine;

namespace BattleSystem.BattleState
{
    public class EnemyTurnState : TurnBaseState
    {
        public EnemyTurnState(BattleStateMachine stateMachine_, BattleCharacter character_) : base(stateMachine_, character_)
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
            yield return CoroutineHelper.GetWait(.5f);
            yield return TurnLogic();
        }

        public override IEnumerator TurnLogic()
        {
            yield return character.AttackTarget(BattleManager.Instance.player);
            yield return EndTurn();
        }

        public override IEnumerator EndTurn()
        {
            yield return CoroutineHelper.GetWait(0.1f);
            Debug.Log($"{character.gameObject}'s End Turn");
            if (BattleManager.player.HealthComponent.CurrentHp <= 0)
            {
                yield return StateMachine.ChangeState(new BattleEndState(StateMachine, false));
                yield break;
            }
            else yield return StateMachine.ChangeState(StateMachine.playerTurn);
        }

        public override IEnumerator Exit()
        {
            Debug.Log($"{character.gameObject}'s Exit State");
            yield break;
        }
    }
}