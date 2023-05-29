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

            yield return BattleTextManager.DoWrite("Battle Start!");
            
            yield return StateMachine.NextTurnState();
        }

        public override IEnumerator Exit()
        {
            yield break;
        }
    }
}