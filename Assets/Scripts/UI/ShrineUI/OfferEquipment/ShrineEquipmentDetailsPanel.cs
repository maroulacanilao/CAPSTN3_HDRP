using System;
using Items;
using UI.ShrineUI.OfferEquipment;
using UnityEngine;
using UnityEngine.UI;

namespace UI.ShrineUI
{
    public class ShrineEquipmentDetailsPanel : ItemDetailsPanel
    {
        [NaughtyAttributes.BoxGroup("Buttons")]
        [SerializeField] private Button OfferButton;
        
        [NaughtyAttributes.BoxGroup("Equipment Menu")]
        [SerializeField] private ShrineOfferEquipment shrineOfferEquipment;

        private void Awake()
        {
            OfferButton.onClick.AddListener(OnClick);
        }

        void OnClick()
        {
            if (currItem == null)
            {
                DisplayNull();
                return;
            }
            
            shrineOfferEquipment.OfferWeapon(currItem);
        }

        public override void DisplayItem(Item item)
        {
            OfferButton.interactable = (item != null);
            base.DisplayItem(item);
        }
    }
}