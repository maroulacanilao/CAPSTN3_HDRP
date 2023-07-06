using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using BaseCore;
using CustomEvent;
using Items;
using Items.Inventory;
using Items.ItemData;
using Managers;
using ScriptableObjectData;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Shop
{
    public class ItemShop : InteractableObject
    {
        [SerializeField] private GameDataBase gameDataBase;
        [SerializeField] private SerializedDictionary<ItemType, Vector2Int> itemTypeCount = new SerializedDictionary<ItemType, Vector2Int>();
        private ItemDatabase itemDatabase => gameDataBase.itemDatabase;

        public List<Item> shopItemList { get; private set; }
        
        public static readonly Evt<List<Item>> OnOpenShop = new Evt<List<Item>>();

        private void Awake()
        {
            shopItemList = new List<Item>();
            CreateNewItems();
            TimeManager.OnBeginDay.AddListener(CreateNewItems);
        }

        private void OnDestroy()
        {
            TimeManager.OnBeginDay.RemoveListener(CreateNewItems);
        }

        public void CreateNewItems()
        {
            if(shopItemList == null) shopItemList = new List<Item>();
            else shopItemList.Clear();

            var _level = gameDataBase.playerData.level;
            var _itemDataList = GetRandomItemDataList();

            foreach (var _itemData in _itemDataList)
            {
                if(_itemData == null) continue;
                var _item = _itemData.GetItem(_level);
                shopItemList.Add(_item);
            }
        }
        
        public List<ItemData> GetRandomItemDataList()
        {
            var _list = new List<ItemData>();
            
            foreach (var _pair in itemTypeCount)
            {
                var _min = Mathf.Clamp(_pair.Value.x, 0, 99);
                var _max = Mathf.Clamp(_pair.Value.y + 1, 2, 99);
                var _count = Random.Range(_min, _max);
                
                _list.AddRange(GetRandomItemsFromType(_pair.Key, _count));
            }
            _list.RemoveAll(id => id == null);

            return _list;
        } 

        private List<ItemData> GetRandomItemsFromType(ItemType type_, int count_)
        {
            var _list = new List<ItemData>();

            var _typeData = itemDatabase.ItemDataByType[type_];

            if (_typeData.Count == 0)
            {
                Debug.Log($"No ItemData of type {type_} found in database.");
                return _list;
            }
            
            for (int i = 0; i < count_; i++)
            {
                var _itemData = _typeData[Random.Range(0, _typeData.Count)];
                _list.Add(_itemData);
                _typeData.Remove(_itemData);
                
                if(_typeData.Count == 0) break;
            }
            
            return _list;
        }
        
        protected override void Interact()
        {
            OnOpenShop.Invoke(shopItemList);
        }
        protected override void Enter()
        {
            
        }
        
        protected override void Exit()
        {
            
        }
    }
}
