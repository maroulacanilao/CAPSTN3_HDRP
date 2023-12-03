using Autotiles3D;
using CustomEvent;
using CustomHelpers;
using Items;
using Items.Inventory;
using Items.ItemData.Tools;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Toolbar
{
    public class ToolUI : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] Sprite selectedSprite, deselectedSprite;
        [SerializeField] private Outline outline;
        [SerializeField] private Image toolIcon;
        [SerializeField] private TextMeshProUGUI count_TXT;
        [SerializeField] private Color selectColor, deselectColor;
        [SerializeField] private Sprite[] hotkeySprites;
        [SerializeField] private Image currentHotkeyImage;

        private PlayerInventory inventory;
        private Image background;
        private int index;
        private Item currItem;

        [SerializeField] private GameObject wateringCanRefillBar;

        public void Initialize(PlayerInventory playerInventory_, int index_)
        {
            wateringCanRefillBar.SetActive(false);
            inventory = playerInventory_;
            index = index_;
            background = GetComponent<Image>();
            PlayerEquipment.OnChangeItemOnHand.AddListener(ChangeItem);
            InventoryEvents.OnItemOnHandUpdate.AddListener(UpdateToolBar);
            InventoryEvents.OnUpdateStackable.AddListener(UpdateStackable);
            InventoryEvents.OnUpdateInventory.AddListener(UpdateInventory);
            currItem = inventory.ItemTools[index];
            UpdateToolBar(index, inventory.ItemTools[index]);
            // hotKey_TXT.text = $"{index + 1}";
            currentHotkeyImage.sprite = hotkeySprites[index];
        }

        private void OnDestroy()
        {
            PlayerEquipment.OnChangeItemOnHand.RemoveListener(ChangeItem);
            InventoryEvents.OnItemOnHandUpdate.RemoveListener(UpdateToolBar);
            InventoryEvents.OnUpdateStackable.RemoveListener(UpdateStackable);
            InventoryEvents.OnUpdateInventory.RemoveListener(UpdateInventory);
        }

        private void ChangeItem(int index_)
        {
            outline.effectColor = index == index_ ? selectColor : deselectColor;
            // currentHotkeyImage.color = index == index_ ? selectColor : Color.black;
            background.sprite = index == index_ ? selectedSprite : deselectedSprite;
        }
        
        private void UpdateToolBar(int index_, Item item_)
        {
            if(this.IsEmptyOrDestroyed()) return;
            if (index_ != index) return;
            if (item_ is {IsStackable: true})
            {
                count_TXT.gameObject.SetActive(true);
                count_TXT.text = $"{item_.StackCount}x";
            }
            else count_TXT.gameObject.SetActive(false);
            toolIcon.sprite = item_?.Data.Icon;
            currItem = item_;
            
            if (item_ == null || toolIcon.sprite == null)
            {
                toolIcon.color = Color.clear;
                wateringCanRefillBar.SetActive(false);
            }
            else
            {
                toolIcon.color = Color.white;
            }

            if (currItem != null)
            {
                if (currItem.Data is WateringCanData wateringCanData)
                {
                    wateringCanRefillBar.SetActive(true);
                    PlayerEquipment.OnRefillAction.Invoke(wateringCanData.MaxUsages);
                    PlayerEquipment.OnRefillReduced.Invoke(wateringCanData.CurrentUsage);
                }
                else
                {
                    wateringCanRefillBar.SetActive(false);
                }
            }
        }

        private void UpdateStackable(ItemStackable StackableItem_)
        {
            if(currItem != StackableItem_ as Item) return;
            UpdateToolBar(index, currItem);
        }

        private void UpdateInventory(PlayerInventory inventory_)
        {
            currItem = inventory.ItemTools[index];
            UpdateToolBar(index, inventory.ItemTools[index]);
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            PlayerEquipment.OnManualSelect.Invoke(index);
        }
    }
}
