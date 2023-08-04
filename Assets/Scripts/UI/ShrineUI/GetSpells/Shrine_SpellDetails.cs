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
            
            requirementItem.Set(spellItem_.offerRequirement);
            requirementItem.gameObject.SetActive(true);
            
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
            requirementItem.gameObject.SetActive(false);
            offerBtn.interactable = false;
        }

    }
}
