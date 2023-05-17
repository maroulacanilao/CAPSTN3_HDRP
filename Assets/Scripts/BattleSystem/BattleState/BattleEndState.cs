using System.Collections;
using CustomHelpers;
using UnityEngine;

namespace BattleSystem.BattleState
{
    public class BattleEndState : BattleStateBase
    {
        private readonly bool didPlayerWon;
        
        public BattleEndState(BattleStateMachine stateMachine_, bool didPlayerWon_) : base(stateMachine_)
        {
            didPlayerWon = didPlayerWon_;
        }
        
        public override IEnumerator Enter()
        {
            var _txt = didPlayerWon ? "Player Won" : "Player Lost";
            yield return CoroutineHelper.GetWait(1f);
            BattleManager.End(didPlayerWon);
            yield break;
        }

        public override IEnumerator Exit()
        {
            yield break;
        }
    }
}