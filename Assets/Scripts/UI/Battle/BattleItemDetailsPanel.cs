using System;
using CustomHelpers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Battle
{
    public class BattleItemDetailsPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nameTxt, quantityTXT, descriptionTxt;

        public void Initialize()
        {
            UseItemActionUI.OnItemBtnSelect.AddListener(DisplayItemDetails);
        }

        private void OnDestroy()
        {
            UseItemActionUI.OnItemBtnSelect.RemoveListener(DisplayItemDetails);
        }

        public void DisplayItemDetails(ItemActionBtn itemBtn)
        {
            if (this.IsEmptyOrDestroyed())
            {
                UseItemActionUI.OnItemBtnSelect.RemoveListener(DisplayItemDetails);
                return;
            }
            if (itemBtn == null || itemBtn.item == null || itemBtn.item.Data == null)
            {
                gameObject.SetActive(false);
                return;
            }

            UseItemActionUI.CurrentItemBtn = itemBtn;
            
            var _count = itemBtn.item.StackCount;
            var _data = itemBtn.item.Data;
            
            nameTxt.text = $"{_data.ItemName} x{_count}";
            quantityTXT.text = $"Quantity: {_count}";
            descriptionTxt.text =_data.Description;
            
            gameObject.SetActive(true);
        }
    }
}
