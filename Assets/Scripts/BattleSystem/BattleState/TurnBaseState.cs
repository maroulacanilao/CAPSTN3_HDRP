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
        protected BattleCharacter battleCharacter;
        
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
    }
}