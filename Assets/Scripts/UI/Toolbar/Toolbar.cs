using System;
using Items.Inventory;
using UnityEngine;

namespace UI.Toolbar
{
    public class Toolbar : MonoBehaviour
    {
        [SerializeField] private PlayerInventory inventory;
        [SerializeField] private ToolUI[] toolUIs;

        private void Awake()
        {
            for (var i = 0; i < toolUIs.Length; i++)
            {
                toolUIs[i].Initialize(inventory, i);
            }
        }
    }
}
