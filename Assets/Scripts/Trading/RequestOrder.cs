using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using ScriptableObjectData;
using BaseCore;
using Items;
using Items.Inventory;
using Items.ItemData;
using UnityEngine;

namespace Trading
{
    [System.Serializable]
    public class RequestOrder
    {
        #region private
        
        [SerializeField] private int orderID;
        [SerializeField] private string orderTitle;
        [SerializeField] private string requesterName;
        [SerializeField] private string orderDescription;
        [SerializeField] private DateTime orderDate;
        [SerializeField] private SerializedDictionary<ItemData,RequestItem> requestedItems;
        [SerializeField] private LootDrop reward;
        
        #endregion
        
        
        #region public

        public int OrderID => orderID;
        public string OrderTitle => orderTitle;
        public string RequesterName => requesterName;
        public string OrderDescription => orderDescription;
        public DateTime OrderDate => orderDate;
        public SerializedDictionary<ItemData, RequestItem> RequestedItems => requestedItems;
        public LootDrop Reward => reward;
        #endregion
        
        
        public RequestOrder(GameDataBase gameDataBase_,RequestOrderTemplate requestOrderTemplate_)
        {
            orderID = (int)DateTime.Now.Ticks;
            orderTitle = requestOrderTemplate_.orderTitle;
            requesterName = "Juan De La Cruz";
            orderDescription = requestOrderTemplate_.orderDescription;
            orderDate = DateTime.Now;
            requestedItems = new SerializedDictionary<ItemData, RequestItem>();
            
            reward = requestOrderTemplate_.rewardTable.GetDrop(gameDataBase_.itemDatabase, gameDataBase_.playerData.LevelData.CurrentLevel);

            SetRequests(requestOrderTemplate_);
        }

        private void SetRequests(RequestOrderTemplate requestOrderTemplate_)
        {

            var _itemRequest = requestOrderTemplate_.GetRequestItems();

            var _totalPrice = 0;

            foreach (var _item in _itemRequest)
            {
                var _requestItem = _item.Clone();

                if (requestedItems.TryGetValue(_requestItem.ItemData, out var _itemFound))
                {
                    _itemFound.AddRequestCount(_requestItem.RequestCount);
                }
                else requestedItems.Add(_requestItem.ItemData, _requestItem);

                _totalPrice += _requestItem.ItemData.SellValue * _requestItem.RequestCount;
            }

            _totalPrice = (int) (_totalPrice * requestOrderTemplate_.sellModifier * 5);
            // reward.moneyDrop.SetAmount(_totalPrice);
            // var _money = 
            // reward.itemsDrop.Add();
        }
    }

    [System.Serializable]
    public class RequestItem
    {
        [SerializeField] private ItemData itemData;
        [SerializeField] private int requestCount;
        // TODO: Remove Attribute
        [SerializeReference] private List<Item> itemsGiven;
        
        public ItemData ItemData => itemData;
        public int RequestCount => requestCount;
        public List<Item> ItemsGiven => itemsGiven;
        public int itemsGivenCount
        {
            get
            {
                if(itemData == null) return 0;
                
                if(itemsGiven.Count == 0) return 0;
                
                if(itemsGiven[0] is ItemStackable _stackable)
                {
                    return _stackable.StackCount;
                }
                
                return itemsGiven.Count;
            }
        }
        
        public int amountDifference => requestCount - itemsGivenCount;
        
        public RequestItem(ItemData itemData_, int requestCount_)
        {
            itemData = itemData_;
            requestCount = requestCount_;
            itemsGiven = new List<Item>();
        }
        
        public RequestItem Clone()
        {
            var _clone = new RequestItem(itemData, requestCount);
            return _clone;
        }
        
        public bool CanAdd(PlayerInventory playerInventory_)
        {
            return playerInventory_.itemsLookup.ContainsKey(itemData);
        }
        
        public bool IsComplete()
        {
            return itemsGiven != null && itemsGiven.Count != 0 && requestCount >= itemsGivenCount;
        }
        
        public void AddItem(Item item_)
        {
            if(item_ == null) return;
            if(item_.Data != itemData) return;
            
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
            if(!itemData.IsStackable) return;

            if (itemsGiven != null && itemsGiven.Count != 0)
            {
                var _stackable = itemsGiven[0] as ItemStackable;
                _stackable?.AddStack(amount_);
                return;
            }
            itemsGiven = new List<Item>();

            ItemStackable _newStackable;
            
            switch (itemData)
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
        
        public void AddRequestCount(int amount_)
        {
            requestCount += amount_;
        }
    }
}