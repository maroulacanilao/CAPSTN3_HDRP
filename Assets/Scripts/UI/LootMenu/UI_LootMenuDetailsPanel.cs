using System;
using System.Collections;
using CustomHelpers;
using Items;
using ScriptableObjectData;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.LootMenu
{
    public class UI_LootMenuDetailsPanel : ItemDetailsPanel
    {
        [Header("Buttons")]
        [SerializeField] private Button lootBtn, trashBtn;

        private UI_LootMenuItem currLootMenuItem;

        public void Initialize(UI_LootMenu lootMenu_)
        {
            lootBtn.onClick.AddListener((() => lootMenu_.Loot(currLootMenuItem)));
            trashBtn.onClick.AddListener((() => lootMenu_.RemoveMenuItem(currLootMenuItem)));
        }

        private void OnEnable()
        {
            UI_LootMenu.OnItemClick.AddListener(OnClick);
        }

        private void OnDisable()
        {
            UI_LootMenu.OnItemClick.RemoveListener(OnClick);
            currLootMenuItem = null;
            //lootMenu.SelectLastSelectable();
        }

        private void OnClick(UI_LootMenuItem lootMenuItem_)
        {
            ShowItemDetail(lootMenuItem_);
            EventSystem.current.SetSelectedGameObject(lootBtn.gameObject);
        }

        public void ShowItemDetail(UI_LootMenuItem lootMenuItem_)
        {
            if (lootMenuItem_ == null || lootMenuItem_.item == null)
            {
                DisplayNull();
                lootBtn.gameObject.SetActive(false);
                trashBtn.gameObject.SetActive(false);
                return;
            }
            if(!gameObject.activeInHierarchy) gameObject.SetActive(true);
            
            currLootMenuItem = lootMenuItem_;
            currItem = currLootMenuItem.item;

            if (currItem == null)
            {
                gameObject.SetActive(false);
                return;
            }
            
            lootBtn.gameObject.SetActive(true);
            trashBtn.gameObject.SetActive(true);
            
            DisplayItem(currItem);
        }
        
        
    }
}
