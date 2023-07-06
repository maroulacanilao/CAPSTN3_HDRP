using System;
using Items;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.ShippingBinUI
{
    public class ShippingConfirmMenu : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI messageText;
        [SerializeField] private Button confirmButton, cancelButton, addButton, subtractButton;
        [SerializeField] private TMP_InputField countInput;
        
        [SerializeField] [NaughtyAttributes.ResizableTextArea] private string defaultAddConfirmMessage;
        [SerializeField] [NaughtyAttributes.ResizableTextArea] private string stackAddConfirmMessage;
        
        [SerializeField] [NaughtyAttributes.ResizableTextArea] private string defaultRemoveConfirmMessage;
        [SerializeField] [NaughtyAttributes.ResizableTextArea] private string stackRemoveConfirmMessage;
        
        private int count;
        private Item item;
        private ShippingMenuItem menuItem;
        private ShippingMenu menu;
        private RectTransform rectTransform;

        public void Initialize(ShippingMenu shippingMenu_)
        {
            menu = shippingMenu_;
            gameObject.SetActive(false);
            confirmButton.onClick.AddListener(OnConfirm);
            cancelButton.onClick.AddListener(OnCancel);
            addButton.onClick.AddListener(OnAdd);
            subtractButton.onClick.AddListener(OnSubtract);
            countInput.onValueChanged.AddListener(OnCountChange);
            
            rectTransform = GetComponent<RectTransform>();
        }
        
        private void OnCountChange(string input_)
        {
            count = int.Parse(input_);
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
            menuItem = null;
            item = null;
        }
        
        private void OnConfirm()
        {
            if(menuItem == null) gameObject.SetActive(false);
            
            if(menuItem.IsInventory) menu.AddToShippingList(menuItem, count);
            else menu.RemoveFromShippingList(menuItem, count);
            
            OnCancel();
        }

        public void DisplayConfirmation(ShippingMenuItem menuItem_)
        {
            menuItem = menuItem_;
            item = menuItem.item;
            
            var _itemName = $"<color=yellow>{item.Data.ItemName}</color>";

            if(item is ItemStackable stackable)
            {
                var _message = menuItem.IsInventory ? 
                    stackAddConfirmMessage.Replace("NAME", _itemName) : 
                    stackRemoveConfirmMessage.Replace("NAME", _itemName);
                
                messageText.text = _message;
                count = stackable.StackCount;
                countInput.gameObject.SetActive(true);
                subtractButton.gameObject.SetActive(true);
                addButton.gameObject.SetActive(true);
                
                countInput.text = count.ToString();
            }
            else
            {
                var _message = menuItem.IsInventory ? 
                    defaultAddConfirmMessage.Replace("NAME", _itemName) : 
                    defaultRemoveConfirmMessage.Replace("NAME", _itemName);
                
                messageText.text = _message;
                countInput.gameObject.SetActive(false);
                addButton.gameObject.SetActive(false);
                subtractButton.gameObject.SetActive(false);
            }
            
            confirmButton.gameObject.SetActive(true);
            cancelButton.gameObject.SetActive(true);
            gameObject.SetActive(true);
            
            rectTransform.position = menuItem.transform.position;
            
            EventSystem.current.SetSelectedGameObject(confirmButton.gameObject);
        }

        private void OnDisable()
        {
            EventSystem.current.SetSelectedGameObject(menuItem.gameObject);
        }

        public void ClampCount()
        {
            switch (item)
            {
                case null:
                    return;
                case ItemStackable stackable:
                    count = Mathf.Clamp(count, 1, stackable.StackCount);
                    countInput.text = count.ToString();
                    break;
            }
        }
    }
}
