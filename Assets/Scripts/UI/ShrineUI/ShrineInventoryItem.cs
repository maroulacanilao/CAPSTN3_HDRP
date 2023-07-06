using System;
using CustomEvent;
using CustomHelpers;
using Items;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.ShrineUI
{
    public class ShrineInventoryItem : SelectableMenuButton, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI countTxt;
        
        public static readonly Evt<ShrineInventoryItem> OnItemClicked = new Evt<ShrineInventoryItem>();
        
        public Item item { get; private set; }
        public bool canInteract { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            button.onClick.AddListener(OnClick);
        }

        private void OnDestroy()
        {
            button.onClick.RemoveListener(OnClick);
        }
        
        private void OnClick()
        {
            if(!canInteract || item == null) return;
            OnItemClicked.Invoke(this);
        }

        public void SetItem(Item item_, bool canInteract_)
        {
            item = item_;
            canInteract = canInteract_;
            countTxt.text = "";
            
            if(item_ == null)
            {
                icon.sprite = null;
                countTxt.text = "";
                button.interactable = false;
                icon.color = Color.gray;
                return;
            }

            icon.sprite = item_.Data.Icon;
            if (item_ is ItemStackable _stackable) countTxt.text = $"x{_stackable.StackCount}";
            
            button.interactable = canInteract_;
            icon.color = item != null && canInteract_ ? Color.white : Color.gray;
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            if(!canInteract) return;
            ToolTip.OnShowToolTip.Invoke(item.Data.ItemName.Beautify());
        }
        
        public void OnPointerExit(PointerEventData eventData)
        {
            ToolTip.OnHideToolTip.Invoke();
        }
    }
}
