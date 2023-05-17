using System;
using System.Collections;
using System.Collections.Generic;
using BaseCore;
using Character;
using Items;
using Items.ItemData;
using Managers;
using ScriptableObjectData;
using UnityEngine;

public static class ItemHelper
{
    private static GameDataBase GameDataBase;
    private static ItemDatabase ItemDatabase;
    private static PlayerLevel PlayerLevel;
    private static GoldData GoldData;
    private static bool hasInitialized;

    public static void Initialize(GameDataBase gameDataBase_)
    {
        GameDataBase = GameManager.Instance.GameDataBase;
        ItemDatabase = GameDataBase.itemDatabase;
        PlayerLevel = GameDataBase.playerData.playerLevelData;
        GoldData = ItemDatabase.GoldItemData;
    }

    public static Item GetItem(this ItemData data_)
    {
        switch (data_.ItemType)
        {
            case ItemType.Weapon:
            {
                var _weaponData = (WeaponData) data_;
                return _weaponData.GetWeaponItem();
            }
            case ItemType.Armor:
            {
                var _armorData = (ArmorData) data_;
                return _armorData.GetArmorItem();
            }

            case ItemType.Consumable:
            {
                var _consumableData = (ConsumableData) data_;
                return _consumableData.GetConsumableItem();
            }
            case ItemType.Gold:
            {
                return new ItemGold(data_, 0);
            }
            case ItemType.Seed:
            {
                return new ItemSeed(data_, 1);
            }
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    public static ItemWeapon GetWeaponItem(this WeaponData data_)
    {
        var _baseStats = data_.GetRandomStats();
        //var _modifiers = (1f + ItemDatabase.RarityModiferValue[rarityType_]) * (1f + ((float)PlayerLevel.CurrentLevel / PlayerLevel.LevelCap));
        var _modifiers = (1f + ((float)PlayerLevel.CurrentLevel / PlayerLevel.LevelCap));
        var _stats = _baseStats * _modifiers;
        return new ItemWeapon(data_, _stats);
    }

    public static ItemArmor GetArmorItem(this ArmorData data_)
    {
        var _baseStats = data_.GetRandomStats();
        //var _modifiers = (1f + ItemDatabase.RarityModiferValue[rarityType_]) * (1f + ((float)PlayerLevel.CurrentLevel / PlayerLevel.LevelCap));
        var _modifiers = (1f + ((float)PlayerLevel.CurrentLevel / PlayerLevel.LevelCap));
        var _stats = _baseStats * _modifiers;
        return new ItemArmor(data_, _stats);
    }

    public static ItemConsumable GetConsumableItem(this ConsumableData data_, int count_ = 0)
    {
        var _count = count_ <= 0 ? UnityEngine.Random.Range(1, data_.maxPossibleDropCount) : count_;
        return new ItemConsumable(data_, _count);
    }
    
    public static ItemSeed GetSeedItem(this SeedData data_, int count_ = 0)
    {
        var _count = count_ <= 0 ? UnityEngine.Random.Range(1, 5) : count_;
        return new ItemSeed(data_, _count);
    }

    public static ItemGold GetGoldItem(int goldAmount_)
    {
        return new ItemGold(GoldData, goldAmount_);
    }
}
