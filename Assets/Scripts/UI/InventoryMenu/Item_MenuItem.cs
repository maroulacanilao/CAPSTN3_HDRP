using System;
using System.Collections;
using CustomEvent;
using Items;
using Items.Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.InventoryMenu
{
    [RequireComponent(typeof(Button))]
    public class Item_MenuItem : SelectableMenuButton
    {
        public enum InventoryItemType { storage = 0, toolBar = 1, weaponBar = 2, armorBar = 3  }
        
        [SerializeField] private TextMeshProUGUI countTxt;
        [SerializeField] private Image icon;
        [SerializeField] private Image ghostIcon;
        [SerializeField] private Color highLightColor;
        [SerializeField] private Color swappingColor;

        [field: SerializeField] public InventoryItemType inventoryItemType { get; private set; }
        
        public static readonly Evt OnItemClick = new Evt();
        private bool isHighlighting;

        public int index { get; private set; }
        
        private PlayerInventory inventory;
        private InventoryMenu inventoryMenu;
        private bool hasInitialized;
        
        public Item item
        {
            get
            {
                return inventoryItemType switch
                {
                    InventoryItemType.toolBar => inventory.ItemTools[index],
                    InventoryItemType.weaponBar => inventory.WeaponEquipped,
                    InventoryItemType.armorBar => inventory.ArmorEquipped,
                    _ => inventory.GetItemInStorage(index)
                };
            }
        }

        public static bool isDragging;
        
        public static Item_MenuItem swappingItem { get; set; }
        public static Item_MenuItem selectedItem { get; set; }


        private void OnEnable()
        {
            UpdateDisplay();
        }

        protected override void OnDisable()
        {
            ghostIcon.transform.localPosition = Vector3.zero;
            isDragging = false;
        }
        
        public void Initialize(InventoryMenu inventoryMenu_, int index_)
        {
            index = index_;
            inventoryMenu = inventoryMenu_;
            inventory = inventoryMenu.Inventory;
            button = GetComponent<Button>();
            InventoryEvents.OrganizeInventory.AddListener(UpdateDisplay);
            InventoryEvents.OnUpdateInventory.AddListener(UpdateDisplayWrapper);
            button.onClick.AddListener(ItemClickWrapper);
            outline.effectDistance = outlineSize;


            hasInitialized = true;
        }

        public void OnDestroy()
        {
            InventoryEvents.OrganizeInventory.RemoveListener(UpdateDisplay);
            InventoryEvents.OnUpdateInventory.RemoveListener(UpdateDisplayWrapper);
            button.onClick.RemoveListener(ItemClickWrapper);
        }

        private void ItemClickWrapper()
        {
            InventoryMenu.OnInventoryItemSelect.Invoke(this);
            button.Select();
            OnItemClick.Invoke();
        }

        private void UpdateDisplayWrapper(PlayerInventory inventory_) => UpdateDisplay();

        public void UpdateDisplay()
        {
            if(!hasInitialized) return;
            bool _isItemValid = item != null && item.Data != null;
            countTxt.gameObject.SetActive(item is {IsStackable: true});
            
            if (_isItemValid)
            {
                icon.sprite = item.Data.Icon;
                ghostIcon.sprite = item.Data.Icon;
            }
            else
            {
                icon.sprite = null;
                ghostIcon.sprite = null;
            }
            if(countTxt.gameObject.activeSelf) countTxt.SetText($"{item.StackCount}x");
        }

        public override void SelectButton()
        {
            base.SelectButton();
            InventoryMenu.OnInventoryItemSelect.Invoke(this);
            selectedItem = this;
        }
        
        public override void DeselectButton()
        {
            if (swappingItem == this)
            {
                SwappingOutline();
                return;
            }
            if (isHighlighting)
            {
                EquipOutline();
                return;
            }
            
            outline.effectColor = Color.clear;
        }

        public void SwappingOutline()
        {
            outline.effectColor = swappingColor;
        }
        
        public void EquipOutline()
        {
            if(this == swappingItem) return;
            isHighlighting = true;
            outline.effectColor = highLightColor;
        }

        public void ResetOutline()
        {
            isHighlighting = false;
            if(selectedItem == this) return;
            outline.effectColor = Color.clear;
        }
    }
}