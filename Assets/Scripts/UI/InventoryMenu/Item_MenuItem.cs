using System;
using System.Collections;
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
        [SerializeField] private TextMeshProUGUI countTXT;
        [SerializeField] private Image icon;
        [SerializeField] private Image ghostIcon;
        [field: SerializeField] public bool isToolBar { get; private set; } = false;
        public int index { get; private set; }
        public Button button { get; private set; }
        private PlayerInventory inventory;
        private InventoryMenu inventoryMenu;
        public Item item => isToolBar ? inventory.ItemTools[index] : inventory.GetItemInStorage(index);

        public static bool isDragging;
        public static Item_MenuItem draggingItem;

        private void OnEnable()
        {
            UpdateDisplay();
        }
        
        private void OnDisable()
        {
            isDragging = false;
            draggingItem = null;
        }
        
        public void Initialize(InventoryMenu inventoryMenu_, int index_)
        {
            index = index_;
            inventoryMenu = inventoryMenu_;
            inventory = inventoryMenu.Inventory;
            button = GetComponent<Button>();
            InventoryEvents.OrganizeInventory.AddListener(UpdateDisplay);
        }

        public void OnDestroy()
        {
            InventoryEvents.OrganizeInventory.RemoveListener(UpdateDisplay);
        }

        public void UpdateDisplay()
        {
            icon.gameObject.SetActive(item != null);
            countTXT.gameObject.SetActive(item is {IsStackable: true});
            
            icon.sprite = ghostIcon.sprite = item?.Data.Icon;
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
            
            if(draggingItem.isToolBar && isToolBar)
            {
                // var temp = inventory.ItemTools[index];
                // inventory.ItemTools[index] = inventory.ItemTools[draggingItem.index];
                // inventory.ItemTools[draggingItem.index] = temp;
                inventory.SwapItemInToolBar(draggingItem.index, index);
            }
            else if(!draggingItem.isToolBar && !isToolBar)
            {
                // var temp = inventory.ItemStorage[index];
                // inventory.ItemStorage[index] = inventory.ItemStorage[draggingItem.index];
                // inventory.ItemStorage[draggingItem.index] = temp;
                inventory.SwapItemsInStorage(draggingItem.index, index);
            }
            else if(draggingItem.isToolBar && !isToolBar)
            {
                // var temp = inventory.ItemStorage[index];
                // inventory.ItemStorage[index] = inventory.ItemTools[draggingItem.index];
                // inventory.ItemTools[draggingItem.index] = temp;
                inventory.SwapItemsInToolBarAndStorage(draggingItem.index, index);
            }
            else if(!draggingItem.isToolBar && isToolBar)
            {
                // var temp = inventory.ItemTools[index];
                // inventory.ItemTools[index] = inventory.ItemStorage[draggingItem.index];
                // inventory.ItemStorage[draggingItem.index] = temp;
                inventory.SwapItemsInToolBarAndStorage(index, draggingItem.index);
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
    }
}