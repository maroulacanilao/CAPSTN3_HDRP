using System.Collections.Generic;
using System.Linq;
using CustomEvent;
using CustomHelpers;
using NaughtyAttributes;
using ScriptableObjectData;
using ScriptableObjectData.CharacterData;
using UnityEngine;

namespace Trading
{
    public class RequestBoardData : ScriptableObject 
    {
        [field: SerializeField] public List<RequestOrderTemplate> requestOrderTemplates { get; private set; }
        [field: SerializeField] public AnimationCurve orderChanceCurve { get; private set; }

        private List<RequestOrder> requestOrders;
        private List<RequestOrder> acceptedOrders;
        
        private GameDataBase gameDataBase;
        private PlayerData playerData;
        private int playerLvl => playerData.LevelData.CurrentLevel;

        public List<RequestOrder> RequestOrders
        {
            get
            {
                PurgeNulls();
                return requestOrders;
            }
        }
        public List<RequestOrder> AcceptedOrder
        {
            get
            {
                PurgeNulls();
                return acceptedOrders;
            }
        }

        public static readonly Evt<RequestOrder> OnRequestOrderAdded = new Evt<RequestOrder>();
        public static readonly Evt<RequestOrder> OnRequestOrderCompleted = new Evt<RequestOrder>();

        public void Initialize(GameDataBase gameDataBase_)
        {
            gameDataBase = gameDataBase_;
            playerData = gameDataBase_.playerData;
            requestOrders = new List<RequestOrder>();
            acceptedOrders = new List<RequestOrder>();
        }

        public float GetOrderChance(float gameTimeScaled_)
        {
            return orderChanceCurve.Evaluate(gameTimeScaled_) * (1 + playerLvl * 0.1f);
        }
        
        private void PurgeNulls()
        {
            if (requestOrders != null && requestOrders.Count != 0)
            {
                requestOrders?.RemoveAll(r => r == null);
            }
            
            if (acceptedOrders != null && acceptedOrders.Count != 0)
            {
                acceptedOrders?.RemoveAll(r => r == null);
            }
        }
        
        [NaughtyAttributes.Button("Receive Random Order")]
        public void ReceiveRandomOrder()
        {
            var _newOrder = GetRandomOrder();
            
            if(_newOrder == null) return;
            
            PurgeNulls();
            requestOrders.Add(_newOrder);
            OnRequestOrderAdded.Invoke(_newOrder);
        }
        
        public void AcceptOrder(RequestOrder requestOrder_)
        {
            if(!requestOrders.Remove(requestOrder_)) return;
            
            PurgeNulls();
            acceptedOrders.Add(requestOrder_);
        }
        
        public void RemoveOrder(RequestOrder requestOrder_)
        {
            if(!requestOrders.Remove(requestOrder_)) return;
            
            PurgeNulls();
        }
        
        public void CompleteOrder(RequestOrder requestOrder_)
        {
            if(!acceptedOrders.Remove(requestOrder_)) return;
            
            PurgeNulls();
            OnRequestOrderCompleted.Invoke(requestOrder_);
        }

        private RequestOrder GetRandomOrder()
        {
            return requestOrderTemplates
                .Where(r => r.minLevel <= playerLvl)
                .ToArray()
                .GetRandomItem()
                .GetRequestOrder(gameDataBase);
        }

#if UNITY_EDITOR

        [Button(" Get All Request Template")]
        private void GetAllRequestTemplate()
        {
            requestOrderTemplates = new List<RequestOrderTemplate>();
            var _requestTemplates = Resources.LoadAll<RequestOrderTemplate>("Data/RequestTemplates");
            foreach (var _data in _requestTemplates)
            {
                requestOrderTemplates.Add(_data);
            }
            UnityEditor.EditorUtility.SetDirty(this);
        }
        
#endif
    }
}
