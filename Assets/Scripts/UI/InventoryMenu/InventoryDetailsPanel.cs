using System;
using Character;
using Items;
using Items.Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InventoryMenu
{
    public class InventoryDetailsPanel : MonoBehaviour
    {
        [NaughtyAttributes.BoxGroup("Panels")]
        [SerializeField] private GameObject namePanel, statsPanel, descriptionPanel, valuePanel;

        [NaughtyAttributes.BoxGroup("Info Text")]
        [SerializeField] private TextMeshProUGUI nameTxt, typeTxt, rarityTxt, valueTxt, descriptionTxt;
        
        [NaughtyAttributes.BoxGroup("Stats Text")]
        [SerializeField] private TextMeshProUGUI maxHpTxt, maxMpTxt, wpnDmgTxt, armValTxt, magDmgTxt, magResTxt, accTxt, speedTxt;

        [NaughtyAttributes.BoxGroup("Buttons")]
        [SerializeField] private Button consumeBtn, equipBtn, discardBtn;

        [NaughtyAttributes.BoxGroup("Icon")]
        [SerializeField] private Image itemIcon;
        
        private Item_MenuItem currMenuItem;
        private Item currItem;
        
        private InventoryMenu inventoryMenu;
        private PlayerInventory inventory;

        public void Initialize(InventoryMenu inventoryMenu_)
        {
            inventoryMenu = inventoryMenu_;
            inventory = inventoryMenu.Inventory;
            
            InventoryMenu.OnItemSelect.AddListener(ShowItemDetail);
            
            consumeBtn.onClick.AddListener(ConsumedItem);
            equipBtn.onClick.AddListener(EquipItem);
            discardBtn.onClick.AddListener(DiscardItem);
        }

        public void OnDestroy()
        {
            InventoryMenu.OnItemSelect.RemoveListener(ShowItemDetail);
        }

        public void ShowItemDetail(Item_MenuItem menuItem_)
        {
            if (menuItem_ == null || menuItem_.item == null)
            {
                gameObject.SetActive(false);
                return;
            }
            
            currMenuItem = menuItem_;
            currItem = menuItem_.item;
            var _data = currItem.Data;
        
            namePanel.SetActive(currItem.ItemType != ItemType.Gold);
            statsPanel.SetActive(currItem.ItemType != ItemType.Gold);
            descriptionPanel.SetActive(currItem.ItemType != ItemType.Gold);
            valuePanel.SetActive(currItem.Data.IsSellable);

            if (currItem.ItemType == ItemType.Gold)
            {
                var gold = (ItemGold) currItem;
                valueTxt.SetText($"Value: {gold.GoldAmount}");
                return;
            }
        
            valueTxt.SetText($"Value: {_data.ItemBaseValue} gold");
            nameTxt.SetText(_data.ItemName);
            typeTxt.SetText(currItem.ItemType.ToString());
            rarityTxt.SetText(currItem.RarityType.ToString());
            descriptionTxt.SetText(_data.Description);
            itemIcon.sprite = _data.Icon;

            if (currItem is ItemGear _gear) SetStatsText(_gear);
            else statsPanel.SetActive(false);
            
            consumeBtn.gameObject.SetActive(currItem is ItemConsumable);
            equipBtn.gameObject.SetActive(currItem is ItemGear);
            discardBtn.gameObject.SetActive(currItem.IsDiscardable);
            
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
        }
        
        private void DiscardItem()
        {
            if(currItem == null) return;
            if(!currItem.IsDiscardable) return;
            if(currMenuItem == null) return;
            
            var _index = currMenuItem.index;
            
            if(currMenuItem.isToolBar) inventory.RemoveItemInHand(currMenuItem.index);
            else inventory.RemoveItemInStorage(_index);
            currMenuItem.UpdateDisplay();
        }
        
        private void EquipItem()
        {
            
        }
        
        private void ConsumedItem()
        {
            if (currItem is not ItemConsumable _consumable)
            {
                consumeBtn.gameObject.SetActive(false);
                return;
            }
            _consumable.Consume(inventoryMenu.player.statusEffectReceiver);
            InventoryEvents.OnUpdateStackable.Invoke(_consumable);
            currMenuItem.UpdateDisplay();
        }
    }
}
