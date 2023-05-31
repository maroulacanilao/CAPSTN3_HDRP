using System;
using UnityEngine;

namespace UI.InventoryMenu
{
    public class InventorySwapHandler : MonoBehaviour
    {
        [SerializeField]  private InventoryMenu inventoryMenu;
        
        private Item_MenuItem selectedItem;
        private Item_MenuItem swappingItem;
        private void Reset()
        {
            inventoryMenu = GameObject.FindObjectOfType<InventoryMenu>();
        }

        private void Awake()
        {
            InventoryMenu.OnInventoryItemSelect.AddListener(ItemSelect);
        }

        private void OnEnable()
        {
            InputUIManager.OnSwap.AddListener(OnSwap);
        }

        private void OnDisable()
        {
            InputUIManager.OnSwap.RemoveListener(OnSwap);
        }

        private void ItemSelect(SelectableMenuButton inventoryItem_)
        {
            inventoryItem_.TryGetComponent(out selectedItem);
        }
        
        private void OnSwap()
        {
            if (selectedItem == null) return;
            if(swappingItem == null) swappingItem = selectedItem;
            else
            {
                
            }
        }

        private void SwapItems()
        {
            
        }
    }
}