using System;
using CustomHelpers;
using Items;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.LootMenu
{
    public class UI_LootMenuItem : SelectableMenuButton
    {
        [SerializeField] private TextMeshProUGUI itemName_TXT;
        [SerializeField] private TextMeshProUGUI num_TXT;
        [SerializeField] private Image typeIcon;

        public Item item { get; private set; }
        
        public UI_LootMenuItem Initialize(Item item_, Sprite typeSprite_)
        {
            button.onClick.AddListener(OnPointerClick);
            item = item_;
            typeIcon.sprite = item_.Data.Icon;
            itemName_TXT.SetText(item.Data.ItemName);
            switch (item.ItemType)
            {
                case ItemType.Consumable:
                    var consumable = (ItemConsumable) item_; 
                    num_TXT.SetText($"x{consumable.StackCount}");
                    num_TXT.gameObject.SetActive(true);
                    break;
                case ItemType.Seed:
                    var seed = (ItemSeed) item_; 
                    num_TXT.SetText($"x{seed.StackCount}");
                    num_TXT.gameObject.SetActive(true);
                    break;
                case ItemType.Gold:
                    var Gold = (ItemGold) item_;
                    num_TXT.SetText($"x{Gold.GoldAmount}");
                    num_TXT.gameObject.SetActive(true);
                    break;
                default:
                    num_TXT.SetText("");
                    break;
            }
            return this;
        }

        public void OnPointerClick()
        {
            if (EventSystem.current.currentSelectedGameObject == gameObject)
            {
                UI_LootMenu.OnItemClick.Invoke(this);
            }
            else
            {
                button.Select();
                OnSelect(null);
            }

        }

        public override void SelectButton()
        {
            base.SelectButton();
            UI_LootMenu.OnShowItemDetail.Invoke(this);
        }
    }
}