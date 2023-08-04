using System.Collections;
using System.Collections.Generic;
using BaseCore;
using BattleSystem;
using CustomHelpers;
using DG.Tweening;
using Spells.Base;
using UnityEngine;

namespace Spells
{
    public class HolyRain_Spell : SpellObject
    {
        [SerializeField] private string vfxName;
        protected override IEnumerator OnActivate()
        {
            var _targetParty = BattleManager.Instance.GetOppositePartyOf(character);

            if(_targetParty.Count == 0) yield break;
            
            yield return battleCharacter.PlaySpellCastAnim();
            
            AssetHelper.PlayEffectCoroutine(vfxName, _targetParty[0].battleStation.transform.position, Quaternion.identity, 1f);

            yield return new WaitForSeconds(1f);

            var _resList = new Dictionary<BattleCharacter,AttackResultType>();
            foreach (var _t in _targetParty)
            {
                if(_t == null || !_t.character.IsAlive) continue;
                var _atkResult = battleCharacter.GetAttackResult(_t, new DamageInfo(damage, character.gameObject, spellData.tags, DamageType.Magical));
                _t.Hit(_atkResult);
                _resList.Add(_t,_atkResult.attackResultType);
            }

            foreach (var _result in _resList)
            {
                if(_result.Key == null || !_result.Key.character.IsAlive) continue;
                if(_result.Value == AttackResultType.Miss) continue;
                
                var _char = _result.Key;
                var _effect = Instantiate(spellData.statusEffect, Vector3.zero, Quaternion.identity);
                var _dmg = Mathf.RoundToInt(damage * 0.1f);
                _effect.SetDamage(_dmg);
                yield return _char.character.statusEffectReceiver.ApplyStatusEffect(_effect, character.gameObject);
            }

            battleCharacter.animator.SetTrigger(battleCharacter.moveAnimationHash);
            
            yield return CoroutineHelper.GetWait(0.5f);
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
