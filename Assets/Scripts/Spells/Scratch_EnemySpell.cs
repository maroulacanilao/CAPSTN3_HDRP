using System.Collections;
using System.Text;
using BaseCore;
using BattleSystem;
using CustomHelpers;
using Managers;
using ObjectPool;
using Spells.Base;
using UI.Battle;
using UnityEngine;
using UnityEngine.VFX;


namespace Spells
{

    public class Scratch_EnemySpell : SpellObject
    {
        [SerializeField]
        public string scratchAssetName, sfxName;

        protected override IEnumerator OnActivate()
        {

            if (target == null) yield break;

            DamageInfo _tempDamageInfo = new DamageInfo(damage, character.gameObject, spellData.tags, DamageType.Physical);

            var _atkResult = battleCharacter.GetAttackResult(target, _tempDamageInfo);

            yield return battleCharacter.GoToPosition(target.battleStation.attackPosition);
            
            AssetHelper.PlayEffect(scratchAssetName,target.transform.position,Quaternion.Euler(15f,0f,0f));
            AudioManager.PlaySfx(sfxName);
            
            yield return battleCharacter.PlaySpellCastAnim();
            var _sb = new StringBuilder();
            _sb.Append(battleCharacter.character.characterData.characterName);
            _sb.Append(" scratched you!");
            target.Hit(_atkResult);
            yield return BattleTextManager.DoWrite(_sb.ToString());
            
            if (_atkResult.attackResultType == AttackResultType.Miss)
            {
                yield return BattleTextManager.DoWrite("But it missed!");
            }
            else
            {
                var _bleedInstance = Instantiate(spellData.statusEffect, Vector3.zero, Quaternion.identity);
                var _bleedDmg = Mathf.RoundToInt(character.stats.strength * 0.1f);
                _bleedDmg = Mathf.Clamp(_bleedDmg, 1, 9999);
                
                if (_bleedInstance != null)
                {
                    _bleedInstance.SetDamage(_bleedDmg);
                    yield return target.character.statusEffectReceiver.ApplyStatusEffect(_bleedInstance, character.gameObject);
                }
            }
            
            yield return battleCharacter.GoBackToStation();

            yield return new WaitForSeconds(0.5f);
            
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
