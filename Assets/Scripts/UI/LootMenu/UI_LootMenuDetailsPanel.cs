using System;
using System.Collections;
using CustomHelpers;
using Items;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.LootMenu
{
    public class UI_LootMenuDetailsPanel : MonoBehaviour
    {
        [Header("Panels")]
        [SerializeField] private GameObject namePanel, statsPanel, descriptionPanel;

        [Header("Text")]
        [SerializeField] private TextMeshProUGUI nameTxt, typeTxt, rarityTxt, valueTxt, descriptionTxt;
        
        [Header("Stats Text")]
        [SerializeField] private TextMeshProUGUI maxHpTxt, maxMpTxt, wpnDmgTxt, armValTxt, magDmgTxt, magResTxt, accTxt, speedTxt;

        [Header("Buttons")]
        [SerializeField] private Button lootBtn, trashBtn;

        [Header("Image")]
        [SerializeField] private Image itemIcon;
        
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
                gameObject.SetActive(false);
                return;
            }
            
            currLootMenuItem = lootMenuItem_;
            var _currItem = lootMenuItem_.item;
            var _data = _currItem.Data;
        
            namePanel.SetActive(_currItem.ItemType != ItemType.Gold);
            statsPanel.SetActive(_currItem.ItemType != ItemType.Gold);
            descriptionPanel.SetActive(_currItem.ItemType != ItemType.Gold);

            if (_currItem.ItemType == ItemType.Gold)
            {
                var gold = (ItemGold) _currItem;
                valueTxt.SetText($"Value: {gold.GoldAmount}");
                return;
            }
        
            valueTxt.SetText($"Value: {_data.SellValue}");
            nameTxt.SetText(_data.ItemName);
            typeTxt.SetText(_currItem.ItemType.ToString());
            rarityTxt.SetText(_currItem.RarityType.ToString());
            descriptionTxt.SetText(_data.Description);
            itemIcon.sprite = _data.Icon;

            if (_currItem is ItemGear _gear) SetStatsText(_gear);
            else statsPanel.SetActive(false);
            
            gameObject.SetActive(true);
        }

        private void SetStatsText(ItemGear item_)
        {
            maxHpTxt.SetText($"Add HP: {item_.Stats.maxHp}");
            maxMpTxt.SetText($"Add MP: {item_.Stats.maxMana}");
            wpnDmgTxt.SetText($"Wpn: {item_.Stats.physicalDamage}");
            armValTxt.SetText($"Arm: {item_.Stats.armor}");
            magDmgTxt.SetText($"Mag: {item_.Stats.magicDamage}");
            magResTxt.SetText($"Res: {item_.Stats.magicResistance}");
            accTxt.SetText($"Acc: {item_.Stats.accuracy}");
            speedTxt.SetText($"Spd: {item_.Stats.speed}");
            
            maxHpTxt.gameObject.SetActive(item_.Stats.maxHp != 0);
            maxMpTxt.gameObject.SetActive(item_.Stats.maxMana != 0);
            wpnDmgTxt.gameObject.SetActive(item_.Stats.physicalDamage != 0);
            armValTxt.gameObject.SetActive(item_.Stats.armor != 0);
            magDmgTxt.gameObject.SetActive(item_.Stats.magicDamage != 0);
            magResTxt.gameObject.SetActive(item_.Stats.magicResistance != 0);
            accTxt.gameObject.SetActive(item_.Stats.accuracy != 0);
            speedTxt.gameObject.SetActive(item_.Stats.speed != 0);
        }
    }
}
