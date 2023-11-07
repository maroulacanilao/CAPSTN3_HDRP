using System;
using Items.Inventory;
using Spells.Base;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.ShrineUI.GetSpells
{
    public class Shrine_SpellDetails : SpellDisplay
    {
        [Header("Shrine")]
        [SerializeField] private Shrine_GetSpells shrine;
        [SerializeField] private PlayerInventory inventory;
        [SerializeField] private TextMeshProUGUI errorTxt;
        [SerializeField] private Button offerBtn;
        [SerializeField] private ShrineRequirementItem requirementItem;
        public ShrineSpellItem currentSpellItem { get; private set; }

        [Header("Shrine")]
        [SerializeField] private Image spellIcon;

        protected void Awake()
        {
            DisplayNull();
            offerBtn.onClick.AddListener(OfferSpell);
        }
        
        public void DisplaySpell(ShrineSpellItem spellItem_)
        {
            if (spellItem_ == null)
            {
                DisplayNull();
                return;
            }
            currentSpellItem = spellItem_;
            DisplaySpell(spellItem_.spellData);
            
            if (spellItem_.offerRequirement.consumableData == null) throw new Exception("No consumable data found!");

            if (requirementItem != null) requirementItem.Set(spellItem_.offerRequirement);

            if (spellIcon != null) spellIcon.sprite = spellItem_.spellData.icon;

            if (requirementItem != null) requirementItem.gameObject.SetActive(true);
            
            var _canLearn = shrine.CanLearn(currentSpellItem.spellData, out var _error);
            
            if(_canLearn)
            {
                errorTxt.gameObject.SetActive(false);
                offerBtn.interactable = true;
            }
            else
            {
                errorTxt.gameObject.SetActive(true);
                errorTxt.text = _error;
                offerBtn.interactable = false;
            }
        }
        
        private void OfferSpell()
        {
            if(currentSpellItem == null) return;
            if(currentSpellItem.spellData == null) return;
            shrine.LearnSpell(currentSpellItem.spellData);
        }

        public override void DisplayNull()
        {
            base.DisplayNull();
            if (requirementItem != null) requirementItem.gameObject.SetActive(false);
            offerBtn.interactable = false;
        }

    }
}
