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
    public class InventoryMenu : MonoBehaviour
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

        public static readonly Evt<bool> OnOpenInventoryMenu = new Evt<bool>();
        public static readonly Evt<Item_MenuItem> OnItemSelect = new Evt<Item_MenuItem>();

        private void Awake()
        {
            OnOpenInventoryMenu.AddListener(OpenMenu);
            InputManager.OnInventoryMenuAction.AddListener(CloseMenu);
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
            
            inventoryDetailsPanel.Initialize(this);
        }

        private void OnDestroy()
        {
            OnOpenInventoryMenu.RemoveListener(OpenMenu);
            InputManager.OnInventoryMenuAction.RemoveListener(CloseMenu);
        }

        private void OpenMenu(bool willOpen_)
        {
            // if(willOpen_ && !menuPanel.activeSelf)
            // {
            //     // Debug.Log("Open Menu");
            //     // menuPanel.gameObject.SetActive(true);
            // }
            // else if(!willOpen_ && menuPanel.activeSelf)
            // {
            //     // Debug.Log("Close Menu");
            //     // menuPanel.gameObject.SetActive(false);
            //     // Debug.Log(menuPanel.activeSelf);
            // }
            toolBarItems[0].button.Select();
        }

        private void CloseMenu(InputAction.CallbackContext context_)
        {
            if(!gameObject.activeSelf) return;
            if(!context_.started) return;
            
            menuPanel.gameObject.SetActive(!menuPanel.activeSelf);
        }
    }
}