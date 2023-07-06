using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Items;

namespace SaveSystem
{
    [Serializable]
    public class InventoryLoadData
    {
        public List<InventorySlotLoadData> storageItems;
        public Item[] toolItems;
        public Item weaponItem;
        public Item armorItem;
        public int gold;
    }
    
    [Serializable]
    public class InventorySlotLoadData
    {
        public int slot;
        public Item item;
    }
}