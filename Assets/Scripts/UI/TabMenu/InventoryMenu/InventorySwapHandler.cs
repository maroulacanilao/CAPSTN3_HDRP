using CustomHelpers;
using Items;
using Items.Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using static UI.TabMenu.InventoryMenu.Item_MenuItem.InventoryItemType;

namespace UI.TabMenu.InventoryMenu
{
    public class InventorySwapHandler : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI errorTxt;
        [SerializeField] private InventoryMenu inventoryMenu;
        private PlayerInventory inventory => inventoryMenu.Inventory;

        private Item_MenuItem swappingItem
        {
            get => Item_MenuItem.swappingItem;
            set => Item_MenuItem.swappingItem = value;
        }
        
        private Item_MenuItem selectedItem
        {
            get => Item_MenuItem.selectedItem;
            set => Item_MenuItem.selectedItem = value;
        }
        
        private bool isSwapping => swappingItem != null;
        

        private void Reset()
        {
            inventoryMenu = GameObject.FindObjectOfType<InventoryMenu>();
        }

        private void OnEnable()
        {
            InputUIManager.OnSwap.AddListener(OnSwap);
            InputUIManager.OnCancel.AddListener(OnCancel);
            errorTxt.gameObject.SetActive(false);
            
            Item_MenuItem.OnDragStateChange.AddListener(OnDragStateChange);
        }

        private void OnDisable()
        {
            InputUIManager.OnSwap.RemoveListener(OnSwap);
            InputUIManager.OnCancel.RemoveListener(OnCancel);
            selectedItem = null;
            swappingItem = null;
            ResetOutlines();
        }
        
        private void OnCancel()
        {
            if(!enabled) return;
            swappingItem = null;
            ResetOutlines();
            
            if(selectedItem.IsEmptyOrDestroyed()) return;
            selectedItem.button.Select();
            
            if(swappingItem == null) selectedItem.SelectButton();
        }

        private void OnSwap()
        {
            Debug.Log("Swap");
            if (!enabled)
            {
                // Debug.Log("Not Enabled");
                return;
            }
            if (selectedItem.IsEmptyOrDestroyed())
            {
                // Debug.Log(selectedItem.gameObject);
                return;
            }
            // Debug.Log($"Selected Item: {selectedItem.item}");
            if(!isSwapping) SelectItem();
            else SwapItem();
        }
        
        private void SelectItem()
        {
            if(selectedItem == null) return;
            if(selectedItem.item == null) return;
            
            swappingItem = selectedItem;
            Highlight();
            selectedItem.button.Select();
            swappingItem.SwappingOutline();
        }

        private void SwapItem()
        {
            if(!CanSwap()) return;

            switch (swappingItem.inventoryItemType)
            {
                case toolBar when selectedItem.inventoryItemType is toolBar:
                    inventory.SwapItemInToolBar(swappingItem.index, selectedItem.index);
                    break;
                
                case storage when selectedItem.inventoryItemType is storage:
                    inventory.SwapItemsInStorage(swappingItem.index, selectedItem.index);
                    break;
                
                case toolBar when selectedItem.inventoryItemType is storage:
                    inventory.SwapItemsInToolBarAndStorage(swappingItem.index, selectedItem.index);
                    break;
                
                case storage when selectedItem.inventoryItemType is toolBar:
                    inventory.SwapItemsInToolBarAndStorage(selectedItem.index, swappingItem.index);
                    break;
                
                case weaponBar when selectedItem.inventoryItemType is storage:
                    inventory.EquipWeapon(selectedItem.index);
                    break;
                
                case storage when selectedItem.inventoryItemType is weaponBar:
                    inventory.EquipWeapon(swappingItem.index);
                    break;
                
                case armorBar when selectedItem.inventoryItemType is storage:
                    inventory.EquipArmor(selectedItem.index);
                    break;
                
                case storage when selectedItem.inventoryItemType is armorBar:
                    inventory.EquipArmor(swappingItem.index);
                    break;
                
                default: break;
            }
            
            swappingItem = null;
            ResetOutlines();
        }
        
        private void ResetOutlines()
        {
            foreach (var _i in inventoryMenu.storageItems)
            {
                _i.ResetOutline();
            }

            foreach (var _i in inventoryMenu.toolBarItems)
            {
                _i.ResetOutline();
            }
            
            inventoryMenu.armorBar.ResetOutline();
            inventoryMenu.weaponBar.ResetOutline();
        }

        public bool CanSwap()
        {
            var _item = swappingItem.item;
            if(_item == null) return false;
            var _itemName = $"<color=orange>{_item.Data.ItemName}</color>";
            var _level = inventoryMenu.playerData.LevelData.CurrentLevel;
            
            switch (selectedItem.inventoryItemType)
            {
                case toolBar:
                    if (!_item.IsToolable)
                    {
                        errorTxt.gameObject.SetActive(true);
                        errorTxt.SetText(_itemName + " cannot be equipped to tool bar");
                        return false;
                    }
                    break;

                case weaponBar:
                    if (_item.ItemType != ItemType.Weapon)
                    {
                        errorTxt.gameObject.SetActive(true);
                        errorTxt.SetText(_itemName + " cannot be equipped as Weapon");
                        return false;
                    }
                    else if (_item is ItemWeapon weapon_)
                    {
                        if (weapon_.Level > _level)
                        {
                            errorTxt.gameObject.SetActive(true);
                            errorTxt.SetText("Your level is too low to equip " + _itemName);
                            Debug.Log($"Weapon Level: {weapon_.Level} | Player Level: {_level}");
                            return false;
                        }
                    }
                    break;
                
                case armorBar:
                    if (_item.ItemType != ItemType.Armor)
                    {
                        errorTxt.gameObject.SetActive(true);
                        errorTxt.SetText(_itemName + " cannot be equipped as Armor");
                        return false;
                    }
                    else if (_item is ItemArmor armor_)
                    {
                        if (armor_.Level > _level)
                        {
                            errorTxt.gameObject.SetActive(true);
                            Debug.Log($"Armor Level: {armor_.Level} | Player Level: {_level}");
                            errorTxt.SetText("Your level is too low to equip " + _itemName);
                            return false;
                        }
                    }
                    break;
                
                case storage:
                default:
                    break;
            }
            
            errorTxt.gameObject.SetActive(false);
            return true;
        }

        public void Highlight()
        {
            ResetOutlines();
            var _item = swappingItem.item;
            if(_item == null) return;
            
            if (_item.IsToolable)
            {
                foreach (var _i in inventoryMenu.toolBarItems)
                {
                    _i.EquipOutline();
                }
                return;
            }
            
            if (_item.ItemType == ItemType.Armor)
            {
                inventoryMenu.armorBar.EquipOutline();
                return;
            }
            if (_item.ItemType == ItemType.Weapon)
            {
                inventoryMenu.weaponBar.EquipOutline();
                return;
            }
        }
        
        private void OnDragStateChange(PointerEventData eventData_, DragState state_)
        {
            switch (state_)
            {
                case DragState.BeginDrag:
                    swappingItem = selectedItem;
                    Highlight();
                    swappingItem.SwappingOutline();
                    break;
                case DragState.Dragging:
                    break;

                case DragState.EndDrag:
                {
                    if(swappingItem == null) return;
                    
                    var _res = eventData_.pointerCurrentRaycast;
                    if(_res.gameObject == null) return;

                    var _menuEndItem = _res.gameObject.GetComponent<Item_MenuItem>();
                    
                    if(_menuEndItem == null) return;
                    
                    if(_menuEndItem == swappingItem) return;
                    if(swappingItem.item == null) return;
                    
                    selectedItem = _menuEndItem;
                    SwapItem();
                    _menuEndItem.button.Select();
                    break;
                }
            }
        }
        
    }
}