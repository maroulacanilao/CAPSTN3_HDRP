using System.Collections;
using UnityEngine;
using CustomHelpers;
using UI.Battle;

namespace BattleSystem.BattleState
{
    public class BattleStartState : BattleStateBase
    {
        Color playerColor;
        Color enemyColor;

        public BattleStartState(BattleStateMachine stateMachine_) : base(stateMachine_)
        {

        }

        public override IEnumerator Enter()
        {
            yield return null;

            yield return BattleTutorial.Welcome();
            
            yield return new WaitForSeconds(0.1f);

            playerColor = new(0.1215686f, 0.572549f, 0.3294118f, 1);
            enemyColor = new(0.6862745f, 0.007843138f, 0.2392157f, 1);

            var _text = $"You encountered {BattleManager.enemyParty[0].characterData.characterName.SurroundWithColor(enemyColor)}!";

            yield return BattleTextManager.DoWrite(_text);
            
            yield return new WaitForSeconds(0.5f);

            var _advantage = BattleManager.battleData.isPlayerFirst ? $"{"You".SurroundWithColor(playerColor)} have" : $"{"Enemy".SurroundWithColor(enemyColor)} has";
            
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