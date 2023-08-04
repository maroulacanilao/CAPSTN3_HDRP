using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AYellowpaper.SerializedCollections;
using CustomHelpers;
using Farming;
using Items.Inventory;
using Managers;
using ScriptableObjectData;
using Unity.VisualScripting;
using UnityEngine;

namespace SaveSystem
{
    public static class SaveHelper
    {
        public static InventorySaveData SaveInventoryData(PlayerInventory inventory_)
        {
            var _count = inventory_.ItemStorage.Count;
            var _storageItems = new List<InventorySlotSaveData>(); 

            var _i = 0;
            foreach (var storagePair in inventory_.ItemStorage)
            {
                var _storageItem = new ItemSaveData
                {
                    dataID = storagePair.Value.Data.ItemID,
                    stats = storagePair.Value.Stats,
                    level = storagePair.Value.Level,
                    stackCount = storagePair.Value.StackCount,
                    itemType = storagePair.Value.ItemType,
                };

                var _inventorySaveStorage = new InventorySlotSaveData()
                {
                    itemSaveData = _storageItem,
                    slot = storagePair.Key
                };
                _storageItems.Add(_inventorySaveStorage);
            }
            
            var _toolItems = new ItemSaveData[inventory_.ItemTools.Length];

            for (var _index = 0; _index < _toolItems.Length; _index++)
            {
                var inventoryTool = inventory_.ItemTools[_index];
                
                if(inventoryTool == null) continue;
                
                _toolItems[_index] = new ItemSaveData
                {
                    dataID = inventoryTool.Data.ItemID,
                    stats = inventoryTool.Stats,
                    level = inventoryTool.Level,
                    stackCount = inventoryTool.StackCount,
                    itemType = inventoryTool.ItemType,
                };
            }
            
            ItemSaveData _weaponItem = null;

            if (inventory_.WeaponEquipped != null)
            {
                _weaponItem = new ItemSaveData
                {
                    dataID = inventory_.WeaponEquipped.Data.ItemID,
                    stats = inventory_.WeaponEquipped.Stats,
                    level = inventory_.WeaponEquipped.Level,
                    stackCount = inventory_.WeaponEquipped.StackCount,
                    itemType = inventory_.WeaponEquipped.ItemType,
                };
            }
            
            ItemSaveData _armorItem = null;
            
            if(inventory_.ArmorEquipped != null)
            {
                _armorItem = new ItemSaveData
                {
                    dataID = inventory_.ArmorEquipped.Data.ItemID,
                    stats = inventory_.ArmorEquipped.Stats,
                    level = inventory_.ArmorEquipped.Level,
                    stackCount = inventory_.ArmorEquipped.StackCount,
                    itemType = inventory_.ArmorEquipped.ItemType,
                };
            }
            
            var _inventorySaveData = new InventorySaveData
            {
                storageItems = _storageItems.ToArray(),
                toolItems = _toolItems,
                weaponItem = _weaponItem,
                armorItem = _armorItem,
                gold = inventory_.Gold.GoldAmount,
            };

            return _inventorySaveData;
        }

        public static FarmTileSaveData[] SaveAllFarmTiles(FarmTileManager manager_)
        {
            var _farmTileSaveData = new List<FarmTileSaveData>();
            foreach (var _tile in manager_.farmTiles)
            {
                var _data = SaveFarmTile(_tile);
                if(_data == null) continue;
                _farmTileSaveData.Add(_data);
            }
            
            return _farmTileSaveData.ToArray();
        }

        public static FarmTileSaveData SaveFarmTile(FarmTile tile_)
        {
            if(tile_.IsEmptyOrDestroyed()) return null;
            var _data = new FarmTileSaveData
            {
                position = tile_.transform.position.ToString(),
                rotation = tile_.transform.rotation.ToString(),
                tileState = tile_.tileState,
                seedDataID = tile_.seedData.ItemID,
                datePlanted = tile_.datePlanted.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture),
                minutesRemaining = tile_.timeRemaining.TotalMinutes.ToString(CultureInfo.InvariantCulture),
            };

            return _data;
        }
        
        
        public static PlayerSaveData SavePlayerProgress(GameDataBase gameDataBase_)
        {
            var _inventorySaveData = SaveInventoryData(gameDataBase_.playerInventory);
            var _statsBought = gameDataBase_.statShopData.statsBought;
            var _exp = gameDataBase_.playerData.LevelData.TotalExperience;
            var _level = gameDataBase_.playerData.LevelData.CurrentLevel;

            var _enemyKills = new List<PlayStatsSaveData>();

            foreach (var _enemyKillStat in gameDataBase_.enemyDataBase.enemyKillsStats)
            {
                var _enemyKill = new PlayStatsSaveData
                {
                    key = _enemyKillStat.Key.characterName,
                    value = _enemyKillStat.Value
                };
                _enemyKills.Add(_enemyKill);
            }
            
            var _cropStats = new List<PlayStatsSaveData>();

            foreach (var _cropHarvestStat in gameDataBase_.cropDataBase.cropHarvestStats)
            {
                var _harvestStat = new PlayStatsSaveData()
                {
                    key = _cropHarvestStat.Key.ItemID,
                    value = _cropHarvestStat.Value
                };
                _cropStats.Add(_harvestStat);
            }
            
            var _spellIdList = gameDataBase_.playerData.spells.Select(spell_ => spell_.spellName).ToArray();


            return new PlayerSaveData
            {
                inventorySaveData = _inventorySaveData,
                statsBought = _statsBought,
                totalExperience = _exp,
                level = _level,
                enemyKills = _enemyKills.ToArray(),
                spells = _spellIdList,
                cropHarvested = _cropStats.ToArray(),
            };
        }

        public static SaveData SaveProgress(GameDataBase gameDataBase_)
        {
            var _progressionData = gameDataBase_.progressionData;
            var _playerSaveData = SavePlayerProgress(gameDataBase_);
            //var _farmTileSaveData = SaveAllFarmTiles(FarmTileManager.Instance);
            
            var _dayCounter = _progressionData.dayCounter;
            var _dungeonLevel = _progressionData.highestDungeonLevel;
            var _timeOfDay = TimeManager.DateTime.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture);
            
            var _saveData = new SaveData
            {
                playerSaveData = _playerSaveData,
                dayCounter = _dayCounter,
                dungeonLevel = _dungeonLevel,
                timeOfDay = _timeOfDay
            };
            
            return _saveData;
        }
    }
}