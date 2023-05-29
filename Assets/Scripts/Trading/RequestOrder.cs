using System;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using BaseCore;
using Items;
using Items.Inventory;
using Items.ItemData;
using Managers;
using ScriptableObjectData;
using UnityEngine;

namespace Trading
{
    [System.Serializable]
    public class RequestItem
    {
        public RequestItem(ItemData itemData_, int amountNeeded_)
        {
            itemNeededData = itemData_;
            amountNeeded = amountNeeded_;
            itemsGiven = new List<Item>();
        }
        
        public ItemData itemNeededData;
        public int amountNeeded;

        public List<Item> itemsGiven;
        public int amountGiven
        {
            get
            {
                if(itemsGiven.Count == 0) return 0;
                
                if(itemsGiven[0].IsStackable) return itemsGiven.Count;
                
                return itemsGiven.Count;
            }
        }
        
        public int amountDifference => amountNeeded - amountGiven;

        public bool CanAdd(PlayerInventory playerInventory_)
        {
            return playerInventory_.itemsLookup.ContainsKey(itemNeededData);
        }
        
        public bool IsComplete()
        {
            return itemsGiven != null && itemsGiven.Count != 0 && amountGiven >= amountNeeded;
        }
        
        public void AddItem(Item item_)
        {
            if(item_ == null) return;
            if(item_.Data != itemNeededData) return;
            
            if (item_.IsStackable)
            {
                AddToStack(item_.StackCount);
                return;
            }
            
            if(itemsGiven == null || itemsGiven.Count == 0)
            {
                itemsGiven = new List<Item> { item_ };
            }
            else
            {
                itemsGiven.Add(item_);
            }
        }
        
        public void AddToStack(int amount_)
        {
            if(!itemNeededData.IsStackable) return;

            if (itemsGiven != null && itemsGiven.Count != 0)
            {
                var _stackable = itemsGiven[0] as ItemStackable;
                _stackable?.AddStack(amount_);
                return;
            }
            itemsGiven = new List<Item>();

            ItemStackable _newStackable;
            
            switch (itemNeededData)
            {
                case SeedData _seedData:
                {
                    _newStackable = new ItemSeed(_seedData, amount_);
                    itemsGiven.Add(_newStackable);
                    break;
                }

                case ConsumableData _consumableData:
                {
                    _newStackable = new ItemConsumable(_consumableData, amount_);
                    itemsGiven.Add(_newStackable);
                    break;
                }

                default: return;
            }
        }
    }

    [System.Serializable]
    public class RequestOrder
    {
        
        #region Private Properties
        
        private int orderID;
        private string orderTitle;
        private string orderDescription;
        private string requesterName;
        private SerializedDictionary<ItemData, RequestItem> requestedItemsDictionary;
        private LootDrop rewardTable;
        private DateTime timeReceived;

        #endregion
        
        #region Getters

        public int OrderID => orderID;
        public string OrderTitle => orderTitle;
        public string OrderDescription => orderDescription;
        public string RequesterName => requesterName;
        public LootDrop RewardTable => rewardTable;
        public SerializedDictionary<ItemData, RequestItem> RequestItems => requestedItemsDictionary;
        public DateTime TimeReceived => timeReceived;

        #endregion
        
        public RequestOrder(GameDataBase gameDataBase_, RequestOrderTemplate requestOrderTemplate_)
        {
            timeReceived = TimeManager.DateTime;
            orderID = requestOrderTemplate_.GetInstanceID() + (int)Time.time;
            orderTitle = requestOrderTemplate_.orderTitle;
            orderDescription = requestOrderTemplate_.orderDescription;
            
            // TODO: Add Name Generator
            requesterName = "Juan De La Cruz";
            
            requestedItemsDictionary = new SerializedDictionary<ItemData, RequestItem>();
            
            var _requestedItemsList = requestOrderTemplate_.GetRequestedItems();
            var _totalAmount = 0;
            
            foreach (var _item in _requestedItemsList)
            {
                requestedItemsDictionary.Add(_item.itemNeededData, _item);
                _totalAmount += _item.itemNeededData.SellValue;
            }

            // TODO: Add more modifiers
            var _modifiers = requestOrderTemplate_.sellModifier;
            _totalAmount = Mathf.RoundToInt(_totalAmount * _modifiers);
            
            rewardTable = requestOrderTemplate_.rewardTable.GetDrop(gameDataBase_.itemDatabase);
            
            rewardTable.moneyDrop.SetAmount(_totalAmount);
        }
        
        public bool CanBeCompleted(PlayerInventory playerInventory_)
        {
            return requestedItemsDictionary.Values.All(requestItem_ => requestItem_.CanAdd(playerInventory_));
        }
        
        public bool IsComplete()
        {
            return requestedItemsDictionary.Values.All(requestItem_ => requestItem_.IsComplete());
        }
        
        public List<Item> PossibleItemsInInventory(PlayerInventory playerInventory_)
        {
            var _possibleItems = new List<Item>();
            
            foreach (var _requestItem in requestedItemsDictionary.Values)
            {
                if(!_requestItem.CanAdd(playerInventory_)) continue;
                
                var _items = playerInventory_.itemsLookup[_requestItem.itemNeededData];
                _possibleItems.AddRange(_items);
            }

            return _possibleItems;
        }
    }
}