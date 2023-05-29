using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using BaseCore;
using CustomHelpers;
using Items;
using Items.ItemData;
using NaughtyAttributes;
using ScriptableObjectData;
using UnityEngine;

namespace Trading
{
    [CreateAssetMenu(fileName = "RequestOrderTemplate", menuName = "ScriptableObjects/RequestOrderTemplate", order = 0)]
    public class RequestOrderTemplate : ScriptableObject
    {
        [field: SerializeField] public int minLevel { get; private set; }
        [field: SerializeField] public string orderTitle { get; private set; }
        [field: SerializeField] public string orderDescription { get; private set; }

        [field: BoxGroup("RequestItems")] [field: SerializeField] [field: SerializedDictionary("items", "Possible Counts")]
        public SerializedDictionary<ItemData, Vector2Int> guaranteedRequestedItems { get; private set; }
        
        [field: BoxGroup("RequestItems")] [field: MinMaxSlider(0, 10)]
        [field: SerializeField] public Vector2Int possibleItemCount { get; private set; }
        
        [field: BoxGroup("RequestItems")] [field: SerializeField] [field: SerializedDictionary("items", "Possible Counts")]
        public SerializedDictionary<ItemData, Vector2Int> otherRequestedItems { get; private set; }

        [field: BoxGroup("Reward")] [field: SerializeField] [field: Range(1, 2)]
        public float sellModifier { get; private set; }
        
        [field: BoxGroup("Reward")] [field: SerializeField] 
        public LootTable rewardTable { get; private set; }
        
        public RequestOrder GetRequestOrder(GameDataBase gameDataBase_)
        {
            var _requestOrder = new RequestOrder(gameDataBase_, this);
            return _requestOrder;
        }

        public List<RequestItem> GetRequestedItems()
        {
            List<RequestItem> _requestItems = new List<RequestItem>(GetGuaranteedRequest());
            _requestItems.AddRange(GetOtherRequest());

            return _requestItems;
        }

        private List<RequestItem> GetGuaranteedRequest()
        {
            var _items = new List<RequestItem>();
            
            foreach (var _itemDataPair in guaranteedRequestedItems)
            {
                var _count = _itemDataPair.Value.GetRandomInRange();
                _items.Add(new RequestItem(_itemDataPair.Key, _count));
            }
            
            return _items;
        }
        
        private List<RequestItem> GetOtherRequest()
        {
            var _items = new List<RequestItem>();
            var _count = possibleItemCount.GetRandomInRange();

            for (int i = 0; i < _count; i++)
            {
                var _itemPair = otherRequestedItems.GetRandomFromCollection();
                var _itemData = _itemPair.Key;
                if(_items.Exists(item_ => item_.itemNeededData == _itemData)) continue;
                
                _items.Add(new RequestItem(_itemData, _itemPair.Value.GetRandomInRange()));
            }

            return _items;
        }
    }
}