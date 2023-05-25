using System.Collections.Generic;
using CustomEvent;
using Items;
using Items.Inventory;
using ObjectPool;
using CustomHelpers;
using UnityEngine;
using UnityEngine.UI;

namespace UI.LootMenu
{
    public class UI_LootMenu : FarmUI
    {
        [SerializeField] private UI_LootMenuItem lootMenuItemPrefab;
        [SerializeField] private PlayerInventory playerInventory;
        [SerializeField] private UI_LootMenuDetailsPanel detailsPanel;

        [Header("Parents")]
        [SerializeField] private Transform itemParent;

        [Header("Buttons")]
        [SerializeField] private Button lootAllBtn, trashAllBtn;

        public static readonly Evt<UI_LootMenuItem> OnShowItemDetail = new Evt<UI_LootMenuItem>();

        private List<UI_LootMenuItem> lootMenuItemList;
        private LootDropObject lootDropObject;

        #region UnityEvents
        
        public override void Initialize()
        {
            LootDropObject.OnLootInteract.AddListener(ShowLootMenu);
            detailsPanel.Initialize(this);
            
            lootAllBtn.onClick.AddListener(() =>
            {
                if (lootMenuItemList.Count <= 0)
                {
                    ClosePanel();
                    return;
                }
                
                for (var i = lootMenuItemList.Count - 1; i > -1; --i)
                {
                    Loot(lootMenuItemList[i]);
                }
            });
            
            trashAllBtn.onClick.AddListener(() =>
            {
                if (lootMenuItemList.Count <= 0)
                {
                    ClosePanel();
                    return;
                }
                
                for (var i = lootMenuItemList.Count - 1; i > -1; --i)
                {
                    RemoveMenuItem(lootMenuItemList[i]);
                }
            });
        }
        
        public override void OpenMenu()
        {
            
        }

        private void OnDestroy()
        {
            LootDropObject.OnLootInteract.RemoveListener(ShowLootMenu);
        }

        private void OnEnable()
        {
            Cursor.visible = true;
            OnShowItemDetail.AddListener(ShowItemDetail);
        }

        private void OnDisable()
        {
            Cursor.visible = false;
            OnShowItemDetail.RemoveListener(ShowItemDetail);
        }

        #endregion

        private void ShowLootMenu(LootDropObject lootDropObject_)
        {
            FarmUIManager.Instance.CloseAllUI();
            
            lootDropObject = lootDropObject_;
            var _lootDrop = lootDropObject_.lootDrop;
            
            lootMenuItemList = new List<UI_LootMenuItem>();
        
            var _menuItem = Instantiate(lootMenuItemPrefab, itemParent).Initialize(_lootDrop.moneyDrop, null);
            lootMenuItemList.Add(_menuItem);
        
            foreach (var _item in _lootDrop.itemsDrop)
            {
                var _uiLootMenuItem = Instantiate(lootMenuItemPrefab, itemParent).Initialize(_item, null);
                lootMenuItemList.Add(_uiLootMenuItem);
            }
            lootMenuItemList[0].GetComponent<Button>().Select();
            gameObject.SetActive(true);
        }

        private void ShowItemDetail(UI_LootMenuItem lootMenuItem_)
        {
            detailsPanel.ShowItemDetail(lootMenuItem_);
        }

        public void Loot(UI_LootMenuItem lootMenuItem_)
        {
            // playerInventory.AddItem(lootMenuItem_.item);
            if(!playerInventory.AddItem(lootMenuItem_.item)) return;
            RemoveMenuItem(lootMenuItem_);
        }
        
        public void RemoveMenuItem(UI_LootMenuItem lootMenuItem_)
        {
            lootMenuItemList.Remove(lootMenuItem_);
            lootDropObject.lootDrop.itemsDrop.Remove(lootMenuItem_.item);
            Destroy(lootMenuItem_.gameObject);
            detailsPanel.gameObject.SetActive(false);
            
            if(lootMenuItemList.Count > 0) return;
            
            // if there is no more loot
            lootDropObject.gameObject.ReturnInstance();
            ClosePanel();
        }

        public void ClosePanel()
        {
            lootDropObject = null;
            if(lootMenuItemList.Count > 0) lootMenuItemList.DestroyComponents();
            FarmUIManager.Instance.CloseAllUI();
        }
    }
}