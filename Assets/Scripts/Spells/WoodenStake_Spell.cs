using System.Collections;
using BaseCore;
using BattleSystem;
using CustomHelpers;
using Spells.Base;
using UnityEngine;

namespace Spells
{
    public class WoodenStake_Spell : SpellObject
    {
        [SerializeField] private string woodenStakeAssetName;
        
        protected override IEnumerator OnActivate()
        {
            DamageInfo _tempDamageInfo = new DamageInfo(damage, character.gameObject, spellData.tags, DamageType.Magical);
            var _atkResult = battleCharacter.GetAttackResult(target, _tempDamageInfo);

            yield return battleCharacter.PlaySpellCastAnim();
            yield return AssetHelper.Co_PlayEffect(woodenStakeAssetName, target.battleStation.projectileTarget, Vector3.zero, Quaternion.identity);
            target.Hit(_atkResult);
            
            battleCharacter.animator.SetTrigger(battleCharacter.moveAnimationHash);
            
            if(_atkResult.attackResultType == AttackResultType.Weakness) yield return Co_StunTarget(_atkResult);

            yield return CoroutineHelper.GetWait(0.2f);
        }
        
        protected override IEnumerator OnDeactivate()
        {
            yield break;
        }
        protected override void OnRemoveSkill()
        {
            return;
        }
    }
}