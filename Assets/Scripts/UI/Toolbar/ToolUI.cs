using System;
using Items;
using Items.Inventory;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ToolUI : MonoBehaviour
    {
        [SerializeField] private Image toolIcon;
        [SerializeField] private TextMeshProUGUI count_TXT;
        [SerializeField] private Color selectColor, deselectColor;

        private PlayerInventory inventory;
        private Image background;
        private int index;
        private Item currItem;
        public void Initialize(PlayerInventory playerInventory_, int index_)
        {
            inventory = playerInventory_;
            index = index_;
            background = GetComponent<Image>();
            PlayerEquipment.OnChangeItemOnHand.AddListener(ChangeItem);
            InventoryEvents.OnItemOnHandUpdate.AddListener(UpdateToolBar);
            currItem = inventory.ItemTools[index];
            UpdateToolBar(index, inventory.ItemTools[index]);
        }

        private void ChangeItem(int index_)
        {
            background.color = index == index_ ? selectColor : deselectColor;
        }
        
        private void UpdateToolBar(int index_, Item item_)
        {
            if (index_ != index) return;
            if (item_ is {IsStackable: true})
            {
                count_TXT.gameObject.SetActive(true);
                count_TXT.text = $"{item_.StackCount}x";
            }
            else count_TXT.gameObject.SetActive(false);
            toolIcon.sprite = item_?.Data.Icon;
            currItem = item_;
        }
    }
}
