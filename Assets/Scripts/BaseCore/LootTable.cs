using System;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using CustomHelpers;
using Items;
using Items.ItemData;
using NaughtyAttributes;
using ScriptableObjectData;
using UnityEngine;

namespace BaseCore
{
    [Serializable]
    public struct LootDrop
    {
        public int expDrop { get; private set; }
    
        [field: SerializeReference]
        public ItemGold moneyDrop { get; private set; }

        [field: SerializeReference]
        public List<Item> itemsDrop { get; private set; }

        public LootDrop(int expDrop_, ItemGold moneyDrop_, List<Item> items_)
        {
            expDrop = expDrop_;
            moneyDrop = moneyDrop_;
            itemsDrop = items_;
        }
    }
    
    [System.Serializable]
    public class LootTable
    {
        [field: MinMaxSlider(0, 1000)] [field: SerializeField] 
        public Vector2Int possibleExperienceDrop { get; private set; }
        
        [MinMaxSlider(0, 1000)] [SerializeField] private Vector2Int possibleMoneyDrop;
        [MinMaxSlider(0, 10)] [SerializeField] private Vector2Int possibleItemCount;
    
        [InfoBox("Guaranteed Items are always dropped.")]
        [SerializedDictionary("Item Data", "Amount")] 
        [SerializeField] private SerializedDictionary<ItemData, int> guaranteedItemDrop;
        
        [InfoBox("Possible Items are randomly dropped.")]
        [SerializeField] private WeightedDictionary<ItemData> itemProbability = new WeightedDictionary<ItemData>();

        private ItemDatabase itemDatabase;
        [ContextMenu("Force Initialize")]
        private void OnEnable()
        {
            itemProbability.ForceInitialize();
        }

        private void OnValidate()
        {
            itemProbability.ForceInitialize();
        }

        public LootDrop GetDrop(ItemDatabase itemDatabase_)
        {
            itemDatabase = itemDatabase_;
            int _exp = possibleExperienceDrop.GetRandomInRange();
            int _moneyAmount = possibleMoneyDrop.GetRandomInRange();

            var _moneyDrop = new ItemGold(itemDatabase_.GoldItemData, _moneyAmount);

            var _itemDrops = GetGuaranteedItems();
            
            _itemDrops.AddRange(GetRandomItems());  

            return new LootDrop(_exp, _moneyDrop, _itemDrops);
        }

        private Item InitializeItem(ItemData itemData_)
        {
            return itemData_.GetItem();
        }

        private List<Item> Organize(List<Item> itemList_)
        {
            var _itemsToRemove = new HashSet<Item>();
        
            foreach (var _item in itemList_)
            {
                if(_item is not ItemStackable) continue;
                if(_itemsToRemove.Contains(_item)) continue;
            
                foreach (var _otherItem in itemList_)
                {
                    if(_otherItem is not ItemStackable) continue;
                    if(_otherItem == _item) continue;
                    if(_item.Data.ItemID != _otherItem.Data.ItemID) continue;
                    if(_itemsToRemove.Contains(_otherItem)) continue;

                    var _consumable = (ItemConsumable)_item;
                    var _otherConsumable = (ItemConsumable) _otherItem;
                
                    _consumable.AddStack(_otherConsumable.StackCount);
                    _itemsToRemove.Add(_otherItem);
                }
            }

            itemList_.RemoveAll(_itemsToRemove.Contains);

            return itemList_;
        }

        private List<Item> GetGuaranteedItems()
        {
            var _items = new List<Item>();
            foreach (var _itemDataPair in guaranteedItemDrop)
            {
                switch (_itemDataPair.Key)
                {
                    case SeedData _seedData:
                    {
                        var _seed = _seedData.GetSeedItem(_itemDataPair.Value);
                        _items.Add(_seed);
                        break;
                    }

                    case ConsumableData _consumableData:
                    {
                        var _consumable = _consumableData.GetConsumableItem(_itemDataPair.Value);
                        _items.Add(_consumable);
                        break;
                    }

                    default:
                    {
                        for (int i = 0; i < _itemDataPair.Value; i++)
                        {
                            _items.Add(_itemDataPair.Key.GetItem());
                        }
                        break;
                    }
                }
            }
            return _items;
        }

        private List<Item> GetRandomItems()
        {
            int _itemsCount = possibleItemCount.GetRandomInRange();
            var _items = new List<Item>();
            
            for (var i = 0; i < _itemsCount; i++)
            {
                var _item = InitializeItem(itemProbability.GetWeightedRandom());
            
                if (_item is not ItemStackable)
                {
                    _items.Add(_item);
                    continue;
                }
            
                var _similarItem = _items.FirstOrDefault(i => i.Data.ItemID == _item.Data.ItemID);
            
                if (_similarItem == null)
                {
                    _items.Add(_item);
                    continue;
                }
            
                var _consumableItem = (ItemConsumable) _item;
                var _similarConsumable = (ItemConsumable) _similarItem;
                _similarConsumable.AddStack(_consumableItem.StackCount);
            }
            
            return _items;
        }
    }
}