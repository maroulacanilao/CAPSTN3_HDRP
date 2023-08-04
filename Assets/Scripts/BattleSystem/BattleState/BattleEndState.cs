using System.Collections;
using System.Linq;
using Character;
using CustomHelpers;
using UnityEngine;

namespace BattleSystem.BattleState
{
    public class BattleEndState : BattleStateBase
    {
        private readonly BattleResultType result;
        
        public BattleEndState(BattleStateMachine stateMachine_, BattleResultType result_) : base(stateMachine_)
        {
            result = result_;
        }
        
        public override IEnumerator Enter()
        {
            var _player = BattleManager.playerParty.FirstOrDefault(c => c.character is PlayerCharacter);
            if (_player != null) yield return _player.character.statusEffectReceiver.RemoveAllStatusEffect();
            
            yield return CoroutineHelper.GetWait(2f);
            BattleManager.End(result);
            
            yield break;
        }

        public override IEnumerator Exit()
        {
            yield break;
        }
    }
}