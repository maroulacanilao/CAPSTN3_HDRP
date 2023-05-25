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
    public class Item_MenuItem : MonoBehaviour, IPointerClickHandler, ISelectHandler, IDragHandler, IEndDragHandler, IBeginDragHandler, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler
    {
        public enum InventoryItemType { storage = 0, toolBar = 1, weaponBar = 2, armorBar = 3  }
        
        [SerializeField] private TextMeshProUGUI countTXT;
        [SerializeField] private Image icon;
        [SerializeField] private Image ghostIcon;
        [SerializeField] private Color highLightColor;
        
        [field: SerializeField] public InventoryItemType inventoryItemType { get; private set; }

        public int index { get; private set; }
        
        public Button button { get; private set; }
        private PlayerInventory inventory;
        private InventoryMenu inventoryMenu;
        private Image borderImage;
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
        public static Item_MenuItem draggingItem;
        private static readonly Evt<Item_MenuItem> OnHighlightToolBar = new Evt<Item_MenuItem>();
        private static readonly Evt<Item_MenuItem> OnHighlightArmorBar = new Evt<Item_MenuItem>();
        private static readonly Evt<Item_MenuItem> OnHighlightWeaponBar = new Evt<Item_MenuItem>();
        private static readonly Evt<Item_MenuItem> OnRemoveHighlights = new Evt<Item_MenuItem>();


        private void OnEnable()
        {
            UpdateDisplay();
        }
        
        private void OnDisable()
        {
            ghostIcon.transform.localPosition = Vector3.zero;
            isDragging = false;
            draggingItem = null;
            RemoveHighlight(this);
        }
        
        public void Initialize(InventoryMenu inventoryMenu_, int index_)
        {
            index = index_;
            inventoryMenu = inventoryMenu_;
            inventory = inventoryMenu.Inventory;
            button = GetComponent<Button>();
            borderImage = button.image;
            InventoryEvents.OrganizeInventory.AddListener(UpdateDisplay);
            InventoryEvents.OnUpdateInventory.AddListener(UpdateDisplayWrapper);
            
            if(inventoryItemType == InventoryItemType.toolBar) OnHighlightToolBar.AddListener(Highlight);
            if(inventoryItemType == InventoryItemType.armorBar) OnHighlightArmorBar.AddListener(Highlight);
            if(inventoryItemType == InventoryItemType.weaponBar) OnHighlightWeaponBar.AddListener(Highlight);
            OnRemoveHighlights.AddListener(RemoveHighlight);
            
            hasInitialized = true;
        }

        public void OnDestroy()
        {
            InventoryEvents.OrganizeInventory.RemoveListener(UpdateDisplay);
            InventoryEvents.OnUpdateInventory.RemoveListener(UpdateDisplayWrapper);
            OnHighlightToolBar.RemoveListener(Highlight);
            OnHighlightArmorBar.RemoveListener(Highlight);
            OnHighlightWeaponBar.RemoveListener(Highlight);
            OnRemoveHighlights.RemoveListener(RemoveHighlight);
        }

        private void UpdateDisplayWrapper(PlayerInventory inventory_) => UpdateDisplay();

        public void UpdateDisplay()
        {
            if(!hasInitialized) return;
            bool isItemValid = item != null && item.Data != null;
            icon.gameObject.SetActive(isItemValid);
            countTXT.gameObject.SetActive(item is {IsStackable: true});
            
            if (isItemValid)
            {
                icon.sprite = item.Data.Icon;
                ghostIcon.sprite = item.Data.Icon;
            }
            else
            {
                icon.sprite = null;
                ghostIcon.sprite = null;
            }
            if(countTXT.gameObject.activeSelf) countTXT.SetText($"{item.StackCount}x");
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            button.Select();
        }
        
        public void OnSelect(BaseEventData eventData)
        {
            InventoryMenu.OnItemSelect.Invoke(this);
        }
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            if(item == null) return;
            isDragging = true;
            ghostIcon.transform.SetParent(inventoryMenu.ghostIconParent);
            ghostIcon.gameObject.SetActive(true);

            if (item.IsToolable)
            {
                OnHighlightToolBar.Invoke(this);
            }
            if(item.ItemType == ItemType.Armor) OnHighlightArmorBar.Invoke(this);
            if(item.ItemType == ItemType.Weapon) OnHighlightWeaponBar.Invoke(this);
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            if(item == null) return;
            ghostIcon.transform.position = Input.mousePosition;
        }
        
        public void OnEndDrag(PointerEventData eventData)
        {
            if(item == null) return;
            draggingItem = this;
            ghostIcon.gameObject.SetActive(false);
            ghostIcon.transform.SetParent(transform);
            ghostIcon.transform.position = transform.position;
            OnRemoveHighlights.Invoke(this);
            StartCoroutine(ExitDrag());
        }
        
        IEnumerator ExitDrag()
        {
            yield return null;
            isDragging = false;
            draggingItem = null;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if(!isDragging) return;
            if (draggingItem == this) return;
            SwapItems();
        }

        private void SwapItems()
        {
            if(draggingItem == null) return;
            
            switch (draggingItem.inventoryItemType)
            {
                case InventoryItemType.toolBar when this.inventoryItemType is InventoryItemType.toolBar:
                    inventory.SwapItemInToolBar(draggingItem.index, index);
                    break;
                
                case InventoryItemType.storage when this.inventoryItemType is InventoryItemType.storage:
                    inventory.SwapItemsInStorage(draggingItem.index, index);
                    break;
                
                case InventoryItemType.toolBar when this.inventoryItemType is InventoryItemType.storage:
                    inventory.SwapItemsInToolBarAndStorage(draggingItem.index, index);
                    break;
                
                case InventoryItemType.storage when this.inventoryItemType is InventoryItemType.toolBar:
                    inventory.SwapItemsInToolBarAndStorage(index, draggingItem.index);
                    break;
                
                case InventoryItemType.weaponBar when this.inventoryItemType is InventoryItemType.storage:
                    inventory.EquipWeapon(this.index);
                    break;
                
                case InventoryItemType.storage when this.inventoryItemType is InventoryItemType.weaponBar:
                    inventory.EquipWeapon(draggingItem.index);
                    break;
                
                case InventoryItemType.armorBar when this.inventoryItemType is InventoryItemType.storage:
                    inventory.EquipArmor(this.index);
                    break;
                
                case InventoryItemType.storage when this.inventoryItemType is InventoryItemType.armorBar:
                    inventory.EquipArmor(draggingItem.index);
                    break;
            }
            
            
            UpdateDisplay();
            draggingItem.UpdateDisplay();
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            button.Select();
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            button.Select();
        }
        
        public void Highlight(Item_MenuItem item)
        {
            borderImage.color = highLightColor;
        }
        
        public void RemoveHighlight(Item_MenuItem item)
        {
            borderImage.color = Color.white;
        }
    }
}