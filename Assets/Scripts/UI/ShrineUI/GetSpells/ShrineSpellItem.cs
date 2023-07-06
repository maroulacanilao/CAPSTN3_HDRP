using System;
using CustomEvent;
using Shop;
using Spells.Base;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.ShrineUI.GetSpells
{
    public class ShrineSpellItem : SelectableMenuButton
    {
        [SerializeField] private TextMeshProUGUI spellNameTxt;
        [SerializeField] private Image requirementIcon;
        [SerializeField] private TextMeshProUGUI requirementTxt;
        
        public static readonly Evt<ShrineSpellItem> OnClickSpell = new Evt<ShrineSpellItem>();

        public SpellData spellData { get; private set; }
        public OfferRequirement offerRequirement { get; private set; }

        private void Awake()
        {
            button.onClick.AddListener(() =>
            {
                OnClickSpell.Invoke(this);
            });
        }

        public void Set(SpellData data_, OfferRequirement requirement_)
        {
            spellData = data_;
            offerRequirement = requirement_;
            
            spellNameTxt.text = spellData.spellName;

            requirementIcon.sprite = offerRequirement.consumableData.Icon;
            requirementTxt.text = $"x{offerRequirement.count}";
        }
    }
}
