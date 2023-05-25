using System.Collections;
using CustomHelpers;
using Spells.Base;

namespace Spells
{
    public class BuffStatusEffectSpell : SpellObject
    {
        protected override IEnumerator OnActivate()
        {
            if(target == null) throw new System.NotImplementedException();

            yield return battleCharacter.PlaySpellCastAnim();
            target.character.statusEffectReceiver.ApplyStatusEffect(spellData.statusEffect, character.gameObject);
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
