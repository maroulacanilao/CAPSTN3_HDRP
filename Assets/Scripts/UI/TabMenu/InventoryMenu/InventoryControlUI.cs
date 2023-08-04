using System;
using TMPro;
using UnityEngine;

namespace UI.TabMenu.InventoryMenu
{
    public class InventoryControlUI : MonoBehaviour
    {
        public GameObject swapTxt, cancelTxt;

        private void Update()
        {
            cancelTxt.SetActive(Item_MenuItem.swappingItem != null);
        }
    }
}
