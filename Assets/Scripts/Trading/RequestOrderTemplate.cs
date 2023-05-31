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

        [field: BoxGroup("Guaranteed Requested Items")] [field: SerializeField]
        public List<RequestItem> guaranteedRequestedItems { get; private set; }
        
        [field: BoxGroup("Additional Requested Items")] [field: MinMaxSlider(0, 10)]
        [field: SerializeField] public Vector2Int possibleItemCount { get; private set; }
        
        [field: BoxGroup("Additional Requested Items")] [field: SerializeField] 
        public WeightedDictionary<RequestItem> additionalRequestedItems { get; private set; }

        [field: BoxGroup("Reward")] [field: SerializeField] [field: Range(1, 2)]
        public float sellModifier { get; private set; }
        
        [field: BoxGroup("Reward")] [field: SerializeField] 
        public LootTable rewardTable { get; private set; }
        
        public RequestOrder GetRequestOrder(GameDataBase gameDataBase_)
        {
            var _requestOrder = new RequestOrder(gameDataBase_, this);
            return _requestOrder;
        }
        
        public List<RequestItem> GetGuaranteedRequestItems()
        {
            var _requestItems = new List<RequestItem>();
            foreach (var _requestItem in guaranteedRequestedItems)
            {
                _requestItems.Add(_requestItem);
            }

            return _requestItems;
        }
        
        public List<RequestItem> GetOtherRequestItems()
        {
            var _list = new List<RequestItem>();

            var _additionalClone = additionalRequestedItems.Clone();
            var _count = possibleItemCount.GetRandomInRange();

            for (int i = 0; i < _count; i++)
            {
                var _requestItem = _additionalClone.GetWeightedRandom();
                if(_requestItem.ItemData == null) continue;
                _list.Add(_requestItem);
                
                _additionalClone.RemoveItem(_requestItem);
            }
            return _list;
        }
        
        public List<RequestItem> GetRequestItems()
        {
            var _list = new List<RequestItem>();
            _list.AddRange(GetGuaranteedRequestItems());
            _list.AddRange(GetOtherRequestItems());
            return _list;
        }
    }
}