using System.Collections;
using BaseCore;
using BattleSystem;
using Character;
using CustomHelpers;
using Spells.Base;
using StatusEffect;
using UnityEngine;

namespace Spells
{
    public class FlameThrower_Spell : SpellObject
    {
        [SerializeField]
        private Projectile projectile;
        
        [Range(0,1f)] [SerializeField] 
        private float magicDmgBurnScale = .15f;
        
        protected override IEnumerator OnActivate()
        {
            if(target == null) throw new System.NotImplementedException();
            DamageInfo _tempDamageInfo = new DamageInfo(damage, character.gameObject, DamageType.Magical);
            var _atkResult = battleCharacter.DamageTarget(target, _tempDamageInfo);

            yield return battleCharacter.PlaySpellCastAnim();
            
            var _projectile = Instantiate(projectile, 
                battleCharacter.battleStation.projectilePosition, 
                Quaternion.identity);

            yield return _projectile.StartProjectile(target.transform.position);
            Destroy(_projectile.gameObject);
            
            target.Hit(_atkResult);
            
            if (_atkResult.attackResultType != AttackResultType.Miss)
            {
                var _effectInstance = Instantiate(spellData.statusEffect, Vector3.zero, Quaternion.identity) as BurnFixedDamage_SE;
                var _burnDmg = Mathf.RoundToInt(character.stats.magicDamage * magicDmgBurnScale);
                
                _effectInstance.SetDamage(_burnDmg);
                target.character.statusEffectReceiver.ApplyStatusEffect(_effectInstance, character.gameObject);
            }
            battleCharacter.animator.SetTrigger(battleCharacter.moveAnimationHash);
            yield return CoroutineHelper.GetWait(0.2f);
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
