using CustomEvent;
using Items;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.ShippingBinUI
{
    public class ShippingMenuItem : SelectableMenuButton, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI countText;
        [SerializeField] private bool isInventory;
        
        public Item item { get; private set; }
        public bool IsInventory => isInventory;
        
        public static readonly Evt<ShippingMenuItem> OnItemClicked = new Evt<ShippingMenuItem>();

        void Reset()
        {
            icon = GetComponentInChildren<Image>();
            countText = GetComponentInChildren<TextMeshProUGUI>();
        }
        
        public void Initialize(bool isInventory_)
        {
            isInventory = isInventory_;
            button.onClick.AddListener(() => OnItemClicked.Invoke(this));
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if(item == null) return;
            if(!item.Data.IsShippable) return;
            
            var _builder = new System.Text.StringBuilder();
            _builder.Append(item.Data.ItemName);
            _builder.Append("\n");
            _builder.Append(item.Data.ShippingValue);
            _builder.Append("g");
            
            ToolTip.OnShowToolTip.Invoke(_builder.ToString());
        }
        
        public void OnPointerExit(PointerEventData eventData)
        {
            ToolTip.OnHideToolTip.Invoke();
        }
        
        public void DisplayItem(Item item_)
        {
            item = item_;
            button.interactable = item != null && item.Data.IsShippable;
            icon.color = item != null && item.Data.IsShippable ? Color.white : Color.gray;
            
            if(item == null)
            {
                icon.sprite = null;
                countText.text = "";
                countText.gameObject.SetActive(false);
                return;
            }
            
            icon.sprite = item.Data.Icon;
            
            if(item is ItemStackable stackable)
            {
                countText.text = stackable.StackCount.ToString();
                countText.gameObject.SetActive(true);
            }
            else
            {
                countText.text = "";
                countText.gameObject.SetActive(false);
            }
        }
    }
}
