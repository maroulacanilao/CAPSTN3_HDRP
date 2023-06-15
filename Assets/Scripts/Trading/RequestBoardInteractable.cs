using BaseCore;
using CustomEvent;
using JetBrains.Annotations;
using UnityEngine;

namespace Trading
{
    public class RequestBoardInteractable : InteractableObject
    {
        [SerializeField] private RequestBoardData requestBoardData;
        [SerializeField] private GameObject interactIcon;
        [SerializeField] private GameObject alertIcon;

        public static readonly Evt OnOpenRequestBoard = new Evt();
        
        protected override void OnEnable()
        {
            base.OnEnable();
            RequestBoardData.OnRequestOrderAdded.AddListener(OnNewOrderAdded);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            RequestBoardData.OnRequestOrderAdded.RemoveListener(OnNewOrderAdded);
        }
        
        protected override void Interact()
        {
            if(requestBoardData.RequestOrders == null || requestBoardData.RequestOrders.Count == 0) return;
            OnOpenRequestBoard.Invoke();
        }
        
        protected override void Enter()
        {
            if(requestBoardData.RequestOrders == null || requestBoardData.RequestOrders.Count == 0) return;
            interactIcon.SetActive(true);
        }
        
        protected override void Exit()
        {
            interactIcon.SetActive(false);
        }
        
        void OnNewOrderAdded(RequestOrder requestOrder_)
        {
            alertIcon.SetActive(true);
        }
        
        
    }
}
