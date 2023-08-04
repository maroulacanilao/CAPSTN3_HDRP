using System;
using CustomHelpers;
using Spells.Base;
using UnityEngine;
using UnityEngine.UI;

namespace UI.TabMenu.CharacterInfo
{
    public class CharacterMenuSpellDisplay : SpellDisplay
    {
        [SerializeField] Button useBtn;

        private bool usable;
        private SpellData spellData;

        private void Start()
        {
            useBtn.onClick.AddListener(UseSpell);
        }
        public override void DisplaySpell(SpellData spellData_)
        {
            base.DisplaySpell(spellData_);
            
            usable = spellData_ != null && 
                          spellData_.spellType is SpellType.Buff or SpellType.Heal &&
                          playerData.mana.CurrentMana >= spellData_.manaCost &&
                          spellData_.hasStatusEffect && spellData_.statusEffect != null;
            
            useBtn.interactable = usable;
            
            spellData = spellData_;
        }
        
        private void UseSpell()
        {
            if(spellData == null) return;
            if (!usable)
            {
                useBtn.interactable = false;
            }
            var _statusEffect = spellData.statusEffect;
            var _seInstance = Instantiate(_statusEffect);
            playerData.statusEffectReceiver.ApplyStatusEffect(_seInstance).StartCoroutine();
            var _cost = spellData.manaCost;
            playerData.mana.UseMana(_cost);
        }
        
    }
}
