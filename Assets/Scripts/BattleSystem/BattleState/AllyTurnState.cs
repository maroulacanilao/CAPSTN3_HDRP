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
        public override IEnumerator StartTurn()
        {
            yield break;
        }

        public override IEnumerator TurnLogic()
        {
            yield break;
        }
        
        public override IEnumerator EndTurn()
        {
            yield break;
        }

        public override IEnumerator Exit()
        {
            yield break;
        }
    }
}