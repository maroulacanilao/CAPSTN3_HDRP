using System;
using System.Collections;
using Character;
using CustomHelpers;
using Items;
using Items.Inventory;
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
        
        [SerializeField] private TextMeshProUGUI errorTxt;

        private UI_LootMenuItem currLootMenuItem;
        private UI_LootMenu lootMenu;
        private PlayerInventory inventory => lootMenu.inventory;

        [SerializeField] private GameObject notifPanel;

        public void Initialize(UI_LootMenu lootMenu_)
        {
            lootBtn.onClick.AddListener((() => lootMenu_.Loot(currLootMenuItem)));
            trashBtn.onClick.AddListener((() => lootMenu_.TrashItem(currLootMenuItem)));
            lootMenu = lootMenu_;
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

            trashBtn.interactable = currItem.IsDiscardable && lootMenu.HasFreeSpace();

            lootBtn.gameObject.SetActive(true);
            trashBtn.gameObject.SetActive(true);
            errorTxt.gameObject.SetActive(false);
            notifPanel.SetActive(false);

            if (!lootMenu.HasFreeSpace())
            {
                if (errorTxt != null)
                {
                    errorTxt.gameObject.SetActive(true);
                    errorTxt.SetText("No space in inventory.");
                }
            }
            if (!currItem.IsDiscardable)
            {
                if (errorTxt != null)
                {
                    errorTxt.gameObject.SetActive(true);
                    errorTxt.SetText("This item cannot be trashed.");
                }
            }

            DisplayItem(currItem);
            
            if (currItem is ItemArmor _armor)
            {
                // statsPanel.Display(_gear.Stats, false);
                // nameTxt.SetText($"{_gear.Data.ItemName} - Lv.{_gear.Level}");
                
                var _oldStats = inventory.ArmorEquipped?.Stats ?? new CombatStats();
                statsPanel.DisplayDiffDynamic(_armor.Stats, _oldStats,false);
                nameTxt.SetText($"{_armor.Data.ItemName} - Lv.{_armor.Level}");
            }
            else if (currItem is ItemWeapon _weapon)
            {
                var _oldStats = inventory.WeaponEquipped?.Stats ?? new CombatStats();
                statsPanel.DisplayDiffDynamic(_weapon.Stats, _oldStats,false);
                nameTxt.SetText($"{_weapon.Data.ItemName} - Lv.{_weapon.Level}");
            }
            else statsPanel.gameObject.SetActive(false);
        }
    }
}
