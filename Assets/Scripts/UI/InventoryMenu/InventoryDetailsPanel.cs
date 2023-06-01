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
        
        [NaughtyAttributes.BoxGroup("Panels")]
        [SerializeField] private InventoryActionButtonGroup actionButtonGroup; 

        [NaughtyAttributes.BoxGroup("Info Text")]
        [SerializeField] private TextMeshProUGUI nameTxt, typeTxt, rarityTxt, valueTxt, descriptionTxt;
        
        [NaughtyAttributes.BoxGroup("Stats Text")]
        [SerializeField] private TextMeshProUGUI maxHpTxt, maxMpTxt, wpnDmgTxt, armValTxt, magDmgTxt, magResTxt, accTxt, speedTxt;

        [NaughtyAttributes.BoxGroup("Buttons")]
        [SerializeField] private Button consumeBtn, equipBtn, unequipBTN,discardBtn;

        [NaughtyAttributes.BoxGroup("Icon")]
        [SerializeField] private Image itemIcon;
        
        [SerializeField] [NaughtyAttributes.Tag] private string inventoryItemTag;
        
        private Item_MenuItem currMenuItem;
        private Item currItem;
        
        private InventoryMenu inventoryMenu;
        private PlayerInventory inventory;

        public void Initialize(InventoryMenu inventoryMenu_)
        {
            inventoryMenu = inventoryMenu_;
            inventory = inventoryMenu.Inventory;
            actionButtonGroup.Initialize();
            
            InventoryMenu.OnInventoryItemSelect.AddListener(ShowItemDetail);
            InventoryEvents.OnUpdateInventory.AddListener(UpdateInventoryWrapper);
            
            consumeBtn.onClick.AddListener(ConsumedItem);
            equipBtn.onClick.AddListener(EquipItem);
            discardBtn.onClick.AddListener(DiscardItem);
            unequipBTN.onClick.AddListener(UnEquipItem);
        }

        public void OnDestroy()
        {
            InventoryMenu.OnInventoryItemSelect.RemoveListener(ShowItemDetail);
            InventoryEvents.OnUpdateInventory.RemoveListener(UpdateInventoryWrapper);
        }

        private void UpdateInventoryWrapper(PlayerInventory inventory_) => ShowItemDetail(null);

        public void ShowItemDetail(SelectableMenuButton selectedObject_)
        {
            if (selectedObject_ == null)
            {
                gameObject.SetActive(false);
                return;
            }
            
            actionButtonGroup.gameObject.SetActive(false);

            if(!selectedObject_.TryGetComponent(out Item_MenuItem _menuItem)) return;

            if (_menuItem == null || _menuItem.item == null)
            {
                gameObject.SetActive(false);
                return;
            }
            
            currMenuItem = _menuItem;
            currItem = _menuItem.item;
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
        
            valueTxt.SetText($"Value: {_data.SellValue} gold");
            nameTxt.SetText(_data.ItemName);
            typeTxt.SetText(currItem.ItemType.ToString());
            rarityTxt.SetText(currItem.RarityType.ToString());
            descriptionTxt.SetText(_data.Description);
            itemIcon.sprite = _data.Icon;

            if (currItem is ItemGear _gear) SetStatsText(_gear);
            else statsPanel.SetActive(false);
            
            consumeBtn.gameObject.SetActive(currItem is ItemConsumable);
            
            var _canEquip = (currItem is ItemGear && !currItem.IsGearEquipped) ||
                            (currItem.IsToolable && currMenuItem.inventoryItemType == Item_MenuItem.InventoryItemType.storage 
                             && inventory.HasFreeSlotInToolBar());

            equipBtn.gameObject.SetActive(_canEquip);
            
            unequipBTN.gameObject.SetActive(currItem is ItemGear && currItem.IsGearEquipped);
            discardBtn.gameObject.SetActive(currItem.IsDiscardable);
            
            actionButtonGroup.gameObject.SetActive(true);
            
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

            switch (currMenuItem.inventoryItemType)
            {
                case Item_MenuItem.InventoryItemType.storage:
                    inventory.RemoveItemInStorage(_index);
                    break;
                case Item_MenuItem.InventoryItemType.toolBar:
                    inventory.RemoveItemInHand(currMenuItem.index);
                    break;
                case Item_MenuItem.InventoryItemType.weaponBar:
                    inventory.DiscardEquippedWeapon();
                    break;
                case Item_MenuItem.InventoryItemType.armorBar:
                    inventory.DiscardEquippedArmor();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            currMenuItem.UpdateDisplay();
        }
        
        private void EquipItem()
        {
            if(currMenuItem.inventoryItemType is not Item_MenuItem.InventoryItemType.storage) return;

            switch (currItem.ItemType)
            {
                case ItemType.Weapon:
                    inventory.EquipWeapon(currMenuItem.index);
                    return;
                case ItemType.Armor:
                    inventory.EquipArmor(currMenuItem.index);
                    return;
            }

            if (currItem.IsToolable)
            {
                inventory.EquipTool(currMenuItem.index);
            }
        }

        private void UnEquipItem()
        {
            switch (currItem.ItemType)
            {
                case ItemType.Weapon:
                    inventory.UnEquipWeapon();
                    break;
                case ItemType.Armor:
                    inventory.UnEquipArmor();
                    break;
                default:
                    break;
            }
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
