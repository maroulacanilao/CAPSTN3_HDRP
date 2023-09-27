using CustomHelpers;
using System.Collections;

namespace BattleSystem.BattleState
{
    public class AllyTurnState : TurnBaseState
    {

        public AllyTurnState(BattleStateMachine stateMachine_, BattleCharacter character_) : base(stateMachine_, character_)
        {
            
        }

        protected override IEnumerator TurnLogic()
        {
            yield return BattleTutorial.TurnTutorial();
            yield return null;
            BattleManager.OnPlayerTurnStart.Invoke(battleCharacter);
            yield return BattleManager.OnPlayerEndDecide.WaitForEvt();
            yield return EndTurn();
        }
    }
}