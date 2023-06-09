﻿using System;
using Character;
using CustomEvent;
using Items.Inventory;
using Player;
using ScriptableObjectData;
using ScriptableObjectData.CharacterData;
using TMPro;
using UI.Farming;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace UI.TabMenu.InventoryMenu
{
    [DefaultExecutionOrder(2)]
    public class InventoryMenu : MonoBehaviour
    {
        [field: Header("Data")]
        [field: SerializeField] public PlayerData playerData { get; private set; }
        [field: SerializeField] public PlayerInventory Inventory { get; private set; }
        [field: SerializeField] public GameDataBase gameDataBase { get; private set; }

        [field: Header("Panels")]
        [field: SerializeField] public Transform ghostIconParent { get; private set; }
        [field: SerializeField] public InventoryDetailsPanel inventoryDetailsPanel { get; private set; }
        
        [field: SerializeField] public Item_MenuItem[] toolBarItems { get; private set; }
        [field: SerializeField] public Item_MenuItem[] storageItems { get; private set; }
        [field: SerializeField] public Item_MenuItem weaponBar { get; private set; }
        [field: SerializeField] public Item_MenuItem armorBar { get; private set; }

        public static readonly Evt<SelectableMenuButton> OnInventoryItemSelect = new Evt<SelectableMenuButton>();
        
        private Item_MenuItem lastSelectedItem;
        
        private EventSystem eventSystem => EventSystem.current;
        
        private void Awake()
        {
            for (int i = 0; i < toolBarItems.Length; i++)
            {
                toolBarItems[i].Initialize(this,i);
            }
            
            for (int j = 0; j < storageItems.Length; j++)
            {
                storageItems[j].Initialize(this,j);
            }
            armorBar.Initialize(this,0);
            weaponBar.Initialize(this,0);
            
            inventoryDetailsPanel.Initialize(this);
            inventoryDetailsPanel.ShowItemDetail(null);
        }

        private void OnEnable()
        {
            EventSystem.current.SetSelectedGameObject(storageItems[0].gameObject);
            SelectableMenuButton.OnSelectButton.AddListener(SelectItem);
            InputUIManager.OnMove.AddListener(OnMove);
        }

        private void OnDisable()
        {
            SelectableMenuButton.OnSelectButton.RemoveListener(SelectItem);
            InputUIManager.OnMove.RemoveListener(OnMove);
        }
        
        private void SelectItem(SelectableMenuButton button_)
        {
            if (button_ is Item_MenuItem _itemMenuItem)
            {
                lastSelectedItem = _itemMenuItem;
                inventoryDetailsPanel.ShowItemDetail(_itemMenuItem);
            }
            else
            {
                if(enabled) lastSelectedItem.SelectOutline(); 
            }
        }

        private void OnMove(InputAction.CallbackContext context_)
        {
            var _currentSelected = EventSystem.current.currentSelectedGameObject;
            if(_currentSelected == null || _currentSelected.activeInHierarchy) return;

            SelectLastSelectable();
        }

        public void SelectLastSelectable()
        {
            lastSelectedItem = lastSelectedItem == null ? storageItems[0] : lastSelectedItem;
            if(!eventSystem.alreadySelecting) eventSystem.SetSelectedGameObject(lastSelectedItem.gameObject);
        }
        
        public void ShowItemDetail(Item_MenuItem itemMenuItem_)
        {
            inventoryDetailsPanel.ShowItemDetail(itemMenuItem_);
        }
    }
}