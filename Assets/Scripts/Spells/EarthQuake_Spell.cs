using System.Collections;
using System.Collections.Generic;
using BaseCore;
using BattleSystem;
using Cinemachine;
using CustomHelpers;
using DG.Tweening;
using Spells.Base;
using UnityEngine;

namespace Spells
{
    public class EarthQuake_Spell : SpellObject
    {
        protected override IEnumerator OnActivate()
        {
            var _targetParty = BattleManager.Instance.GetOppositePartyOf(character);

            if(_targetParty.Count == 0) yield break;
            
            yield return battleCharacter.PlaySpellCastAnim();

            yield return Effect();

            var _resList = new Dictionary<BattleCharacter,AttackResultType>();
            foreach (var _t in _targetParty)
            {
                if(_t == null || !_t.character.IsAlive) continue;
                Debug.Log("EarthQuake_Spell: " + _t.character.name + " is hit!");
                var _atkResult = battleCharacter.GetAttackResult(_t, new DamageInfo(damage, character.gameObject, spellData.tags, DamageType.Magical));
                _t.Hit(_atkResult);
                _resList.Add(_t,_atkResult.attackResultType);
            }

            foreach (var _result in _resList)
            {
                if(_result.Key == null || !_result.Key.character.IsAlive) continue;
                if(_result.Value == AttackResultType.Miss) continue;
                
                var _char = _result.Key;
                var _bleedInstance = Instantiate(spellData.statusEffect, Vector3.zero, Quaternion.identity);
                yield return _char.character.statusEffectReceiver.ApplyStatusEffect(_bleedInstance, character.gameObject);
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

        IEnumerator Effect()
        {
            var _cam = SceneHelper.FindComponentInActiveScene<CinemachineVirtualCamera>();
            if(_cam == null) yield break;
            var _lookAt = _cam.LookAt;
            if(_lookAt == null) yield break;
            yield return _lookAt.DOShakePosition(1f, 1.5f).WaitForCompletion();
        }
    }
}