using System.Collections;
using CustomEvent;
using CustomHelpers;
using FungusWrapper;
using UnityEngine;

namespace GameTutorial
{
    public class InventoryTutorial : MonoBehaviour
    {
        public bool hasStarted = true;
        public string lootingKey, equippingKey, hoeing;
        public GameObject inventoryPanel;
        
        public static readonly Evt HasLooted = new Evt();
        private void OnDisable()
        {
            if(!TutorialValues.HasOpenedInventory) return;
        
            Co_SendMessage().StartCoroutine();
        }

        private void Update()
        {
            if(!hasStarted) return;

            if (!inventoryPanel.activeInHierarchy) return;
            Debug.Log("<color=blue>Active</color>");
        
            hasStarted = false;
            TutorialValues.HasOpenedInventory = true;
        }

        private IEnumerator Co_SendMessage()
        {
            yield return new WaitForSeconds(0.1f);
            var _messenger = inventoryPanel.AddComponent<OnEnabledMessenger>();
            _messenger.keys.Add(equippingKey);
            _messenger.DestroyOnSend = true;

            Fungus.Flowchart.BroadcastFungusMessage(lootingKey);
        
            yield return new WaitForSecondsRealtime(0.1f);
            var _messenger2 = inventoryPanel.AddComponent<OnDisableMessenger>();
            _messenger2.keys.Add(hoeing);
            _messenger2.DestroyOnSend = true;

            Destroy(this);
        }
    }
}
