using System;
using CustomEvent;
using Shop;
using Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Items.ItemData;

namespace UI.ShrineUI.GetSeeds
{
    public class ShrineSeedItem : SelectableMenuButton
    {
        [SerializeField] private TextMeshProUGUI spellNameTxt;
        [SerializeField] private Image requirementIcon;
        [SerializeField] private TextMeshProUGUI requirementTxt;

        [SerializeField] private Image seedIcon;

        public static readonly Evt<ShrineSeedItem> OnClickSeed = new Evt<ShrineSeedItem>();

        public SeedData SeedData { get; private set; }
        public OfferRequirement OfferRequirement { get; private set; }

        protected new void Awake()
        {
            button.onClick.AddListener(() =>
            {
                OnClickSeed.Invoke(this);
            });
        }

        public void Set(SeedData data_, OfferRequirement requirement_)
        {
            SeedData = data_;
            OfferRequirement = requirement_;

            spellNameTxt.text = SeedData.name;

            requirementIcon.sprite = OfferRequirement.consumableData.Icon;
            requirementTxt.text = $"x{OfferRequirement.count}";

            if (seedIcon != null) seedIcon.sprite = SeedData.Icon;
        }

    }
}