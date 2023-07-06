using System;
using System.Collections.Generic;
using System.Linq;
using BaseCore;
using CustomEvent;
using CustomHelpers;
using Items.Inventory;
using Items.ItemData;
using Managers;
using NaughtyAttributes;
using ScriptableObjectData;
using UnityEngine;

namespace Trading
{
    public class RequestBoard : Singleton<RequestBoard>
    {
        [field: SerializeField] public RequestBoardData RequestBoardData { get; private set; }
        [SerializeField] private int tickRate = 6; // in game minutes

        private PlayerInventory inventory;

        protected override void Awake()
        {
            base.Awake();
            // TimeManager.OnMinuteTick.AddListener(MinuteTick);
        }

        private void OnDestroy()
        {
            // TimeManager.OnMinuteTick.RemoveListener(MinuteTick);
        }
        
        private void MinuteTick()
        {
            var _willUpdate = TimeManager.CurrentMinute % tickRate == 0;
            if(!_willUpdate) return;
            
            var _willReceive = CustomHelpers.RandomHelper.RandomBool(RequestBoardData.GetOrderChance(TimeManager.GameTime));
            
            if(!_willReceive) return;
            
            RequestBoardData.ReceiveRandomOrder();
        }
    }
}
