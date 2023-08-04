using System;
using System.Collections;
using CustomHelpers;
using Items.ItemData.Tools;
using UnityEngine;

namespace GameTutorial
{
    public class LootingTutorial : MonoBehaviour
    {
        public string lootAllItemMessage, doneLootingMessage;
        public TreasureChest TreasureChest;
        public HoeData HoeData;
        public WateringCanData wateringCanData;

        private void OnDisable()
        {
            if (TreasureChest.IsEmptyOrDestroyed())
            {
                OnDoneLooting();
                return;
            }
            if (TreasureChest.lootDrop.itemsDrop == null)
            {
                OnDoneLooting();
                return;
            }
            if (TreasureChest.lootDrop.itemsDrop.Count <= 0)
            {
                OnDoneLooting();
                return;
            }

            SendToFungus(lootAllItemMessage).StartCoroutine();
        }
        
        

        private void OnDoneLooting()
        {
            EquippingTutorial.HasLooted.Invoke();
            SendToFungus(doneLootingMessage).StartCoroutine();
        }

        IEnumerator SendToFungus(string message_)
        {
            yield return new WaitForSeconds(0.05f);
            Fungus.Flowchart.BroadcastFungusMessage(message_);
        }
    }
}