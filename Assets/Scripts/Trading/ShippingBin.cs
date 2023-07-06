using System;
using BaseCore;
using CustomEvent;
using Managers;
using UnityEngine;

namespace Trading
{
    public class ShippingBin : InteractableObject
    {
        [SerializeField] private ShippingData data;
        
        public static readonly Evt OnShippingBinOpened = new Evt();

        private void Awake()
        {
            TimeManager.OnBeginDay.AddListener(data.ClearItems);
            TimeManager.OnEndDay.AddListener(data.ShipItems);
        }

        private void OnDestroy()
        {
            TimeManager.OnBeginDay.RemoveListener(data.ClearItems);
            TimeManager.OnEndDay.RemoveListener(data.ShipItems);
        }

        protected override void Interact()
        {
            OnShippingBinOpened?.Invoke();
        }
        protected override void Enter()
        {
            
        }
        
        protected override void Exit()
        {
            
        }
    }
}
