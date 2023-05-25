using System;
using System.Collections;

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
        
        public TurnBaseState(BattleStateMachine stateMachine_, BattleCharacter battleCharacter_) : base(stateMachine_)
        {
            battleCharacter = battleCharacter_;
            if (battleCharacter == null) throw new Exception("Character is null");
        }
        public override abstract IEnumerator Enter();
        public abstract IEnumerator StartTurn();
        public abstract IEnumerator TurnLogic();
        public abstract IEnumerator EndTurn();
        public override abstract IEnumerator Exit();

        protected IEnumerator CheckForEndState(bool willEndCurrentState_ = false)
        {
            if (!BattleManager.IsEnemyPartyStillAlive())
            {
                yield return new BattleEndState(StateMachine, true).Enter();
                yield break;
            }
            else if (!BattleManager.IsPlayerPartyStillAlive())
            {
                yield return new BattleEndState(StateMachine, false).Enter();
                yield break;
            }
            if (willEndCurrentState_) yield return StateMachine.NextTurnState();
            yield break;
        }
    }
}