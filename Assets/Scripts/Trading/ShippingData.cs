using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Items;
using Items.Inventory;
using Items.ItemData;
using Managers;
using ScriptableObjectData;
using UnityEngine;

namespace Trading
{
    [CreateAssetMenu(menuName = "ScriptableObjects/ShippingBinData", fileName = "ShippingBinData")]
    public class ShippingData : ScriptableObject
    {
        [SerializeField] private PlayerInventory inventory;
        [SerializeField] private CropDataBase cropDataBase;

        public List<Item> shippingList { get; private set; } = new List<Item>();
        private SerializedDictionary<ItemData, ItemStackable> stackableLookup = new SerializedDictionary<ItemData, ItemStackable>();

        public void AddItem(Item item, int count_ = 0)
        {
            if(stackableLookup.TryGetValue(item.Data, out ItemStackable _stackable))
            {
                _stackable.AddStack(count_);
                
                var _origStack = item as ItemStackable;
                _origStack?.RemoveStack(count_);
                if(_origStack != null) inventory.UpdateStackable(_origStack);
                
                return;
            }

            if(item is ItemStackable _itemStackable)
            {
                var _clone = _itemStackable.Clone() as ItemStackable;
                _clone.SetStack(count_);
                
                stackableLookup.Add(item.Data, _clone);
                shippingList.Add(_clone);
                _itemStackable.RemoveStack(count_);
                
                inventory.UpdateStackable(_itemStackable);
            }
            else
            {
                shippingList.Add(item);
                inventory.RemoveItem(item);
            }
        }

        public void ReturnItemToInventoryAt(int index_, int count_ = 0)
        {
            ReturnItemToInventory(shippingList[index_], count_);
        }
        
        public void ReturnItemToInventory(Item item_, int count_ = 0)
        {
            if(stackableLookup.TryGetValue(item_.Data, out ItemStackable _stackable))
            {
                _stackable.RemoveStack(count_);
                
                var _clone = _stackable.Clone() as ItemStackable;
                _clone.SetStack(count_);

                if(_stackable.StackCount == 0)
                {
                    stackableLookup.Remove(item_.Data);
                    shippingList.Remove(_stackable);
                }

                inventory.AddItem(_clone);

                return;
            }
            
            inventory.AddItem(item_);
            shippingList.Remove(item_);
        }

        public void ShipItems()
        {
            var _total = 0;
            
            foreach (var item in shippingList)
            {
                if (item is not ItemStackable _stackable)
                {
                    _total += item.Data.ShippingValue;
                    
                    continue;
                }
                
                _total += _stackable.StackCount * _stackable.Data.ShippingValue;

                if (item.Data is not ConsumableData _consumableData) continue;
                
                cropDataBase.AddHarvest(_consumableData, _stackable.StackCount);
            }
            
            inventory.AddGold(_total);
        }
        
        public int GetTotalShippingValue()
        {
            var _total = 0;
            
            foreach (var item in shippingList)
            {
                if(item is ItemStackable _stackable)
                {
                    _total += _stackable.StackCount * _stackable.Data.ShippingValue;
                }
                else
                {
                    _total += item.Data.ShippingValue;
                }
            }

            return _total;
        }

        public void ClearItems()
        {
            shippingList.Clear();
            stackableLookup.Clear();
        }
    }
}
