using System;
using System.Collections.Generic;
using System.Linq;
using CustomHelpers;
using Items.Inventory;
using TMPro;
using Trading;
using UI.Farming;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace UI.ShippingBinUI
{
    public class ShippingMenu : PlayerMenu
    {
        [SerializeField] private ShippingData data;
        [SerializeField] private PlayerInventory inventory;
        [SerializeField] private List<ShippingMenuItem> inventoryMenuItems;
        [SerializeField] private ShippingMenuItem shippingMenuItemPrefab;
        [SerializeField] private RectTransform shippingItemParent;
        [SerializeField] private TextMeshProUGUI totalValueText;
        [SerializeField] private ShippingConfirmMenu confirmMenu;
        
        private readonly List<ShippingMenuItem> shippingMenuItems = new List<ShippingMenuItem>();
        
        private void Awake()
        {
            confirmMenu.Initialize(this);

            foreach (var inventoryItem in inventoryMenuItems)
            {
                inventoryItem.Initialize(true);
            }
        }

        protected override void OnEnable()
        {
            PlayerMenuManager.OnCloseAllUI.Invoke();
            RevisedPlayerMenuManager.OnCloseAllUIRevised.Invoke();

            base.OnEnable();
            
            ShippingMenuItem.OnItemClicked.AddListener(OnItemClick);

            confirmMenu.gameObject.SetActive(false);
            
            DisplayItems();
            
            Canvas.ForceUpdateCanvases();
            var _firstSelectable = GetFirstSelectable();
            EventSystem.current.SetSelectedGameObject(_firstSelectable != null ? _firstSelectable.gameObject : null);
            Canvas.ForceUpdateCanvases();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            
            ShippingMenuItem.OnItemClicked.RemoveListener(OnItemClick);
        }

        private void OnItemClick(ShippingMenuItem menuItem_)
        {
            if (this.IsEmptyOrDestroyed())
            {
                ShippingMenuItem.OnItemClicked.RemoveListener(OnItemClick);
                return;
            }
            confirmMenu.DisplayConfirmation(menuItem_);
        }

        private void DisplayItems()
        {
            for (var _index = 0; _index < inventoryMenuItems.Count; _index++)
            {
                var inventoryItem = inventoryMenuItems[_index];
                inventoryItem.DisplayItem(inventory.GetItemInStorage(_index));
            }
            
            RemoveShippingItem();

            foreach (var item in data.shippingList)
            {
                if(item == null) continue;
                
                var _shippingItem = Instantiate(shippingMenuItemPrefab, shippingItemParent);
                _shippingItem.Initialize(false);
                _shippingItem.DisplayItem(item);
                
                shippingMenuItems.Add(_shippingItem);
            }
            
            totalValueText.text = $"Total Value: {data.GetTotalShippingValue()}";
        }

        private void RemoveShippingItem()
        {
            if(shippingMenuItems == null || shippingMenuItems.Count == 0) return;
            
            for (int i = shippingMenuItems.Count - 1; i >= 0; i--)
            {
                Destroy(shippingMenuItems[i].gameObject);
            }
            
            shippingMenuItems.Clear();
        }
        
        public void AddToShippingList(ShippingMenuItem menuItem_, int count_ = 0)
        {
            data.AddItem(menuItem_.item, count_);
            DisplayItems();
        }
        
        public void RemoveFromShippingList(ShippingMenuItem menuItem_, int count_ = 0)
        {
            data.ReturnItemToInventory(menuItem_.item, count_);
            DisplayItems();
        }
        
        private ShippingMenuItem GetFirstSelectable()
        {
            return inventoryMenuItems.FirstOrDefault(i => i.button.interactable && i.item != null);
        }
    }
}
