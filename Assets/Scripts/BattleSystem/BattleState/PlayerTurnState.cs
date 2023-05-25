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
            Debug.Log($"{battleCharacter.gameObject}'s Enter Turn");
            yield return battleCharacter.character.statusEffectReceiver.BeforeTurnTick(this);
            yield return StartTurn();
        }

        public override IEnumerator StartTurn()
        {
            Debug.Log($"{battleCharacter.gameObject}'s Turn");
            // Open UI
            yield return CoroutineHelper.GetWait(0.1f);
            yield return CheckForEndState();
            yield return TurnLogic();
        }

        public override IEnumerator TurnLogic()
        {
            BattleManager.OnPlayerTurnStart.Invoke(battleCharacter);
            yield return BattleManager.OnPlayerEndDecide.WaitForEvt();
            yield return EndTurn();
        }

        public override IEnumerator EndTurn()
        {
            yield return CoroutineHelper.GetWait(1f);
            yield return battleCharacter.character.statusEffectReceiver.AfterTurnTick(this);
            Debug.Log($"{battleCharacter.gameObject}'s End Turn");
            yield return CheckForEndState(true);
        }

        public override IEnumerator Exit()
        {
            Debug.Log($"{battleCharacter.gameObject}'s Exit State");
            yield break;
        }
    }
}