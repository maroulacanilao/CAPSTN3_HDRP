using System.Collections;
using UnityEngine;
using CustomHelpers;

namespace BattleSystem.BattleState
{
    public class BattleStartState : BattleStateBase
    {

        public BattleStartState(BattleStateMachine stateMachine_) : base(stateMachine_)
        {

        }

        public override IEnumerator Enter()
        {
            Debug.Log("Start Battle");
            yield return new WaitForSeconds(2f);
            yield return StateMachine.ChangeState(StateMachine.playerTurn);
        }

        public override IEnumerator Exit()
        {
            yield break;
        }
    }
}