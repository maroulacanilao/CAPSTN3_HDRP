using System.Collections;
using UnityEngine;
using CustomHelpers;
using UI.Battle;

namespace BattleSystem.BattleState
{
    public class BattleStartState : BattleStateBase
    {

        public BattleStartState(BattleStateMachine stateMachine_) : base(stateMachine_)
        {

        }

        public override IEnumerator Enter()
        {
            yield return null;

            yield return BattleTutorial.Welcome();
            
            yield return new WaitForSeconds(0.1f);

            var _text = $"You encountered {BattleManager.enemyParty[0].characterData.characterName.SurroundWithColor(Color.red)}!";

            yield return BattleTextManager.DoWrite(_text);
            
            yield return new WaitForSeconds(0.5f);

            var _advantage = BattleManager.battleData.isPlayerFirst ? $"{"You".SurroundWithColor(Color.green)} have" : $"{"Enemy".SurroundWithColor(Color.red)} has";
            
            _text = $"{_advantage} the advantage!";
            
            yield return BattleTextManager.DoWrite(_text);

            yield return StateMachine.NextTurnState();
        }

        public override IEnumerator Exit()
        {
            yield break;
        }
    }
}