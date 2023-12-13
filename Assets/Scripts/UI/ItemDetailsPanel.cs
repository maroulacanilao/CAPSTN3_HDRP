using System;
using Character;
using CustomHelpers;
using Items;
using Items.Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ItemDetailsPanel : MonoBehaviour
    {
        [NaughtyAttributes.BoxGroup("Panels")]
        [SerializeField] protected GameObject namePanel;
        [NaughtyAttributes.BoxGroup("Panels")]
        [SerializeField] protected GameObject descriptionPanel;

        [NaughtyAttributes.BoxGroup("Stats Text")]
        [SerializeField] protected StatsInfo statsPanel;

        [NaughtyAttributes.BoxGroup("Info Text")]
        [SerializeField] protected TextMeshProUGUI nameTxt;
        [NaughtyAttributes.BoxGroup("Info Text")] 
        [SerializeField] protected TextMeshProUGUI typeTxt; 
        [NaughtyAttributes.BoxGroup("Info Text")]
        [SerializeField] protected TextMeshProUGUI rarityTxt;
        [NaughtyAttributes.BoxGroup("Info Text")]
        [SerializeField] protected TextMeshProUGUI descriptionTxt;

        [NaughtyAttributes.BoxGroup("Info Text")]
        [SerializeField] protected TextMeshProUGUI ItemTypeRarityTxt;

        [NaughtyAttributes.BoxGroup("Icon")]
        [SerializeField] protected Image itemIcon;
        
        protected Item currItem;

        [SerializeField] private PlayerInventory playerInventory;
        public PlayerInventory Inventory => playerInventory;


        private void Awake()
        {
            DisplayNull();
        }

        public virtual void DisplayItem(Item item)
        {
            if (item == null)
            {
                DisplayNull();
                return;
            }
            currItem = item;
            var _data = item.Data;
            itemIcon.color = Color.white;
            
            // namePanel.SetActive(currItem.ItemType != ItemType.Gold);
            statsPanel.gameObject.SetActive(currItem.ItemType != ItemType.Gold);
            descriptionPanel.SetActive(currItem.ItemType != ItemType.Gold);

            if (currItem.ItemType == ItemType.Gold)
            {
                var gold = (ItemGold) currItem;
                return;
            }

            if (nameTxt != null) nameTxt.SetText(_data.ItemName);
            if (typeTxt != null) typeTxt.SetText(currItem.ItemType.ToString());
            if (rarityTxt != null) rarityTxt.SetText(currItem.RarityType.GetColoredText());

            if (ItemTypeRarityTxt != null) ItemTypeRarityTxt.text = $"{currItem.RarityType.GetColoredText()} {currItem.ItemType}";

            if (descriptionTxt != null) descriptionTxt.SetText(_data.Description.Beautify());
            if (itemIcon != null) itemIcon.sprite = _data.Icon;

            //if (currItem != null && currItem is ItemGear _gear)
            //{
            //    statsPanel.DisplayDynamic(_gear.Stats, false);
            //    nameTxt.SetText($"{_gear.Data.ItemName} - Lv.{_gear.Level}");
            //}
            //else statsPanel.gameObject.SetActive(false);

            if (currItem != null && currItem is ItemArmor _armor)
            {
                var _oldStats = Inventory.ArmorEquipped?.Stats ?? new CombatStats();
                statsPanel.DisplayDiffDynamic(_armor.Stats, _oldStats, false);
                nameTxt.SetText($"{_armor.Data.ItemName} - Lv.{_armor.Level}");
            }
            else if (currItem != null && currItem is ItemWeapon _weapon)
            {
                var _oldStats = Inventory.WeaponEquipped?.Stats ?? new CombatStats();
                statsPanel.DisplayDiffDynamic(_weapon.Stats, _oldStats, false);
                nameTxt.SetText($"{_weapon.Data.ItemName} - Lv.{_weapon.Level}");
            }
            else statsPanel.gameObject.SetActive(false);
        }

        public  virtual void DisplayNull()
        {
            if (nameTxt != null) nameTxt.SetText("No Item Selected");
            if (typeTxt != null) typeTxt.SetText("???");
            if (rarityTxt != null) rarityTxt.SetText("???");
            if (descriptionTxt != null) descriptionTxt.SetText("???");
            if (itemIcon != null)
            {
                itemIcon.sprite = null;
                itemIcon.color = Color.clear;
            }
            if (ItemTypeRarityTxt != null) ItemTypeRarityTxt.SetText("???");
            // statsPanel.DisplayDynamic(new CombatStats(), false);
            statsPanel.gameObject.SetActive(false);
            
        }
    }
}
