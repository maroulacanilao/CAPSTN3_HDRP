using System.Collections;
using CustomHelpers;
using Spells.Base;
using UnityEngine;

namespace Spells
{
    public class BuffStatusEffectSpell : SpellObject
    {
        protected override IEnumerator OnActivate()
        {
            if(target == null) throw new System.NotImplementedException();

            yield return battleCharacter.PlaySpellCastAnim();
            
            var _effectInstance = Instantiate(spellData.statusEffect, Vector3.zero, Quaternion.identity);
            target.character.statusEffectReceiver.ApplyStatusEffect(_effectInstance, character.gameObject);
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
