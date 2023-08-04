using System;
using System.Collections;
using CustomHelpers;
using UnityEngine;

namespace BattleSystem.BattleState
{
    public enum TurnPhase
    {
        Start,
        Turn,
        End
    }

    public abstract class TurnBaseState : BattleStateBase
    {
        public BattleCharacter battleCharacter { get; protected set; }

        protected TurnBaseState(BattleStateMachine stateMachine_, BattleCharacter battleCharacter_) : base(stateMachine_)
        {
            battleCharacter = battleCharacter_;
            if (battleCharacter == null) throw new Exception("Character is null");
        }
        
        public override IEnumerator Enter()
        {
            if (!battleCharacter.character.IsAlive)
            {
                yield return StateMachine.NextTurnState();
            }
            yield return CoroutineHelper.GetWait(0.15f);
            yield return battleCharacter.character.statusEffectReceiver.BeforeTurnTick(this);
            yield return StartTurn();
        }

        protected virtual IEnumerator StartTurn()
        {
            yield return CheckForEndState();
            yield return CoroutineHelper.GetWait(.2f);
            yield return TurnLogic();
        }

        protected abstract IEnumerator TurnLogic();

        protected virtual IEnumerator EndTurn()
        {
            yield return CoroutineHelper.GetWait(0.1f);
            yield return battleCharacter.character.statusEffectReceiver.AfterTurnTick(this);

            yield return CheckForEndState(true);
        }
        
        public override IEnumerator Exit()
        {
            yield break;
        }

        protected IEnumerator CheckForEndState(bool willEndCurrentState_ = false)
        {
            if (!BattleManager.IsEnemyPartyStillAlive())
            {
                yield return new BattleEndState(StateMachine, BattleResultType.Win).Enter();
                yield break;
            }
            else if (!BattleManager.IsPlayerPartyStillAlive())
            {
                yield return new BattleEndState(StateMachine, BattleResultType.Lose).Enter();
                yield break;
            }
            if (willEndCurrentState_) yield return StateMachine.NextTurnState();
            yield break;
        }
    }
}