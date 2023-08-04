using CustomEvent;
using CustomHelpers;
using Items;
using Items.Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.TabMenu.InventoryMenu
{
    public enum DragState { BeginDrag, Dragging, EndDrag }
    
    [RequireComponent(typeof(Button))]
    public class Item_MenuItem : SelectableMenuButton, IDragHandler, IEndDragHandler, IBeginDragHandler
    {
        public enum InventoryItemType { storage = 0, toolBar = 1, weaponBar = 2, armorBar = 3  }

        [SerializeField] private Image bg;
        [SerializeField] private TextMeshProUGUI countTxt;
        [SerializeField] private Image icon;
        [SerializeField] private Image ghostIcon;
        [SerializeField] private Color highLightColor;
        [SerializeField] private Color swappingColor;
        [SerializeField] Sprite selectedSprite, deselectedSprite;

        [field: SerializeField] public InventoryItemType inventoryItemType { get; private set; }
        
        public static readonly Evt<Item_MenuItem> OnItemClick = new Evt<Item_MenuItem>();
        public static readonly Evt<PointerEventData,DragState> OnDragStateChange = new Evt<PointerEventData, DragState>();
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

        public static Item_MenuItem swappingItem { get; set; }
        public static Item_MenuItem selectedItem { get; set; }

        protected void OnValidate()
        {
            bg = GetComponent<Image>();
        }


        protected override void OnEnable()
        {
            base.OnEnable();
            UpdateDisplay();
            ghostIcon.transform.localPosition = Vector3.zero;
            ghostIcon.gameObject.SetActive(false);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            ghostIcon.transform.localPosition = Vector3.zero;
            ghostIcon.gameObject.SetActive(false);
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
            
            UpdateDisplay();
        }

        public void OnDestroy()
        {
            InventoryEvents.OrganizeInventory.RemoveListener(UpdateDisplay);
            InventoryEvents.OnUpdateInventory.RemoveListener(UpdateDisplayWrapper);
            button.onClick.RemoveListener(ItemClickWrapper);
        }

        private void ItemClickWrapper()
        {
            if(item == null) return;

            OnItemClick.Invoke(this);
        }

        private void UpdateDisplayWrapper(PlayerInventory inventory_) => UpdateDisplay();

        public void UpdateDisplay()
        {
            if(this.IsEmptyOrDestroyed()) return;
            if(!hasInitialized) return;
            bool _isItemValid = item != null && item.Data != null;
            bg.color = Color.white;

            if (_isItemValid)
            {
                icon.sprite = item.Data.Icon;
                ghostIcon.sprite = item.Data.Icon;
                icon.color = Color.white;
            }
            else
            {
                icon.sprite = null;
                ghostIcon.sprite = null;
                icon.color = Color.clear;
                countTxt.gameObject.SetActive(false);
                return;
            }
            
            countTxt.gameObject.SetActive(item is {IsStackable: true});

            if(countTxt.gameObject.activeSelf) countTxt.SetText($"{item.StackCount}x");
        }

        public override void SelectButton()
        {
            SelectOutline();
            InventoryMenu.OnInventoryItemSelect.Invoke(this);
            selectedItem = this;
        }
        
        public void SelectOutline()
        {
            outline.effectColor = outlineColor;
            bg.sprite = selectedSprite;
        }
        
        public override void DeselectButton()
        {
            if (swappingItem == this)
            {
                SwappingOutline();
                return;
            }
            bg.sprite = deselectedSprite;
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
            bg.color = swappingColor;
        }
        
        public void EquipOutline()
        {
            if(this == swappingItem) return;
            isHighlighting = true;
            outline.effectColor = highLightColor;
            bg.color = highLightColor;
        }

        public void ResetOutline()
        {
            isHighlighting = false;
            if(selectedItem == this) return;
            outline.effectColor = Color.clear;
            bg.sprite = deselectedSprite;
            bg.color = Color.white;
        }
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            if(item == null) return;
            
            selectedItem = this;
            ghostIcon.transform.SetParent(inventoryMenu.ghostIconParent);
            ghostIcon.gameObject.SetActive(true);
            OnDragStateChange.Invoke(eventData, DragState.BeginDrag);
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            if(item == null) return;
            if (swappingItem == null)
            {
                ghostIcon.gameObject.SetActive(false);
                ghostIcon.transform.SetParent(transform);
                ghostIcon.transform.position = transform.position;
                return;
            }
            ghostIcon.transform.position = Input.mousePosition;
            OnDragStateChange.Invoke(eventData, DragState.Dragging);
        }
        
        public void OnEndDrag(PointerEventData eventData)
        {
            ghostIcon.gameObject.SetActive(false);
            ghostIcon.transform.SetParent(transform);
            ghostIcon.transform.position = transform.position;
            OnDragStateChange.Invoke(eventData, DragState.EndDrag);
        }
    }
}