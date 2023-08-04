using System.Collections;
using BaseCore;
using BattleSystem;
using CustomHelpers;
using ObjectPool;
using Spells.Base;
using UnityEngine;
using UnityEngine.VFX;

namespace Spells
{
    public class Lightning_Spell : SpellObject
    {
        [SerializeField]
        public string lightningAssetName;

        protected override IEnumerator OnActivate()
        {
            if(target == null) yield break;
            
            DamageInfo _tempDamageInfo = new DamageInfo(damage, character.gameObject, spellData.tags, DamageType.Magical);
            
            var _atkResult = battleCharacter.GetAttackResult(target, _tempDamageInfo);
            
            yield return battleCharacter.PlaySpellCastAnim();
            
            var _lightning = AssetHelper.GetPrefab(lightningAssetName).GetInstance(target.battleStation.projectilePosition, Quaternion.identity);
            
            var _vfx = _lightning.GetComponent<VisualEffect>();
            if(_vfx != null)
            {
                _vfx.Play();
            }

            yield return new WaitForSeconds(0.5f);
            
            target.Hit(_atkResult);
            battleCharacter.animator.SetTrigger(battleCharacter.moveAnimationHash);
            
            if(_atkResult.attackResultType == AttackResultType.Weakness) yield return Co_StunTarget(_atkResult);

            yield return CoroutineHelper.GetWait(0.2f);
            
            _lightning.ReturnInstance();
        }
        
        protected override IEnumerator OnDeactivate()
        {
            yield break;
        }
        
        protected override void OnRemoveSkill()
        {
            
        }
    }
}