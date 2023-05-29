using System;
using System.Collections.Generic;
using System.Linq;
using BaseCore;
using CustomEvent;
using CustomHelpers;
using Items.Inventory;
using Items.ItemData;
using Managers;
using ScriptableObjectData;
using UnityEngine;

namespace Trading
{
    public class RequestBoard : Singleton<RequestBoard>
    {
        [SerializeField] private GameDataBase gameDataBase;
        [SerializeField] private int tickRate = 6; // in game minutes
        
        [Range(0,1)] [SerializeField] 
        private float requestBaseChance = 0.5f;

        private PlayerInventory inventory;
        
        public List<RequestOrder> requestOrders { get; private set; }
        
        public static readonly Evt<RequestOrder> OnRequestOrderAdded = new Evt<RequestOrder>();
        public static readonly Evt<RequestOrder> OnRequestOrderCompleted = new Evt<RequestOrder>();

        protected override void Awake()
        {
            base.Awake();
            // TimeManager.OnMinuteTick.AddListener(MinuteTick);
        }

        private void Start()
        {
            inventory = gameDataBase.playerInventory;
            requestOrders = new List<RequestOrder>();
        }

        private void OnDestroy()
        {
            // TimeManager.OnMinuteTick.RemoveListener(MinuteTick);
        }
        
        private void MinuteTick()
        {
            var _willUpdate = TimeManager.CurrentMinute % tickRate == 0;
            if(!_willUpdate) return;
            
            var _willReceive = CustomHelpers.RandomHelper.RandomBool(GetOrderChance());
            
            if(!_willReceive) return;
        
            AddRandomOrder();
        }
        
        [NaughtyAttributes.Button("Add Random Order")]
        public void AddRandomOrder()
        {
            var _newOrder = GetRandomOrder();
            
            if(_newOrder == null) return;
            
            PurgeNulls();
            requestOrders.Add(_newOrder);
            OnRequestOrderAdded.Invoke(_newOrder);
            
            Debug.Log($"Added new order: {_newOrder.OrderTitle}");
            foreach (var _requestItems in _newOrder.RequestItems.Values)
            {
                Debug.Log(_requestItems.itemNeededData.ItemName);
            }
        }
        
        public void CompleteOrder(RequestOrder requestOrder_)
        {
            if(!requestOrders.Remove(requestOrder_)) return;
        
            //TODO: get rewards
            
            OnRequestOrderCompleted.Invoke(requestOrder_);
            PurgeNulls();
            
        }
        
        
        #region Private Methods
        
        private RequestOrder GetRandomOrder()
        {
            var _level = gameDataBase.playerData.playerLevelData.CurrentLevel;
            
            return gameDataBase
                .requestOrderTemplates
                .Where(r => r.minLevel <= _level)
                .ToArray()
                .GetRandomItem()
                .GetRequestOrder(gameDataBase);
        }
        
        private void PurgeNulls()
        {
            if(requestOrders.Count == 0) return;
            
            requestOrders?.RemoveAll(r => r == null);
        }
        
        private float GetOrderChance()
        {
            //TODO: get chance based on level
            return requestBaseChance;
        }
        
        #endregion
    }
}
