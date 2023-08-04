using System;
using System.Collections.Generic;
using System.Linq;
using BaseCore;
using Character.CharacterComponents;
using CustomHelpers;
using Farming;
using Items;
using Items.Inventory;
using Items.ItemData;
using Items.ItemData.Tools;
using ScriptableObjectData;
using ScriptableObjectData.CharacterData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.TabMenu
{
    public class CheatMenu : MonoBehaviour
    {
        [SerializeField] private GameDataBase gameDataBase;
        [SerializeField] private List<Button> buttons;

        private PlayerData playerData => gameDataBase.playerData;
        private ItemDatabase itemDatabase => gameDataBase.itemDatabase;
        private PlayerInventory inventory => playerData.inventory;
        private PlayerLevel levelData => playerData.LevelData;
        private PlayerHealth health => playerData.health;
        private PlayerMana mana => playerData.mana;
        private ProgressionData progressionData => gameDataBase.progressionData;

        private void OnEnable()
        {
            foreach (var btn in buttons)
            {
                btn.interactable = true;
            }
        }

        public void GetAllSeeds(Button button)
        {
            var _seedDataList = itemDatabase.ItemDataByType[ItemType.Seed];

            foreach (var _itemData in _seedDataList)
            {
                if(_itemData is not SeedData _seedData) continue;
                var _seed = _seedData.GetSeedItem(99);
                inventory.AddItem(_seed);
            }
            button.interactable = false;
        }
        
        public void GetAllCrops(Button button)
        {
            var _dataList = itemDatabase.ItemDataByType[ItemType.Consumable];

            foreach (var _itemData in _dataList)
            {
                if(_itemData is not ConsumableData _consumableData) continue;
                var _item = _consumableData.GetConsumableItem(99);
                inventory.AddItem(_item);
            }
            button.interactable = false;
        }
        
        public void GetAllWeapons(Button button)
        {
            var _dataList = itemDatabase.ItemDataByType[ItemType.Weapon];

            foreach (var _itemData in _dataList)
            {
                if(_itemData is not WeaponData _weaponData) continue;
                var _item = _weaponData.GetWeaponItem(levelData.CurrentLevel);
                inventory.AddItem(_item);
            }
            button.interactable = false;
        }

        public void GetAllArmors(Button button)
        {
            var _dataList = itemDatabase.ItemDataByType[ItemType.Armor];

            foreach (var _itemData in _dataList)
            {
                if(_itemData is not ArmorData _armorData) continue;
                var _item = _armorData.GetArmorItem(levelData.CurrentLevel);
                inventory.AddItem(_item);
            }
            button.interactable = false;
        }
        
        public void RecoverHealth(Button button)
        {
            health.RefillHealth();
            button.interactable = false;
        }
        
        public void RecoverMana(Button button)
        {
            mana.RefreshMana();
            button.interactable = false;
        }
        
        public void LevelUp(Button button)
        {
            var _exp = levelData.ExperienceNeededToLevelUp + 1;
            levelData.AddExp(_exp);
        }
        
        public void SetInvincible(TextMeshProUGUI text)
        {
            health.IsInvincible = !health.IsInvincible;
            text.text = health.IsInvincible ? "Invincible: On" : "Invincible: Off";
        }
        
        public void UnlockAllDungeon(Button button)
        {
            progressionData.highestDungeonLevel = 5;
            button.interactable = false;
        }

        public void GetAllSpells(Button button)
        {
            var _spellsList = gameDataBase.spellDataBase.spellDataLookup.Values.ToList();

            foreach (var _spell in _spellsList)
            {
                playerData.spells.AddUnique(_spell);
            }
        }

        public void GetAllTools(Button button)
        {
            var _dataList = itemDatabase.ItemDataByType[ItemType.Tool];

            foreach (var _itemData in _dataList)
            {
                if(_itemData is not ToolData _toolData) continue;
                var _item = _toolData.GetItem();
                if (inventory.itemsLookup.TryGetValue(_itemData, out var _list))
                {
                    if(_list.Count > 0 && _list[0] != null) continue;
                }
                inventory.AddItem(_item);
            }
            button.interactable = false;
        }

        public void InstantGrowAllCrops(Button button_)
        {
            var _tiles = FarmTileManager.Instance.farmTiles;

            foreach (var _tile in _tiles)
            {
                if(_tile.seedData == null) continue;
                if(_tile.tileState != TileState.Growing) continue;
                
                _tile.ChangeState(_tile.readyToHarvestTileState);
            }
        }
    }
}
