using System;
using Character;
using CustomEvent;
using Items.Inventory;
using Managers;
using ScriptableObjectData;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI.InventoryMenu
{
    public class InventoryMenu : FarmUI
    {
        [field: Header("Data")]
        [field: SerializeField] public PlayerInventory Inventory { get; private set; }
        [field: SerializeField] public GameDataBase gameDataBase { get; private set; }
        [field: SerializeField] public PlayerCharacter player { get; private set; }
        
        [field: Header("Panels")]
        [field: SerializeField] public Transform ghostIconParent { get; private set; }
        [field: SerializeField] public InventoryDetailsPanel inventoryDetailsPanel { get; private set; }
        [SerializeField] private GameObject menuPanel;
        [SerializeField] Item_MenuItem[] toolBarItems;
        [SerializeField] Item_MenuItem[] storageItems;
        [SerializeField] private Item_MenuItem weaponBar;
        [SerializeField] private Item_MenuItem armorBar;
        
        public static readonly Evt<Item_MenuItem> OnItemSelect = new Evt<Item_MenuItem>();
        
        public override void Initialize()
        {
            for (int i = 0; i < toolBarItems.Length; i++)
            {
                toolBarItems[i].Initialize(this,i);
                // // to remove
                // toolBarItems[i].SetDisplay();
            }
            
            for (int j = 0; j < storageItems.Length; j++)
            {
                storageItems[j].Initialize(this,j);
                // // to remove
                // storageItems[j].SetDisplay();
            }
            armorBar.Initialize(this,0);
            weaponBar.Initialize(this,0);
            inventoryDetailsPanel.Initialize(this);
        }
        
        public override void OpenMenu()
        {
            FarmUIManager.Instance.lastOpenMenu = this;
        }
    }
}