using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using ScriptableObjectData;
using Shop;
using Items;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Items.ItemData;

namespace UI.ShrineUI.GetSeeds
{
    public class Shrine_GetSeeds : ShrineMenu
    {
        [SerializeField] private Transform contentParent;
        [SerializeField] private Shrine_SeedDetails seedDetails;
        [SerializeField] private ShrineSeedItem seedItemPrefab;
        [SerializeField] private ConsumableDisplay consumableDisplay;
        private ItemDatabase ItemDataBase => gameDataBase.itemDatabase;

        private SerializedDictionary<SeedData, OfferRequirement> seedOfferList;

        private readonly List<ShrineSeedItem> seedItems = new();

        private void Awake()
        {
            seedOfferList = new SerializedDictionary<SeedData, OfferRequirement>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            CreateSeedOffers();
            ShrineSeedItem.OnClickSeed.AddListener(OnClickSeed);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            ShrineSeedItem.OnClickSeed.RemoveListener(OnClickSeed);
        }

        private void CreateSeedOffers()
        {
            seedOfferList.Clear();
            RemoveAllItems();

            //  seedOfferList = new SerializedDictionary<SeedData, OfferRequirement>(ItemDataBase.seedOfferList);

            foreach (var _seedOffer in seedOfferList)
            {
                var _seedItem = Instantiate(seedItemPrefab, contentParent);
                _seedItem.Set(_seedOffer.Key, _seedOffer.Value);
                seedItems.Add(_seedItem);
            }

            SelectFirstSeed();
            consumableDisplay.UpdateDisplay();
        }

        private void OnClickSeed(ShrineSeedItem seedBtn_)
        {
            seedDetails.DisplayItem(seedBtn_.SeedData.GetItem());
        }

        private void SelectFirstSeed()
        {
            if (seedItems.Count > 0)
            {
                var _btn = seedItems[0];
                _btn.SelectButton();
                EventSystem.current.SetSelectedGameObject(_btn.gameObject);
                seedDetails.DisplayItem(_btn.SeedData.GetItem());
                errorTxt.gameObject.SetActive(false);
            }
            else
            {
                errorTxt.text = "No more seeds available.";
                errorTxt.gameObject.SetActive(true);
                seedDetails.DisplayNull();
            }
        }

        public bool CanPurchase(SeedData seedData_, out string message)
        {
            message = string.Empty;
            if (!seedOfferList.TryGetValue(seedData_, out var _offer))
            {
                message = "Seed already purchased.";
                return false;
            }

            if (_offer.consumableData == null) throw new Exception("Offer requirement is null");

            var _requirementData = _offer.consumableData;

            if (!inventory.StackableDictionary.TryGetValue(_requirementData, out var _stackable))
            {
                message = "You don't have the required item.";
                return false;
            }

            if (_stackable.StackCount < _offer.count)
            {
                message = "You don't have enough of the required item.";
                return false;
            }

            return true;
        }

        public void LearnSeed(SeedData seedData_)
        {
            errorTxt.gameObject.SetActive(false);
            if (!CanPurchase(seedData_, out var _message))
            {
                errorTxt.text = _message;
                errorTxt.gameObject.SetActive(true);
                return;
            }

            if (!seedOfferList.TryGetValue(seedData_, out var _requirement)) return;

            if (_requirement.consumableData == null) throw new Exception("Offer requirement is null");

            var _requirementData = _requirement.consumableData;

            if (!inventory.StackableDictionary.TryGetValue(_requirementData, out var _stackable)) return;

            if (_stackable.StackCount >= _requirement.count) inventory.RemoveStackable(_stackable, _requirement.count);

            gameDataBase.playerData.inventory.AddItem(seedData_.GetItem());
            CreateSeedOffers();
        }

        private void RemoveAllItems()
        {
            foreach (var _seedItem in seedItems)
            {
                Destroy(_seedItem.gameObject);
            }

            seedItems.Clear();
        }

    }
}