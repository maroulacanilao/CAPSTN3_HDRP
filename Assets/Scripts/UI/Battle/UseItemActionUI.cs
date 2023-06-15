using System;
using System.Collections.Generic;
using System.Linq;
using CustomEvent;
using CustomHelpers;
using Items;
using Items.Inventory;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UI.Battle
{
    public class UseItemActionUI : MonoBehaviour
    {
        [SerializeField] private PlayerInventory inventory;
    
        [SerializeField] private ItemActionBtn itemBtnPrefab;
    
        [SerializeField] private List<ItemActionBtn> itemBtnList;
        [SerializeField] private ItemDetailsPanel itemDetailsPanel;
        
        public static readonly Evt<ItemActionBtn> OnItemBtnSelect = new Evt<ItemActionBtn>();
        
        public static ItemActionBtn CurrentItemBtn { get; set; }
        private BattleActionUI battleActionUI;
        
        public void Initialize(BattleActionUI battleActionUI_)
        {
            CreateItemButtons();
            battleActionUI = battleActionUI_;
            itemDetailsPanel.Initialize();
        }
        
        public void OnDisable()
        {
            InventoryEvents.OnUpdateInventory.RemoveListener(UpdateInventory);
        }

        private void OnEnable()
        {
            InventoryEvents.OnUpdateInventory.AddListener(UpdateInventory);
            if(itemBtnList.Count == 0) CreateItemButtons();
            if(itemBtnList.Count == 0) return;
            
            OnItemBtnSelect.Invoke(itemBtnList[0]);
        }

        public void UpdateInventory(PlayerInventory inventory_)
        {
            if (this.gameObject.IsEmptyOrDestroyed())
            {
                InventoryEvents.OnUpdateInventory.RemoveListener(UpdateInventory);
                return;
            }
            PurgeNulls();
            
            foreach (var _itemBtn in itemBtnList)
            {
                if(_itemBtn.item != null) continue;
                if(inventory.itemsLookup.ContainsKey(_itemBtn.item.Data)) continue;
                
                itemBtnList.Remove(_itemBtn);
                Object.Destroy(_itemBtn);
            }

            itemDetailsPanel.gameObject.SetActive(CurrentItemBtn != null);
            
            if (!itemDetailsPanel.gameObject.activeSelf) return;
            
            itemDetailsPanel.DisplayItemDetails(CurrentItemBtn);
        }

        private void CreateItemButtons()
        {
            itemBtnList = new List<ItemActionBtn>();
            var _itemsList = inventory.ItemStorage.Values.Where(item => item.ItemType == ItemType.Consumable).ToList();
        
            foreach (var _item in _itemsList)
            {
                var _btn = Object.Instantiate(itemBtnPrefab, transform);
                _btn.Initialize(battleActionUI,_item);
                itemBtnList.Add(_btn);
            }
        }

        private void PurgeNulls()
        {
            if(itemBtnList.Count == 0) return;
            foreach (var _itemBtn in itemBtnList)
            {
                if(_itemBtn == null) itemBtnList.Remove(_itemBtn);
                
                if (_itemBtn.item != null) continue;
                itemBtnList.Remove(_itemBtn);
                Object.Destroy(_itemBtn);
            }
        }
        
        public bool HasItems()
        {
            return itemBtnList.Count > 0;
        }
    }
}
