using System.Collections;
using BaseCore;
using BattleSystem;
using Character;
using CustomHelpers;
using Managers;
using ObjectPool;
using Spells.Base;
using StatusEffect;
using UI.Battle;
using UnityEngine;

namespace Spells
{
    public class FlameThrower_Spell : SpellObject
    {
        [SerializeField]
        public string projectileAssetName = "FireBall";

        [Range(0,1f)] [SerializeField] 
        private float magicDmgBurnScale = .15f;
        
        protected override IEnumerator OnActivate()
        {
            if(target == null) yield break;
            DamageInfo _tempDamageInfo = new DamageInfo(damage, character.gameObject, spellData.tags, DamageType.Magical);
            var _atkResult = battleCharacter.GetAttackResult(target, _tempDamageInfo);

            yield return battleCharacter.PlaySpellCastAnim();

            var _projectile = AssetHelper.GetPrefab(projectileAssetName).GetInstance<Projectile>
                (battleCharacter.battleStation.projectilePosition, Quaternion.identity);
            

            yield return _projectile.StartProjectile(target.transform.position.SetY(1));
            _projectile.gameObject.ReturnInstance();

            target.Hit(_atkResult);
            
            if (_atkResult.attackResultType != AttackResultType.Miss)
            {
                var _effectInstance = Instantiate(spellData.statusEffect, Vector3.zero, Quaternion.identity);
                var _burnDmg = Mathf.RoundToInt(character.stats.intelligence * magicDmgBurnScale);
                
                if(_effectInstance == null)yield break;
                
                _effectInstance.SetDamage(_burnDmg);
                yield return target.character.statusEffectReceiver.ApplyStatusEffect(_effectInstance, character.gameObject);
            }
            
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
            
        }
    }
}
