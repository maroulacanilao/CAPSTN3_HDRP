using System;
using System.Collections.Generic;
using CustomEvent;
using CustomHelpers;
using Trading;
using UI.Farming;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.RequestBoardUI
{
    public class RequestMenu : FarmUI
    {
        [SerializeField] private RequestBoardData requestBoardData;
        [SerializeField] private RequestMenuDetails requestMenuDetails;
        [SerializeField] private RequestMenuItem requestMenuItemPrefab;
        [SerializeField] private Transform requestListParent;
        
        public readonly Evt<RequestMenuItem> OnSelectRequest = new Evt<RequestMenuItem>();
        
        public List<RequestMenuItem> requestMenuItems { get; private set; } = new List<RequestMenuItem>();
        public override void Initialize()
        {
            if (requestBoardData == null) throw new Exception("RequestBoardData is null");
            requestMenuDetails.Initialize(this);
            
            RequestBoardInteractable.OnOpenRequestBoard.AddListener(OpenMenu);
            RequestBoardData.OnRequestOrderAdded.AddListener(OnNewOrderAdded);
        }

        private void OnEnable()
        {
            if (requestMenuItems.Count == 0) return;
            PurgeNulls();
            
            EventSystem.current.SetSelectedGameObject(requestMenuItems[0].gameObject);
            requestMenuItems[0].OnSelect(null);
            requestMenuItems[0].SelectButton();
            requestMenuDetails.ShowDetails(requestMenuItems[0]);
            Canvas.ForceUpdateCanvases();
        }

        public void OnDestroy()
        {
            RequestBoardInteractable.OnOpenRequestBoard.RemoveListener(OpenMenu);
            RequestBoardData.OnRequestOrderAdded.RemoveListener(OnNewOrderAdded);
        }

        public override void OpenMenu()
        {
            if(requestBoardData.RequestOrders == null || requestBoardData.RequestOrders.Count == 0) return;
            gameObject.SetActive(true);
        }

        private void OnNewOrderAdded(RequestOrder requestOrder_)
        {
            var _requestMenuItem = GameObject.Instantiate(requestMenuItemPrefab, requestListParent).Initialize(this, requestOrder_);
            requestMenuItems.Add(_requestMenuItem);
        }
        
        private void PurgeNulls()
        {
            if(requestMenuItems == null || requestMenuItems.Count == 0) return;
            
            requestMenuItems.RemoveAll(_item => _item == null);
            
            for (int i = requestMenuItems.Count - 1; i >= 0; i--)
            {
                if(requestMenuItems[i].order != null) continue;
                requestMenuItems.RemoveAt(i);
                Destroy(requestMenuItems[i].gameObject);
            }
        }
    }
}
