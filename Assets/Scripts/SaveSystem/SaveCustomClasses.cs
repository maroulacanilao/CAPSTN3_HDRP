using System;
using AYellowpaper.SerializedCollections;
using Character;
using Farming;
using Items;
using UnityEngine;

namespace SaveSystem
{
    [Serializable]
    public class ItemSaveData
    {
        public string dataID;
        public CombatStats stats;
        public int level = 1;
        public int stackCount;
        public ItemType itemType;
    }
    
    [Serializable]
    public class InventorySlotSaveData
    {
        public int slot;
        public ItemSaveData itemSaveData;
    }
    
    [Serializable]
    public class InventorySaveData
    {
        public InventorySlotSaveData[] storageItems;
        public ItemSaveData[] toolItems;
        public ItemSaveData weaponItem;
        public ItemSaveData armorItem;
        public int gold;
    }
    
    [Serializable]
    public class FarmTileSaveData
    {
        public string position;
        public string rotation; 
        public TileState tileState;
        public string seedDataID;
        public string datePlanted;
        public string minutesRemaining;
    }
    
    [Serializable]
    public class PlayStatsSaveData
    {
        public string key;
        public int value;
    }

    [Serializable]
    public class PlayerSaveData
    {
        public InventorySaveData inventorySaveData;
        public string[] spells;

        public int totalExperience;
        public int level;

        public CombatStats statsBought;
        public PlayStatsSaveData[] enemyKills;
        public PlayStatsSaveData[] cropHarvested;
    }
    
    [Serializable]
    public class SaveData
    {
        public PlayerSaveData playerSaveData;
        public FarmTileSaveData[] farmTileSaveData;

        public int dayCounter;
        public int dungeonLevel;
        public string timeOfDay;
    }
}