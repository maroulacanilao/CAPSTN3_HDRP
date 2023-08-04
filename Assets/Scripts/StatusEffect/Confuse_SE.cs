using System.Collections;
using System.Text;
using BaseCore;
using BattleSystem;
using BattleSystem.BattleState;
using CustomHelpers;
using Managers;
using UI.Battle;
using UnityEngine;

namespace StatusEffect
{
    public class Confuse_SE : StatusEffectBase
    {
        [SerializeField] private string vfxName;
        
        protected override IEnumerator OnActivate()
        {
            if(!GameManager.IsBattleSceneActive()) yield break;
            var _sb = new StringBuilder();
            _sb.Append("<color=red>");
            _sb.Append(Target.character.characterData.characterName);
            _sb.Append("</color>");
            _sb.Append(" is confuse by all of the commotion!");
            
            yield return BattleTextManager.DoWrite(_sb.ToString());
        }
        
        protected override void OnDeactivate()
        {
            
        }
        
        protected override void OnStackEffect(StatusEffectBase newEffect_)
        {
            SetDurationLeft(turnsLeft + turnDuration);
        }


        protected override IEnumerator OnBeforeTurnTick(TurnBaseState ownerTurnState_)
        {
            var _target = BattleSystem.BattleManager.Instance.GetPartyOf(Target.character);
            var _randomTarget = _target[Random.Range(0, _target.Count)];

            var _battleCharacter = Target.battleCharacter;

            yield return _randomTarget.BasicAttack(_battleCharacter);
            
            AssetHelper.PlayEffectCoroutine(vfxName, Target.character.transform, Vector3.zero, Quaternion.identity);

            RemoveTurn();

            var _msg = _battleCharacter == _randomTarget ? 
                $"{_battleCharacter.characterData.characterName} is confused and attacked itself!" : 
                $"{_battleCharacter.characterData.characterName} is confused and attacked {_randomTarget.characterData.characterName}!";

            yield return null;
            
            yield return BattleTextManager.DoWrite(_msg);
            
            yield return CoroutineHelper.GetWait(0.1f);
            
            yield return ownerTurnState_.StateMachine.NextTurnState();
        }

    }
}
