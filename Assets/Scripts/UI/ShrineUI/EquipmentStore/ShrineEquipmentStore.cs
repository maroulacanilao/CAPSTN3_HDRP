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
        [SerializeField] ShrineData shrineData;
        [SerializeField] Transform contentParent;
        [SerializeField] private ShrineStoreEquipmentDetails detailPanel;
        [SerializeField] private ShrineEquipmentStoreItem equipmentStoreItemPrefab;
        
        private Dictionary<ItemGear, OfferRequirement> gearOffers => shrineData.GetAllGearOffers();
        private List<ShrineEquipmentStoreItem> equipmentStoreItems = new List<ShrineEquipmentStoreItem>();

        private void OnEnable()
        {
            CreateList();
            ShrineEquipmentStoreItem.OnClickItem.AddListener(OnClickItem);
        }
        
        private void OnDisable()
        {
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
                errorTxt.gameObject.SetActive(false);
            }
            else
            {
                errorTxt.text = "No more gears available";
                errorTxt.gameObject.SetActive(true);
                detailPanel.SetItem(null);
            }
        }
        
        private void OnClickItem(ShrineEquipmentStoreItem buttonItem_)
        {
            detailPanel.SetItem(buttonItem_);
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
                    errorTxt.text = "";
                    break;
                default:
                    errorTxt.text = "";
                    break;
            }
        }
    }
}
