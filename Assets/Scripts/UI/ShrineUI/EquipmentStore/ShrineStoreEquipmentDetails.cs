using System;
using Items;
using Shop;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.ShrineUI.EquipmentStore
{
    public class ShrineStoreEquipmentDetails : ItemDetailsPanel
    {
        [SerializeField] private ShrineEquipmentStore shrineEquipmentStore;
        [SerializeField] private TextMeshProUGUI errorTXt;
        [SerializeField] private Button buyBtn;
        [SerializeField] private ShrineRequirementItem requirementItem;
        
        public ShrineEquipmentStoreItem currentEquipmentStoreItem { get; private set; }

        private void Awake()
        {
            buyBtn.onClick.AddListener(() =>
            {
                shrineEquipmentStore.Buy(currentEquipmentStoreItem);
            });
        }

        public void SetItem(ShrineEquipmentStoreItem item_)
        {
            currentEquipmentStoreItem = item_;
            DisplayItem(item_.gearItem);
            
            if (item_ != null)
            {
                requirementItem.Set(item_.offerRequirement);
                requirementItem.gameObject.SetActive(true);
            }
            else
            {
                requirementItem.gameObject.SetActive(false);
                buyBtn.interactable = false;
                return;
            }
            
            var _result = shrineEquipmentStore.CanBuyItem(item_);
            buyBtn.interactable = _result == OfferingResult.Success;

            switch (_result)
            {
                case OfferingResult.NoOpenSlot:
                    errorTXt.text = "No open slot";
                    break;
                case OfferingResult.NoRequirementInInventoryStorage:
                case OfferingResult.NotEnoughRequirementsCount:
                    errorTXt.text = "Not enough requirement";
                    break;
                default:
                    errorTXt.text = "";
                    break;
            }
        }
    }
}