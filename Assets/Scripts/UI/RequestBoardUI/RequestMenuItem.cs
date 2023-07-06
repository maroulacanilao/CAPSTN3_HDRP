using System;
using TMPro;
using Trading;
using UnityEngine;

namespace UI.RequestBoardUI
{
    public class RequestMenuItem : SelectableMenuButton
    {
        [SerializeField] TextMeshProUGUI requestNameTxt;
        public RequestOrder order { get; private set; }
        private RequestMenu requestMenu;
        
        public RequestMenuItem Initialize(RequestMenu menu_, RequestOrder order_)
        {
            order = order_;
            requestMenu = menu_;
            button.onClick.AddListener(OnClick);
            requestNameTxt.SetText($"Request from {order.RequesterName}");
            outline.effectColor = Color.clear;
            return this;
        }

        public override void SelectButton()
        {
            base.SelectButton();
            requestMenu.OnSelectRequest.Invoke(this);
        }

        public void OnDestroy()
        {
            button.onClick.RemoveListener(OnClick);
        }

        private void OnClick()
        {
            button.Select();
            requestMenu.OnSelectRequest.Invoke(this);
        }
    }
}
