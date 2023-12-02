using System;
using System.Collections.Generic;
using Items;
using Shop;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.ShrineUI.EquipmentStore
{
    public class ShrineEquipmentStore : ShrineMenu
    {
        [SerializeField] Transform contentParent;
        [SerializeField] private ShrineStoreEquipmentDetails detailPanel;
        [SerializeField] private ShrineEquipmentStoreItem equipmentStoreItemPrefab;
        [SerializeField] private ConsumableDisplay consumableDisplay;
        
        private Dictionary<ItemGear, OfferRequirement> gearOffers => shrineData.GetAllGearOffers();
        private List<ShrineEquipmentStoreItem> equipmentStoreItems = new List<ShrineEquipmentStoreItem>();

        protected override void OnEnable()
        {
            base.OnEnable();
            CreateList();
            ShrineEquipmentStoreItem.OnClickItem.AddListener(OnClickItem);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            ShrineEquipmentStoreItem.OnClickItem.RemoveListener(OnClickItem);
        }

        private void CreateList()
        {
            ClearList();
            foreach (var pair in gearOffers)
            {
                var _shrineItem = Instantiate(equipmentStoreItemPrefab, contentParent);
                _shrineItem.SetEquipment(pair.Key, pair.Value);
                equipmentStoreItems.Add(_shrineItem);
            }
            
            SelectFirstItem();
            consumableDisplay.UpdateDisplay();
        }
        
        private void ClearList()
        {
            foreach (var item in equipmentStoreItems)
            {
                Destroy(item.gameObject);
            }
            equipmentStoreItems.Clear();
        }
        
        private void SelectFirstItem()
        {
            if (equipmentStoreItems.Count > 0)
            {
                var _btn = equipmentStoreItems[0];
                _btn.SelectButton();
                EventSystem.current.SetSelectedGameObject(_btn.gameObject);
                detailPanel.SetItem(_btn);
                detailPanel.gameObject.SetActive(true);
                errorTxt.gameObject.SetActive(false);
            }
            else
            {
                errorTxt.text = "No more gears available.";
                errorTxt.gameObject.SetActive(true);
                detailPanel.SetItem(null);
                detailPanel.gameObject.SetActive(false);
                detailPanel.DisplayNull();
            }
        }
        
        private void OnClickItem(ShrineEquipmentStoreItem buttonItem_)
        {
            detailPanel.SetItem(buttonItem_);
            detailPanel.gameObject.SetActive(true);
        }

        public OfferingResult CanBuyItem(ShrineEquipmentStoreItem buttonItem_)
        {
            if(buttonItem_ == null)
                return OfferingResult.SomethingWentWrong;
            return shrineData.CanOffer(buttonItem_.offerRequirement);
        }

        public void Buy(ShrineEquipmentStoreItem buttonItem_)
        {
            var _result = shrineData.BuyGear(buttonItem_.gearItem);

            switch (_result)
            {
                case OfferingResult.NoOpenSlot:
                    errorTxt.text = "No open slot";
                    break;
                case OfferingResult.NoRequirementInInventoryStorage:
                case OfferingResult.NotEnoughRequirementsCount:
                    errorTxt.text = "Not enough requirement";
                    break;
                case OfferingResult.Success:
                    CreateList();
                    errorTxt.text = "No more gears available.";
                    break;
                default:
                    errorTxt.text = "No more gears available.";
                    break;
            }
        }
    }
}
