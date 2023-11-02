using System;
using Character;
using CustomHelpers;
using Items;
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

        [NaughtyAttributes.BoxGroup("Icon")]
        [SerializeField] protected Image itemIcon;
        
        protected Item currItem;

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
            
            nameTxt.SetText(_data.ItemName);
            typeTxt.SetText(currItem.ItemType.ToString());
            rarityTxt.SetText(currItem.RarityType.GetColoredText());
            descriptionTxt.SetText(_data.Description.Beautify());
            itemIcon.sprite = _data.Icon;

            if (currItem is ItemGear _gear)
            {
                statsPanel.DisplayDynamic(_gear.Stats, false);
                nameTxt.SetText($"{_gear.Data.ItemName} - Lv.{_gear.Level}");
            }
            else statsPanel.gameObject.SetActive(false);
        }

        public  virtual void DisplayNull()
        {
            nameTxt.SetText("No Item Selected");
            typeTxt.SetText("???");
            rarityTxt.SetText("???");
            descriptionTxt.SetText("???");
            itemIcon.sprite = null;
            itemIcon.color = Color.clear;
            statsPanel.DisplayDynamic(new CombatStats(), false);
            
        }
    }
}
