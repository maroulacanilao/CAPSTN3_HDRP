using System.Linq;
using BaseCore;
using Items;
using Items.Inventory;
using Items.ItemData;
using UnityEngine;
using UnityEngine.Events;

namespace Dungeon
{
    public class DungeonBlocker : InteractableObject
    {
        [SerializeField] private PlayerInventory inventory;
        [SerializeField] private QuestItemData questItemData;
        
        [Header("Messages")]
        [SerializeField] private string blockMessage;
        [SerializeField] private string unblockMessage;
        
        [Header("Interact Text")]
        [SerializeField] private string blockText;
        [SerializeField] private string unblockText;

        public UnityEvent onUnlock;
        
        
        protected override void Interact()
        {
            // if has quest item
            if(HasQuestItem())
            {
                if(!string.IsNullOrEmpty(unblockMessage)) Fungus.Flowchart.BroadcastFungusMessage(unblockMessage);
                onUnlock?.Invoke();
                return;
            }
            
            // if it does not have quest item
            if(!string.IsNullOrEmpty(blockMessage)) Fungus.Flowchart.BroadcastFungusMessage(blockMessage);
        }
        
        protected override void Enter()
        {
            interactText = HasQuestItem() ? unblockText : blockText;
        }
        
        protected override void Exit()
        {
            
        }

        protected bool HasQuestItem()
        {
            if (!inventory.itemsLookup.TryGetValue(questItemData, out var _itemList)) return false;
            
            if(_itemList.Count <= 0) return false;
            
            var _questItem = _itemList.Where(i => i is ItemQuest && i.Data.ItemID == questItemData.ItemID).Cast<ItemQuest>().FirstOrDefault();
            
            return _questItem != null;
        }
    }
}
