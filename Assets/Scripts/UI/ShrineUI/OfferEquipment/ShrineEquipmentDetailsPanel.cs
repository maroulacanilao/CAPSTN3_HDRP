﻿using System;
using Items;
using Items.ItemData;
using Shop;
using TMPro;
using UI.ShrineUI.OfferEquipment;
using UnityEngine;
using UnityEngine.UI;

namespace UI.ShrineUI
{
    public class ShrineEquipmentDetailsPanel : ItemDetailsPanel
    {
        [SerializeField] private ShrineData shrineData;
        
        [NaughtyAttributes.BoxGroup("Buttons")]
        [SerializeField] private Button OfferButton;

        [NaughtyAttributes.BoxGroup("Equipment Menu")]
        [SerializeField] private ShrineGearToSeed shrineGearToSeed;

        [SerializeField] private GameObject seedPanel;
        [SerializeField] private Image seedIcon;
        [SerializeField] private TextMeshProUGUI seedNameTxt, seedCountTxt;

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
            
            shrineGearToSeed.OfferWeapon(currItem);
        }

        public override void DisplayItem(Item item)
        {
            OfferButton.interactable = (item != null);
            base.DisplayItem(item);
            DisplayConversion(item);
            descriptionPanel.gameObject.SetActive(false);
        }

        public override void DisplayNull()
        {
            base.DisplayNull();
            descriptionPanel.gameObject.SetActive(false);
            OfferButton.interactable = false;
        }

        private void DisplayConversion(Item item)
        {
            if(item == null) return;
            if(item.Data == null) return;
            if(item.Data is not GearData _gearData) return;
            
            var _seedData = shrineData.GetGearToSeedConversion(_gearData);
            if(_seedData == null) return;
            
            seedIcon.sprite = _seedData.Icon;
            seedNameTxt.text = _seedData.ItemName;
            seedCountTxt.text = $"x{item.Level}";
            seedPanel.gameObject.SetActive(_seedData != null);
        }
    }
}