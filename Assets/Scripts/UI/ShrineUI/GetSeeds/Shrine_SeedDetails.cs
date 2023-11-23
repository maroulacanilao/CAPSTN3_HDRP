using System;
using Items.Inventory;
using Items;
using TMPro;
using UI.ShrineUI;
using UnityEngine;
using UnityEngine.UI;
using UI;

namespace UI.ShrineUI.GetSeeds
{
    public class Shrine_SeedDetails : ItemDetailsPanel
    {
        [Header("Shrine")]
        [SerializeField] private Shrine_GetSeeds shrine;
        [SerializeField] private PlayerInventory inventory;
        [SerializeField] private TextMeshProUGUI errorTxt;
        [SerializeField] private Button offerBtn;
        [SerializeField] private ShrineRequirementItem requirementItem;

        public ShrineSeedItem CurrentSeedItem { get; private set; }

        [Header("Shrine")]
        [SerializeField] private Image seedIcon;

        protected void Awake()
        {
            DisplayNull();
            offerBtn.onClick.AddListener(OfferSeed);
        }

        public void DisplaySeed(ShrineSeedItem seedItem_)
        {
            if (seedItem_ == null)
            {
                DisplayNull();
                return;
            }
            CurrentSeedItem = seedItem_;
            DisplayItem(seedItem_.SeedData.GetItem());

            // if (seedItem_.OfferRequirement.consumableData == null) throw new Exception("No consumable data found!");

            // if (requirementItem != null) requirementItem.Set(seedItem_.OfferRequirement);

            if (seedIcon != null) seedIcon.sprite = seedItem_.SeedData.Icon;

            if (requirementItem != null) requirementItem.gameObject.SetActive(true);

            var _canPurchase = shrine.CanPurchase(CurrentSeedItem.SeedData, out var _error);

            if (_canPurchase)
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

        private void OfferSeed()
        {
            if (CurrentSeedItem == null) return;
            if (CurrentSeedItem.SeedData == null) return;
            shrine.PurchaseSeed(CurrentSeedItem.SeedData);
        }

        public override void DisplayNull()
        {
            base.DisplayNull();
            if (requirementItem != null) requirementItem.gameObject.SetActive(false);
            offerBtn.interactable = false;
        }
    }
}