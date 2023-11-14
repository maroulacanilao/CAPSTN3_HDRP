using System;
using CustomEvent;
using Items;
using Shop;
using Spells.Base;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.ShrineUI.EquipmentStore
{
    public class ShrineEquipmentStoreItem : SelectableMenuButton
    {
        [SerializeField] private TextMeshProUGUI nameTxt;
        [SerializeField] private Image requirementIcon;
        [SerializeField] private TextMeshProUGUI requirementTxt;

        [SerializeField] private Image gearIcon;

        public ItemGear gearItem { get; private set; }
        public OfferRequirement offerRequirement { get; private set; }
        
        public static readonly Evt<ShrineEquipmentStoreItem> OnClickItem = new Evt<ShrineEquipmentStoreItem>();

        protected override void Awake()
        {
            base.Awake();
            button.onClick.AddListener(() =>
            {
                OnClickItem.Invoke(this);
            });
        }

        public void SetEquipment(ItemGear gearItem_, OfferRequirement requirement_)
        {
            gearItem = gearItem_;
            offerRequirement = requirement_;
            
            nameTxt.text = gearItem.Data.ItemName;

            requirementIcon.sprite = offerRequirement.consumableData.Icon;
            requirementTxt.text = $"x{offerRequirement.count}";

            if (gearIcon != null) gearIcon.sprite = gearItem.Data.Icon;
        }
    }
}