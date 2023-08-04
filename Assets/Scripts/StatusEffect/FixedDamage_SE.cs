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
    public class FixedDamage_SE : StatusEffectBase
    {
        [SerializeField] private string applyText;
        [SerializeField] private string vfxName;
        
        protected override IEnumerator OnActivate()
        {
            if(!GameManager.IsBattleSceneActive()) yield break;
            var _name = $"<color=red>{Target.character.characterData.characterName}</color>";
            var _msg = applyText.Replace("NAME", _name).Beautify();
            
            yield return AssetHelper.Co_PlayEffect(vfxName, Target.character.transform, Vector3.zero, Quaternion.identity, 1f);

            yield return BattleTextManager.DoWrite(_msg);
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
            DamageInfo _damageInfo = new DamageInfo(Damage, Source, effectTags);
            AttackResult _attackResult = new AttackResult()
            {
                attackResultType = AttackResultType.Hit,
                damageInfo = _damageInfo
            };
            Target.battleCharacter.Hit(_attackResult);
            
            RemoveTurn();
            
            var _msg = BattleText
                .Replace("NAME", characterName)
                .Replace("DMG", _damageInfo.DamageAmount.ToString());
            
            yield return AssetHelper.Co_PlayEffect(vfxName, Target.character.transform, Vector3.zero, Quaternion.identity, 1f);
            
            yield return null;
            
            yield return BattleTextManager.DoWrite(_msg.Beautify());
            
            yield return CoroutineHelper.GetWait(0.1f);
        }
    }
}
