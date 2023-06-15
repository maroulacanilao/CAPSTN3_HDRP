using System;
using System.Collections.Generic;
using CustomHelpers;
using TMPro;
using Trading;
using UnityEngine;

namespace UI.RequestBoardUI
{
    public class RequestMenuDetails : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI requestNameTxt, requestDescriptionTxt;
        [SerializeField] private Transform requestItemsParent;
        [SerializeField] private RequestItemLabel requestItemLabelPrefab;
        
        private RequestMenu requestMenu;
        private RequestMenuItem currentRequestMenuItem;
        private RequestOrder currentRequestOrder => currentRequestMenuItem.order;

        private readonly List<RequestItemLabel> requestItemUiList = new List<RequestItemLabel>();

        public void Initialize(RequestMenu requestMenu_)
        {
            requestMenu = requestMenu_;
            requestMenu.OnSelectRequest.AddListener(ShowDetails);
            
            requestNameTxt.SetText("");
            requestDescriptionTxt.SetText("");
            
            RemoveItems();
        }
        
        public void OnDestroy()
        {
            requestMenu.OnSelectRequest.RemoveListener(ShowDetails);
        }
        
        public void ShowDetails(RequestMenuItem requestMenuItem_)
        {
            currentRequestMenuItem = requestMenuItem_;
            requestNameTxt.SetText($"Request from {currentRequestOrder.RequesterName}");
            requestDescriptionTxt.SetText(currentRequestOrder.OrderDescription);

            RemoveItems();
            
            foreach (var _item in currentRequestOrder.RequestedItems)
            {
                var _requestItemUi = GameObject.Instantiate(requestItemLabelPrefab, requestItemsParent).Initialize(_item.Key, _item.Value.RequestCount);
                requestItemUiList.Add(_requestItemUi);
            }

            gameObject.SetActive(true);
        }

        private void OnDisable()
        {
            RemoveItems();
            requestNameTxt.SetText("");
            requestDescriptionTxt.SetText("");
        }

        private void RemoveItems()
        {
            if(requestItemUiList.Count == 0) return;
            requestItemUiList.DestroyComponents();
            requestItemUiList.Clear();
        }
    }
}
