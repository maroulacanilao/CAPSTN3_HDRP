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
        [SerializeField] private TextMeshProUGUI errorTxt;
        [SerializeField] private Button buyBtn;
        [SerializeField] private ShrineRequirementItem requirementItem;

        [SerializeField] private GameObject requirementPanel;
        
        public ShrineEquipmentStoreItem currentEquipmentStoreItem { get; private set; }

        private void Awake()
        {
            buyBtn.onClick.AddListener(() =>
            {
                shrineEquipmentStore.Buy(currentEquipmentStoreItem);
            });
            DisplayNull();
        }

        public void SetItem(ShrineEquipmentStoreItem item_)
        {
            currentEquipmentStoreItem = item_;
            if (item_ != null)
            {
                DisplayItem(item_.gearItem);
            }
            
            if (item_ != null)
            {
                requirementItem.Set(item_.offerRequirement);
                requirementItem.gameObject.SetActive(true);
                requirementPanel.gameObject.SetActive(true);
            }
            else
            {
                requirementItem.gameObject.SetActive(false);
                requirementPanel.gameObject.SetActive(false);
                buyBtn.interactable = false;
                return;
            }
            
            var _result = shrineEquipmentStore.CanBuyItem(item_);
            buyBtn.interactable = _result == OfferingResult.Success;

            errorTxt.gameObject.SetActive(true);
            switch (_result)
            {
                case OfferingResult.NoOpenSlot:
                    errorTxt.text = "No open slot";
                    break;
                case OfferingResult.NoRequirementInInventoryStorage:
                case OfferingResult.NotEnoughRequirementsCount:
                    errorTxt.text = "Not enough requirement";
                    break;
                default:
                    errorTxt.text = "";
                    break;
            }
        }

        public override void DisplayNull()
        {
            base.DisplayNull();
            buyBtn.interactable = false;
            requirementItem.gameObject.SetActive(false);
            requirementPanel.gameObject.SetActive(false);
            descriptionPanel.gameObject.SetActive(false);
            errorTxt.gameObject.SetActive(false);
        }
    }
}