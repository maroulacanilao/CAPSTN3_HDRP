using System;
using Items;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.LootMenu
{
    public class UI_LootMenuItem : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, ISelectHandler
    {
        [SerializeField] private TextMeshProUGUI itemName_TXT;
        [SerializeField] private TextMeshProUGUI num_TXT;
        [SerializeField] private Image typeIcon;
        [SerializeField] private Button button;

        public Item item { get; private set; }

        public UI_LootMenuItem Initialize(Item item_, Sprite typeSprite_)
        {

            item = item_;
            typeIcon.sprite = typeSprite_;
            itemName_TXT.SetText(item.Data.ItemName);
            switch (item.ItemType)
            {
                case ItemType.Consumable:
                    var consumable = (ItemConsumable) item_; 
                    num_TXT.SetText($"{consumable.StackCount}x");
                    num_TXT.gameObject.SetActive(true);
                    break;
                case ItemType.Seed:
                    var seed = (ItemSeed) item_; 
                    num_TXT.SetText($"{seed.StackCount}x");
                    num_TXT.gameObject.SetActive(true);
                    break;
                case ItemType.Gold:
                    var Gold = (ItemGold) item_;
                    num_TXT.SetText($"{Gold.GoldAmount}x");
                    num_TXT.gameObject.SetActive(true);
                    break;
                default:
                    num_TXT.gameObject.SetActive(false);
                    break;
            }
            return this;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            button.Select();
        }
    
        public void OnPointerEnter(PointerEventData eventData)
        {
            button.Select();
        }
    
        public void OnPointerExit(PointerEventData eventData)
        {
            
        }
        
        public void OnSelect(BaseEventData eventData)
        {
            UI_LootMenu.OnShowItemDetail.Invoke(this);
        }
    }
}