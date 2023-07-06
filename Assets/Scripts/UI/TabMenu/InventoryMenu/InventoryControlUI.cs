using System;
using TMPro;
using UnityEngine;

namespace UI.TabMenu.InventoryMenu
{
    public class InventoryControlUI : MonoBehaviour
    {
        public TextMeshProUGUI swapTxt, cancelTxt;

        private void Update()
        {
            cancelTxt.gameObject.SetActive(Item_MenuItem.swappingItem != null);
        }
    }
}
