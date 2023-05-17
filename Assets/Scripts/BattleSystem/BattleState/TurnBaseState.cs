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
        protected BattleCharacter character;
        
        public TurnBaseState(BattleStateMachine stateMachine_, BattleCharacter character_) : base(stateMachine_)
        {
            character = character_;
            if (character == null) throw new Exception("Character is null");
        }
        public override abstract IEnumerator Enter();
        public abstract IEnumerator StartTurn();
        public abstract IEnumerator TurnLogic();
        public abstract IEnumerator EndTurn();
        public override abstract IEnumerator Exit();
    }
}