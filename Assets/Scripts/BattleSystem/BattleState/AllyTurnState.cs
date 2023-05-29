using System.Collections;

namespace BattleSystem.BattleState
{
    public class AllyTurnState : TurnBaseState
    {

        public AllyTurnState(BattleStateMachine stateMachine_, BattleCharacter character_) : base(stateMachine_, character_)
        {
            
        }
        
        public override IEnumerator Enter()
        {
            yield break;
        }
        protected override IEnumerator StartTurn()
        {
            yield break;
        }

        protected override IEnumerator TurnLogic()
        {
            yield break;
        }

        protected override IEnumerator EndTurn()
        {
            yield break;
        }

        public override IEnumerator Exit()
        {
            yield break;
        }
    }
}