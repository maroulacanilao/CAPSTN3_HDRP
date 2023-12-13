using System;
using Character;
using CustomHelpers;
using Items;
using Items.Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
// using static UnityEditor.Progress;

namespace UI.TabMenu.InventoryMenu
{
    [DefaultExecutionOrder(3)]
    public class InventoryDetailsPanel : ItemDetailsPanel
    {
        [NaughtyAttributes.BoxGroup("Panels")]
        [SerializeField] private InventoryActionButtonGroup actionButtonGroup;

        [NaughtyAttributes.BoxGroup("Buttons")]
        [SerializeField] private Button consumeBtn, equipBtn, unequipBtn,discardBtn;
        
        [NaughtyAttributes.BoxGroup("Error Text")] 
        [SerializeField] protected TextMeshProUGUI errorTxt;
        
        [NaughtyAttributes.BoxGroup("ConfirmMenu")] 
        [SerializeField] protected DiscardConfirm discardConfirmMenu;

        [NaughtyAttributes.BoxGroup("Info Text")]
        [SerializeField] protected Image countIcon;
        [NaughtyAttributes.BoxGroup("Info Text")]
        [SerializeField] protected TextMeshProUGUI countTxt;

        private Item_MenuItem currMenuItem;

        private InventoryMenu inventoryMenu;
        private PlayerInventory inventory;

        public void Initialize(InventoryMenu inventoryMenu_)
        {
            inventoryMenu = inventoryMenu_;
            inventory = inventoryMenu.Inventory;
            actionButtonGroup.Initialize();

            consumeBtn.onClick.AddListener(ConsumedItem);
            equipBtn.onClick.AddListener(EquipItem);
            discardBtn.onClick.AddListener(DiscardItem);
            unequipBtn.onClick.AddListener(UnEquipItem);

            DisplayNull();
        }

        private void OnEnable()
        {
            discardConfirmMenu.gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            if(!inventoryMenu.gameObject.activeInHierarchy) return;
            
            discardConfirmMenu.gameObject.SetActive(false);
            
            inventoryMenu.SelectLastSelectable();
        }

        public void OnDestroy()
        {
            consumeBtn.onClick.RemoveListener(ConsumedItem);
            equipBtn.onClick.RemoveListener(EquipItem);
            discardBtn.onClick.RemoveListener(DiscardItem);
            unequipBtn.onClick.RemoveListener(UnEquipItem);
        }

        public void ShowItemDetail(Item_MenuItem selectedMenuItem_)
        {
            if (this.IsEmptyOrDestroyed())
            {
                return;
            }
            if(!gameObject.activeInHierarchy) gameObject.SetActive(true);
            
            if (selectedMenuItem_ == null)
            {
                DisplayNull();
                actionButtonGroup.gameObject.SetActive(false);
                return;
            }
            
            currMenuItem = selectedMenuItem_;
            currItem = currMenuItem.item;

            if (currItem == null)
            {
                DisplayNull();
                actionButtonGroup.gameObject.SetActive(false);
                return;
            }
            
            DisplayItem(currItem);

            if (countIcon != null) countIcon.gameObject.SetActive(currItem is { IsStackable: true });

            if (countTxt != null)
            {
                countIcon.gameObject.SetActive(currItem is { IsStackable: true });
                countTxt.gameObject.SetActive(currItem is { IsStackable: true });
                if (countTxt.gameObject.activeSelf) countTxt.SetText($"{currItem.StackCount}");
            }

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
            

            consumeBtn.gameObject.SetActive(currItem is ItemConsumable);
            
            var _canEquip = (currItem is ItemGear && !currItem.IsGearEquipped) ||
                            (currItem.IsToolable && currMenuItem.inventoryItemType == Item_MenuItem.InventoryItemType.storage 
                             && inventory.HasFreeSlotInToolBar());
            
            equipBtn.gameObject.SetActive(_canEquip);
            
            var _canUnEquip = (currItem is ItemGear && currItem.IsGearEquipped) ||
                             (currItem.IsToolable && currMenuItem.inventoryItemType == Item_MenuItem.InventoryItemType.toolBar && inventory.IsAnyOpenSlot());
            
            unequipBtn.gameObject.SetActive(_canUnEquip);
            discardBtn.gameObject.SetActive(currItem.IsDiscardable);

            actionButtonGroup.gameObject.SetActive(true);
        }

        private void DiscardItem()
        {
            if(currItem == null) return;
            if(!currItem.IsDiscardable) return;
            if(currMenuItem == null) return;
            
            discardConfirmMenu.ShowMenu(currMenuItem);
        }
        
        private void EquipItem()
        {
            if(currMenuItem.inventoryItemType is not Item_MenuItem.InventoryItemType.storage) return;
            
            var _level = inventoryMenu.playerData.LevelData.CurrentLevel;
            var _itemName = $"<color=red>{currItem.Data.ItemName}</color>";
            
            if(currItem is ItemWeapon _weapon)
            {
                if (_weapon.Level > _level)
                {
                    errorTxt.gameObject.SetActive(true);
                    errorTxt.SetText("Your level is too low to equip " + _itemName);
                    Debug.Log($"Weapon Level: {_weapon.Level} | Player Level: {_level}");
                    return;
                }
                else inventory.EquipWeapon(currMenuItem.index);
            }
            if(currItem is ItemArmor _armor)
            {
                if (_armor.Level > _level)
                {
                    errorTxt.gameObject.SetActive(true);
                    errorTxt.SetText("Your level is too low to equip " + _itemName);
                    Debug.Log($"Weapon Level: {_armor.Level} | Player Level: {_level}");
                    return;
                }
                else inventory.EquipArmor(currMenuItem.index);
            }

            if (currItem.IsToolable)
            {
                inventory.EquipTool(currMenuItem.index);
            }
            
            currMenuItem.UpdateDisplay();
            ShowItemDetail(currMenuItem);
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
                    if (currMenuItem.inventoryItemType == Item_MenuItem.InventoryItemType.toolBar)
                    {
                        if(!inventory.IsAnyOpenSlot()) break;
                        inventory.UnEquipToolInToolbar(currMenuItem.index);
                    }
                    break;
            }
            
            currMenuItem.UpdateDisplay();
            ShowItemDetail(currMenuItem);
        }
        
        private void ConsumedItem()
        {
            if (currItem is not ItemConsumable _consumable)
            {
                consumeBtn.gameObject.SetActive(false);
                return;
            }
            _consumable.Consume(inventoryMenu.playerData.statusEffectReceiver);
            InventoryEvents.OnUpdateStackable.Invoke(_consumable);
            
            currMenuItem.UpdateDisplay();
            ShowItemDetail(currMenuItem);
        }

        public override void DisplayItem(Item item)
        {
            base.DisplayItem(item);

            if (currMenuItem.inventoryItemType != Item_MenuItem.InventoryItemType.toolBar)
            {
                statsPanel.gameObject.SetActive(false);
            }
        }
    }
}
