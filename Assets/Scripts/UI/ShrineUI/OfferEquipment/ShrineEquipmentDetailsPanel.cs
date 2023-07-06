using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.ShrineUI
{
    public class ShrineEquipmentDetailsPanel : ItemDetailsPanel
    {
        [NaughtyAttributes.BoxGroup("Buttons")]
        [SerializeField] private Button OfferButton;
        
        [NaughtyAttributes.BoxGroup("Equipment Menu")]
        [SerializeField] private Shrine_Equipment shrineEquipment;

        private void Awake()
        {
            OfferButton.onClick.AddListener(OnClick);
        }

        void OnClick()
        {
            if (currItem == null)
            {
                Debug.LogError("No item selected");
                return;
            }
            
            shrineEquipment.OfferWeapon(currItem);
        }
    }
}