using System;
using Items;
using Items.Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.TabMenu.InventoryMenu
{
    public class DiscardConfirm : MonoBehaviour
    {
        [SerializeField] private PlayerInventory playerInventory;
        
        [SerializeField] private TextMeshProUGUI messageText;
        [SerializeField] private Button confirmButton, cancelButton, addButton, subtractButton;
        [SerializeField] private GameObject countPanel;
        [SerializeField] private TMP_InputField countInput;
        [SerializeField] private InventoryDetailsPanel detailsPanel;

        [SerializeField] [NaughtyAttributes.ResizableTextArea] private string defaultDiscardMessage;
        [SerializeField] [NaughtyAttributes.ResizableTextArea] private string stackDiscardMessage;
        
        Item_MenuItem menuItem;
        private Item item => menuItem.item;
        private ItemStackable stackableItem => item as ItemStackable;
        private bool isStackable => item is ItemStackable;
        private int count;

        private void Awake()
        {
            confirmButton.onClick.AddListener(OnConfirm);
            cancelButton.onClick.AddListener(OnCancel);
            addButton.onClick.AddListener(OnAdd);
            subtractButton.onClick.AddListener(OnSubtract);
            countInput.onValueChanged.AddListener(OnCountChange);
            if (detailsPanel == null) detailsPanel = FindObjectOfType<InventoryDetailsPanel>();
        }

        private void OnDisable()
        {
            menuItem = null;
        }

        public void ShowMenu(Item_MenuItem menuItem_)
        {
            menuItem = menuItem_;
            if(!item.IsDiscardable) return;
            
            gameObject.SetActive(true);
            
            countPanel.SetActive(isStackable);

            messageText.text = isStackable ? 
                stackDiscardMessage.Replace("NAME", item.Data.ItemName) : 
                defaultDiscardMessage.Replace("NAME", item.Data.ItemName);
            
            if(!isStackable) return;
            
            count = 1;
            subtractButton.interactable = count > 1;
            countInput.text = "1";
        }
        
        private void OnCountChange(string value_)
        {
            if(!int.TryParse(value_, out var newCount_)) return;
            count = newCount_;
            ClampCount();
        }
        private void OnSubtract()
        {
            count--;
            ClampCount();
        }
        private void OnAdd()
        {
            count++;
            ClampCount();
        }
        private void OnCancel()
        {
            gameObject.SetActive(false);
        }
        private void OnConfirm()
        {
            Discard();
        }

        private void ClampCount()
        {
            count = Mathf.Clamp(count, 1, stackableItem.StackCount);
            countInput.SetTextWithoutNotify(count.ToString());
            subtractButton.interactable = count > 1;
            addButton.interactable = count < stackableItem.StackCount;
        }

        private void Discard()
        {
            if(menuItem == null) gameObject.SetActive(false);
            if(item == null) gameObject.SetActive(false);
            if(isStackable && stackableItem != null) playerInventory.RemoveStackable(stackableItem, count);
            else playerInventory.RemoveItem(item);
            menuItem.UpdateDisplay();
            detailsPanel.ShowItemDetail(menuItem);
            gameObject.SetActive(false);
            
        }
    }
}
