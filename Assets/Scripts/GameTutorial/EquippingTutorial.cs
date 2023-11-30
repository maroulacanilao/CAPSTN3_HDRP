using System;
using System.Collections;
using System.Linq;
using CustomEvent;
using CustomHelpers;
using FungusWrapper;
using Items.ItemData;
using Items.ItemData.Tools;
using NaughtyAttributes;
using UI.Farming;
using UI.TabMenu.InventoryMenu;
using UnityEngine;

namespace GameTutorial
{
    [DefaultExecutionOrder(5)]
    public class EquippingTutorial : MonoBehaviour
    {
        [Header("MessageReceive")]
        public string openInventoryMessage = "InventoryTut";

        [Header("MessageSender")]
        public string lootingKey;
        public string equippingKey;
        public string hoeing;
        public string forceEquip;
        public GameObject inventoryTutCanvas;
        private GameObject inventoryMenu;

        public HoeData HoeData;
        public WateringCanData wateringCanData;

        public static readonly Evt HasLooted = new Evt();

        private void Start()
        {
            inventoryMenu = GameObject.FindObjectOfType<InventoryMenu>(true).gameObject;
            Debug.Log(inventoryMenu);
            FungusReceiver.OnReceiveMessage.AddListener(OnReceiveMessage);
            HasLooted.AddListener(OnHasLooted);
        }

        private void OnDestroy()
        {
            FungusReceiver.OnReceiveMessage.RemoveListener(OnReceiveMessage);
            InventoryMenu.OnInventoryMenuClose.RemoveListener(OnStartLootingTut);
            InventoryMenu.OnInventoryMenuOpen.RemoveListener(OnOpenEquippingTut);
            InventoryMenu.OnInventoryMenuClose.RemoveListener(ForceEquip);
            HasLooted.RemoveListener(OnHasLooted);
        }
        
        private void OnReceiveMessage(string obj_)
        {
            if (obj_.ToHash() == openInventoryMessage.ToHash())
            {
                InventoryMenu.OnInventoryMenuClose.AddListener(OnStartLootingTut);
                return;
            }
        }
        private void OnStartLootingTut(InventoryMenu inventoryMenu_)
        {
            StartCoroutine(Co_StartLootTutorial());
        }
        
        bool isLooting = false;
        private IEnumerator Co_StartLootTutorial()
        {
            if(isLooting) yield break;
            isLooting = true;
            yield return null;
            //PlayerMenuManager.OnCloseAllUI.Invoke();
            RevisedPlayerMenuManager.OnCloseAllUIRevised.Invoke();
            yield return null;
            Fungus.Flowchart.BroadcastFungusMessage(lootingKey);
            InventoryMenu.OnInventoryMenuClose.RemoveListener(OnStartLootingTut);
            isLooting = false;
        }
        
        private void OnHasLooted()
        {
            InventoryMenu.OnInventoryMenuOpen.AddListener(OnOpenEquippingTut);
        }
        
        private void OnOpenEquippingTut(InventoryMenu inventoryMenu_)
        {
            inventoryTutCanvas.SetActive(true);
            InventoryMenu.OnInventoryMenuOpen.RemoveListener(OnOpenEquippingTut);
            InventoryMenu.OnInventoryMenuClose.AddListener(ForceEquip);
        }

        private void ForceEquip(InventoryMenu inventoryMenu_)
        {
            StartCoroutine(Co_Force(inventoryMenu_));
        }

        bool isForce = false;   
        IEnumerator Co_Force(InventoryMenu inventoryMenu_)
        {
            if(isForce) yield break;
            isForce = true;
            yield return null;
            var _tools = inventoryMenu_.Inventory.ItemTools;
            if (_tools.Any(t => t != null && t.Data == HoeData)
                && _tools.Any(t => t != null &&  t.Data == wateringCanData)
                && _tools.Any(t =>  t != null && t.Data is SeedData))
            {
                InventoryMenu.OnInventoryMenuClose.RemoveListener(ForceEquip);
                yield return new WaitForSeconds(0.1f);
                Debug.Log("<color=blue>Hoe Start</color>");
                Fungus.Flowchart.BroadcastFungusMessage(hoeing);
                isForce = false;
                yield break;
            }
            yield return new WaitForSeconds(0.1f);
            Debug.Log("<color=red>ForceEquip</color>");
            Fungus.Flowchart.BroadcastFungusMessage(forceEquip);
            isForce = false;
        }
    }
}
