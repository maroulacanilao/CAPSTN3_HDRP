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
        
        [field: SerializeField] public Item_MenuItem[] toolBarItems { get; private set; }
        [field: SerializeField] public Item_MenuItem[] storageItems { get; private set; }
        [field: SerializeField] public Item_MenuItem weaponBar { get; private set; }
        [field: SerializeField] public Item_MenuItem armorBar { get; private set; }
        
        public static readonly Evt<SelectableMenuButton> OnInventoryItemSelect = new Evt<SelectableMenuButton>();
        
        public override void Initialize()
        {
            for (int i = 0; i < toolBarItems.Length; i++)
            {
                toolBarItems[i].Initialize(this,i);
            }
            
            for (int j = 0; j < storageItems.Length; j++)
            {
                storageItems[j].Initialize(this,j);
            }
            armorBar.Initialize(this,0);
            weaponBar.Initialize(this,0);
            
            inventoryDetailsPanel.Initialize(this);
            inventoryDetailsPanel.gameObject.SetActive(false);
        }
        
        public override void OpenMenu()
        {
            FarmUIManager.Instance.lastOpenMenu = this;
        }
    }
}