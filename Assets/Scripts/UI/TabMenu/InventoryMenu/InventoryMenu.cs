﻿using System;
using Character;
using CustomEvent;
using CustomHelpers;
using Items.Inventory;
using Player;
using ScriptableObjectData;
using ScriptableObjectData.CharacterData;
using TMPro;
using UI.Farming;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

namespace UI.TabMenu.InventoryMenu
{
    [DefaultExecutionOrder(2)]
    public class InventoryMenu : PlayerMenu
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
        public static readonly Evt<InventoryMenu> OnInventoryMenuOpen = new Evt<InventoryMenu>();
        public static readonly Evt<InventoryMenu> OnInventoryMenuClose = new Evt<InventoryMenu>();
        
        private Item_MenuItem lastSelectedItem;
        
        private EventSystem eventSystem => EventSystem.current;

        // [SerializeField] private GameObject inventoryPanel;
        [SerializeField] private GameObject[] itemsPanel;

        private void Awake()
        {
            // InputUIManager.OnInventoryMenu.AddListener(ShowInventoryPanel);

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

            // InputUIManager.OnCancel.AddListener(CloseMenu);
        }

        protected override void OnEnable()
        {
            // base function goes here
            base.OnEnable();

            EventSystem.current.SetSelectedGameObject(storageItems[0].gameObject);
            SelectableMenuButton.OnSelectButton.AddListener(SelectItem);
            InputUIManager.OnMove.AddListener(OnMove);
            OnInventoryMenuOpen.Invoke(this);

            if (itemsPanel != null) PrevItemsInInventory();
        }

        protected override void OnDisable()
        {
            // base function goes here
            base.OnDisable();

            SelectableMenuButton.OnSelectButton.RemoveListener(SelectItem);
            InputUIManager.OnMove.RemoveListener(OnMove);
            OnInventoryMenuClose.Invoke(this);
        }
        
        private void SelectItem(SelectableMenuButton button_)
        {
            if (button_.TryGetComponent(out Item_MenuItem _itemMenuItem))
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

        public void NextItemsInInventory()
        {
            if (itemsPanel != null)
            {
                itemsPanel[0].SetActive(false);
                itemsPanel[1].SetActive(true);
            }
        }

        public void PrevItemsInInventory()
        {
            if (itemsPanel != null)
            {
                itemsPanel[1].SetActive(false);
                itemsPanel[0].SetActive(true);
            }
        }

        public void OnExitBtnClicked()
        {
            CloseMenu();
        }
    }
}