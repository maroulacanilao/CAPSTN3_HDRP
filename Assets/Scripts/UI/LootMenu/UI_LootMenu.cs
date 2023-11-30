using System;
using System.Collections.Generic;
using CustomEvent;
using CustomHelpers;
using Items;
using Items.Inventory;
using ObjectPool;
using TMPro;
using UI.Farming;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UI.LootMenu
{
    public class UI_LootMenu : PlayerMenu
    {
        [SerializeField] private PlayerInventory playerInventory;
        [SerializeField] private UI_LootMenuItem lootMenuItemPrefab;
        [SerializeField] private UI_LootMenuDetailsPanel detailsPanel;
        
        [Header("Parents")]
        [SerializeField] private Transform itemParent;

        [Header("Buttons")]
        [SerializeField] private Button lootAllBtn, trashAllBtn;

        public static readonly Evt<UI_LootMenuItem> OnShowItemDetail = new Evt<UI_LootMenuItem>();
        public static readonly Evt<UI_LootMenuItem> OnItemClick = new Evt<UI_LootMenuItem>();
        private List<UI_LootMenuItem> lootMenuItemList;
        private LootDropObject lootDropObject;

        private int lastItemIndex;
        public PlayerInventory inventory => playerInventory;

        [SerializeField] private GameObject notifPanel;
        [SerializeField] private TextMeshProUGUI notifTxt;

        #region UnityEvents

        private void Awake()
        {
            detailsPanel.Initialize(this);
            lootAllBtn.onClick.AddListener(LootAll);

            trashAllBtn.onClick.AddListener(TrashAll);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            OnShowItemDetail.AddListener(ShowItemDetail);
            SelectableMenuButton.OnSelectButton.AddListener(OnSelect);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            
            lootDropObject = null;
            RemoveAllItemOnly();
            
            OnShowItemDetail.RemoveListener(ShowItemDetail);
            SelectableMenuButton.OnSelectButton.RemoveListener(OnSelect);
        }

        #endregion

        private void OnSelect(SelectableMenuButton selectableMenuButton_)
        {
            if (selectableMenuButton_.IsValid() && selectableMenuButton_ is UI_LootMenuItem _menuItem)
            {
                lastItemIndex = lootMenuItemList.IndexOf(_menuItem);
            }
        }

        public void ShowLootMenu(LootDropObject lootDropObject_)
        {
            if (this.IsEmptyOrDestroyed())
            {
                // Debug.LogError("Loot Menu is not initialized");
                OnShowItemDetail.RemoveListener(ShowItemDetail);
                return;
            }
            
            //PlayerMenuManager.OnCloseAllUI.Invoke();
            RevisedPlayerMenuManager.OnCloseAllUIRevised.Invoke();
            RemoveAllItemOnly();
            
            lootDropObject = lootDropObject_;
            var _lootDrop = lootDropObject_.lootDrop;
            
            lootMenuItemList = new List<UI_LootMenuItem>();

            if (_lootDrop.itemsDrop.Count <= 0)
            {
                lootDropObject_.ReturnToPool();
                return;
            }

            foreach (var _item in _lootDrop.itemsDrop)
            {
                var _uiLootMenuItem = Instantiate(lootMenuItemPrefab, itemParent).Initialize(_item, null);
                lootMenuItemList.Add(_uiLootMenuItem);
            }
            var _first = lootMenuItemList[0];
            if(_first == null) return;
            EventSystem.current.SetSelectedGameObject(_first.gameObject);

            gameObject.SetActive(true);
            detailsPanel.ShowItemDetail(_first);
        }

        private void ShowItemDetail(UI_LootMenuItem lootMenuItem_)
        {
            detailsPanel.ShowItemDetail(lootMenuItem_);
        }

        public void Loot(UI_LootMenuItem lootMenuItem_)
        {
            if (HasFreeSpace())
            {
                if (!playerInventory.AddItem(lootMenuItem_.item)) return;
                RemoveMenuItem(lootMenuItem_);
            }
            else
            {
                if (notifPanel != null)
                {
                    notifPanel.SetActive(true);
                    notifTxt.SetText("INVENTORY FULL!");
                }
            }
        }
        
        public void RemoveMenuItem(UI_LootMenuItem lootMenuItem_)
        {
            lootMenuItemList.Remove(lootMenuItem_);
            lootDropObject.lootDrop.itemsDrop.Remove(lootMenuItem_.item);
            Destroy(lootMenuItem_.gameObject);

            if (lootMenuItemList.Count == 0)
            {
                lootDropObject.ReturnToPool();
                CloseMenu();
                return;
            }
            
            SelectLastSelectable();

        }
        
        public void TrashItem(UI_LootMenuItem lootMenuItem_)
        {
            if(!lootMenuItem_.item.IsDiscardable) return;
            RemoveMenuItem(lootMenuItem_);
        }

        protected override void CloseMenu()
        {
            lootDropObject = null;
            RemoveAllItemOnly();
            base.CloseMenu();
        }
        
        public void RemoveAllItemOnly()
        {
            if(lootMenuItemList == null) return;
            if(lootMenuItemList.Count <= 0) return;
            
            for (int i = lootMenuItemList.Count - 1; i >= 0; i--)
            {
                if(lootMenuItemList[0].IsEmptyOrDestroyed()) continue;
                
                Destroy(lootMenuItemList[i].gameObject);
            }
            
            lootMenuItemList.Clear();
        }
        
        private void OnMove(InputAction.CallbackContext context_)
        {
            var _currentSelected = EventSystem.current.currentSelectedGameObject;
            if(_currentSelected == null || _currentSelected.activeInHierarchy) return;

            SelectLastSelectable();
        }

        public void SelectLastSelectable()
        {
            if (lootMenuItemList.Count == 0)
            {
                lootDropObject.gameObject.ReturnInstance();
                CloseMenu();
                return;
            }
            
            lastItemIndex = lastItemIndex <= 0  ? 0 : lastItemIndex;
            lastItemIndex = lastItemIndex >= lootMenuItemList.Count - 1 ? lootMenuItemList.Count - 1 : lastItemIndex;
            EventSystem.current.SetSelectedGameObject(lootMenuItemList[lastItemIndex].gameObject);
        }
        
        private void LootAll()
        {
            if (lootMenuItemList.Count <= 0)
            {
                CloseMenu();
                return;
            }

            for (var i = lootMenuItemList.Count - 1; i > -1; --i)
            {
                Loot(lootMenuItemList[i]);
            }
        }

        private void TrashAll()
        {
            if (lootMenuItemList.Count <= 0)
            {
                CloseMenu();
                return;
            }
                
            for (var i = lootMenuItemList.Count - 1; i > -1; --i)
            {
                TrashItem(lootMenuItemList[i]);
            }
        }
        
        public bool HasFreeSpace()
        {
            return playerInventory.hasFreeSlot;
        }

        public void OnNotifBtnClicked()
        {
            notifPanel.SetActive(false);
        }
    }
}