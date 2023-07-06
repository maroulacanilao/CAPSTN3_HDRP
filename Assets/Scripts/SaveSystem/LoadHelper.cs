using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using CustomHelpers;
using Farming;
using Items;
using Items.ItemData;
using Managers;
using ObjectPool;
using ScriptableObjectData;
using UnityEngine;

namespace SaveSystem
{
    public class LoadHelper
    {
        public static InventoryLoadData LoadInventoryData(InventorySaveData inventorySaveData_, GameDataBase gameDataBase_)
        {
            var _itemDatabase = gameDataBase_.itemDatabase;

            var _storageItems = new List<InventorySlotLoadData>();
            
            Debug.Log($"Inv Num: {inventorySaveData_.storageItems.Length}");
            
            foreach (var inventorySlot in inventorySaveData_.storageItems)
            {
                var _itemValue = inventorySlot.itemSaveData;
                if(_itemValue == null) continue;
                
                Debug.Log("Item Value not null");
                
                if(!_itemDatabase.ItemDataDictionary.TryGetValue(_itemValue.dataID, out var _itemData)) continue;
                
                var _item = _itemData.GetItem();
                _item.SetLevel(_itemValue.level);
                _item.SetStats(_itemValue.stats);
                _item.SetStack(_itemValue.stackCount);
                
                Debug.Log($"Item: {_item.Data.ItemName} @ slot {inventorySlot.slot}");
                
                _storageItems.Add(new InventorySlotLoadData
                {
                    slot = inventorySlot.slot,
                    item = _item,
                });
            }
            

            var _toolItems = new Item[inventorySaveData_.toolItems.Length];
            for (int i = 0; i < inventorySaveData_.toolItems.Length; i++)
            {
                var _toolValue = inventorySaveData_.toolItems[i];
                if(_toolValue == null) continue;
                
                if(!_itemDatabase.ItemDataDictionary.TryGetValue(_toolValue.dataID, out var _itemData)) continue;
                
                var _item = _itemData.GetItem();
                _item.SetLevel(_toolValue.level);
                _item.SetStats(_toolValue.stats);
                _item.SetStack(_toolValue.stackCount);
                
                _toolItems[i] = _item;
            }
            
            Item _weaponItem = null;

            if (inventorySaveData_.weaponItem != null && inventorySaveData_.weaponItem.dataID != string.Empty)
            {
                if (_itemDatabase.ItemDataDictionary.TryGetValue(inventorySaveData_.weaponItem.dataID, out var _itemData))
                {
                    _weaponItem = _itemData.GetItem();
                    _weaponItem.SetLevel(inventorySaveData_.weaponItem.level);
                    _weaponItem.SetStats(inventorySaveData_.weaponItem.stats);
                    _weaponItem.SetStack(inventorySaveData_.weaponItem.stackCount);
                }
            }
            
            Item _armorItem = null;
            
            if (inventorySaveData_.armorItem != null && inventorySaveData_.armorItem.dataID != string.Empty)
            {
                if (_itemDatabase.ItemDataDictionary.TryGetValue(inventorySaveData_.armorItem.dataID, out var _itemData))
                {
                    _armorItem = _itemData.GetItem();
                    _armorItem.SetLevel(inventorySaveData_.armorItem.level);
                    _armorItem.SetStats(inventorySaveData_.armorItem.stats);
                    _armorItem.SetStack(inventorySaveData_.armorItem.stackCount);
                }
            }
            
            return new InventoryLoadData
            {
                storageItems = _storageItems,
                toolItems = _toolItems,
                weaponItem = _weaponItem,
                armorItem = _armorItem,
                gold = inventorySaveData_.gold,
            };
        }

        public static void LoadPlayStats(SaveData saveData_, GameDataBase gameDataBase_)
        {
            foreach (var _cropStat in saveData_.playerSaveData.cropHarvested)
            {
                var _itemData = gameDataBase_.itemDatabase.ItemDataDictionary.TryGetValue(_cropStat.key, out var _item) ? _item : null;
                if (_item == null) continue;
                gameDataBase_.cropDataBase.AddHarvest(_itemData as ConsumableData, _cropStat.value);
            }

            foreach (var _enemyKill in saveData_.playerSaveData.enemyKills)
            {
                var _enemyData = gameDataBase_.enemyDataBase.enemyDataDictionary.TryGetValue(_enemyKill.key, out var _enemy) ? _enemy : null;
                if (_enemy == null) continue;
                gameDataBase_.enemyDataBase.AddKills(_enemyData, _enemyKill.value);
            }
        }

        public static void LoadData(SaveData saveData_, GameDataBase gameDataBase_)
        {
            LoadInventoryData(saveData_.playerSaveData.inventorySaveData, gameDataBase_);
            LoadPlayStats(saveData_, gameDataBase_);
            LoadSpells(saveData_, gameDataBase_);
            
            var _statsBought = saveData_.playerSaveData.statsBought;
            gameDataBase_.statShopData.SetBoughtStats(_statsBought);
            
            var _exp = saveData_.playerSaveData.totalExperience;
            gameDataBase_.playerData.LevelData.ResetExperience();
            gameDataBase_.playerData.LevelData.AddExp(_exp);
            
            var _progressData = gameDataBase_.progressionData;
            
            _progressData.dayCounter = saveData_.dayCounter;
            _progressData.highestDungeonLevel = saveData_.dungeonLevel;
        }
        
        public static void LoadFarmTiles(FarmTileSaveData[] farmTileSaveData_, GameDataBase gameDataBase_)
        {
            var _itemDatabase = gameDataBase_.itemDatabase;

            foreach (var _tileData in farmTileSaveData_)
            {
                var _tile = AssetHelper.GetPrefab("FarmTile").GetInstance<FarmTile>();

                var _seedData = _itemDatabase.ItemDataDictionary.TryGetValue(_tileData.seedDataID, out var _itemData) ? _itemData :  null;
                
                _tile.Load(_tileData, _seedData as SeedData);
                FarmTileManager.AddTileManual(_tile);
            }
        }

        public static void LoadSpells(SaveData saveData_, GameDataBase gameDataBase_)
        {
            var _player = gameDataBase_.playerData;
            var _spellIdList = saveData_.playerSaveData.spells;

            var _lookup = gameDataBase_.spellDataBase.spellDataLookup;

            foreach (var _spellID in _spellIdList)
            {
                if(!_lookup.TryGetValue(_spellID, out var _spellData)) continue;
                
                _player.spells.Add(_spellData);
            }
        }
    }
}