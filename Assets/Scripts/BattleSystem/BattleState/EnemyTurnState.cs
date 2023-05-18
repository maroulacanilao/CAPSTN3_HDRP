using System.Collections;
using CustomHelpers;
using UnityEngine;

namespace BattleSystem.BattleState
{
    public class EnemyTurnState : TurnBaseState
    {
        public EnemyTurnState(BattleStateMachine stateMachine_, BattleCharacter battleCharacter_) : base(stateMachine_, battleCharacter_)
        {
        }

        public override IEnumerator Enter()
        {
            Debug.Log($"{battleCharacter.gameObject}'s Enter Turn");
            yield return CoroutineHelper.GetWait(0.1f);
            yield return battleCharacter.character.statusEffectReceiver.BeforeTurnTick(this);
            yield return StartTurn();
        }

        public override IEnumerator StartTurn()
        {
            Debug.Log($"{battleCharacter.gameObject}'s Turn");
            yield return CoroutineHelper.GetWait(.2f);
            yield return TurnLogic();
        }

        public override IEnumerator TurnLogic()
        {
            yield return battleCharacter.AttackTarget(BattleManager.Instance.player);
            yield return EndTurn();
        }

        public override IEnumerator EndTurn()
        {
            yield return CoroutineHelper.GetWait(0.1f);
            yield return battleCharacter.character.statusEffectReceiver.AfterTurnTick(this);
            Debug.Log($"{battleCharacter.gameObject}'s End Turn");
            if (BattleManager.player.HealthComponent.CurrentHp <= 0)
            {
                yield return StateMachine.ChangeState(new BattleEndState(StateMachine, false));
                yield break;
            }
            else yield return StateMachine.ChangeState(StateMachine.playerTurn);
        }

        public override IEnumerator Exit()
        {
            Debug.Log($"{battleCharacter.gameObject}'s Exit State");
            yield break;
        }
    }
}