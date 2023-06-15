using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Battle
{
    public class ItemDetailsPanel : MonoBehaviour
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
            UseItemActionUI.CurrentItemBtn = itemBtn;
            
            var _count = itemBtn.item.StackCount;
            var _data = itemBtn.item.Data;
            
            nameTxt.text = $"{_data.ItemName} x{_count}";
            quantityTXT.text = $"Quantity: {_count}";
            descriptionTxt.text =_data.Description;
        }
    }
}
