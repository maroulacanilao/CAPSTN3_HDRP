using Items.ItemData;
using TMPro;
using Trading;
using UnityEngine;
using UnityEngine.UI;

namespace UI.RequestBoardUI
{
    public class RequestItemLabel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI itemNameTxt, itemQuantityTxt;
        [SerializeField] private Image itemIcon;

        public RequestItemLabel Initialize(ItemData itemData_, int amount)
        {
            itemNameTxt.SetText(itemData_.ItemName);
            itemQuantityTxt.SetText(amount + "x");
            itemIcon.sprite = itemData_.Icon;
            return this;
        }
    }
}
