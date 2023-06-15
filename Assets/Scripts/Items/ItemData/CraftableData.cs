using System;
using AYellowpaper.SerializedCollections;
using Items.Inventory;
using UnityEngine;

namespace Items.ItemData
{
    public class CraftableData : ItemData
    {
        [field: SerializeField] public SerializedDictionary<ItemData, int> RequiredItems { get; private set; }
        [field: SerializeField] public GameObject CraftedItem { get; private set; }
        
        protected override void OnValidate()
        {
            base.OnValidate();
            IsStackable = true;
            ItemType = ItemType.Craftable;
        }
        
        
        public bool CanCraft(PlayerInventory inventory_)
        {
            foreach (var _requiredItem in RequiredItems)
            {
                var _itemData = _requiredItem.Key;
                if(!inventory_.itemsLookup.TryGetValue(_itemData, out var _itemsList)) return false;
                if(_itemsList == null || _itemsList.Count == 0) return false;
                
                if (_itemData.IsStackable)
                {
                    if(_itemsList[0].StackCount >= _requiredItem.Value) continue;
                    else return false;
                }
                
                if(_requiredItem.Value <= _itemsList.Count) continue;
                else return false;
            }
            return true;
        }
    }
}
