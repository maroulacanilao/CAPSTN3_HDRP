using System;
using Items;
using Items.Inventory;
using UI.ShrineUI;
using UnityEngine;

namespace UI.ShippingBinUI
{
    public abstract class ShrineInventoryStorageMenu : ShrineMenu
    {
        [SerializeField] private PlayerInventory playerInventory;
        [SerializeField] private ShrineInventoryItem[] storageItemMenus;

        protected override void OnEnable()
        {
            base.OnEnable();
            SetStorageItems();
            ShrineInventoryItem.OnItemClicked.AddListener(ItemClicked);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            ShrineInventoryItem.OnItemClicked.AddListener(ItemClicked);
        }

        public virtual void SetStorageItems()
        {
            for (int i = 0; i < storageItemMenus.Length; i++)
            {
                var _item = playerInventory.GetItemInStorage(i);
                storageItemMenus[i].SetItem(_item, CanItemInteract(_item));
            }
        }

        public abstract bool CanItemInteract(Item item_);
        
        public abstract void ItemClicked(ShrineInventoryItem item_);
    }
}