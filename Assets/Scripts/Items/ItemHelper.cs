using System;
using System.Collections;
using System.Collections.Generic;
using BaseCore;
using Character;
using CustomHelpers;
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
        GameDataBase = gameDataBase_;
        ItemDatabase = GameDataBase.itemDatabase;
        PlayerLevel = GameDataBase.playerData.LevelData;
        GoldData = ItemDatabase.GoldItemData;
    }

    public static Item GetItem(this ItemData data_, int level_ = 1)
    {
        switch (data_.ItemType)
        {
            case ItemType.Weapon:
            {
                var _weaponData = (WeaponData) data_;
                return _weaponData.GetWeaponItem(level_);
            }
            case ItemType.Armor:
            {
                var _armorData = (ArmorData) data_;
                return _armorData.GetArmorItem(level_);
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
            case ItemType.Tool:
            {
                return new ItemTool(data_);
            }
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public static ItemGear GetGearItem(this GearData data_, int level = 1)
    {
        if (data_.ItemType == ItemType.Armor && data_ is ArmorData _armorData)
        {
            return _armorData.GetArmorItem(level);
        }
        else if (data_.ItemType == ItemType.Weapon && data_ is WeaponData _weaponData)
        {
            return _weaponData.GetWeaponItem(level);
        }

        return null;
    }
    
    public static ItemWeapon GetWeaponItem(this WeaponData data_, int level_ = 1)
    {
        level_ = Mathf.Clamp(level_, 1, 999);
        var _rarity = ItemDatabase.GetRandomRarity(level_, out var _modifer);
        var _stats = data_.GetRandomStats(_modifer * level_);
        return new ItemWeapon(data_, _stats, level_, _rarity);
    }

    public static ItemArmor GetArmorItem(this ArmorData data_, int level_ = 1)
    {
        level_ = Mathf.Clamp(level_, 1, 999);
        var _rarity = ItemDatabase.GetRandomRarity(level_, out var _modifer);
        var _stats = data_.GetRandomStats(_modifer * level_);
        return new ItemArmor(data_, _stats, level_, _rarity);
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

    // public static float GetDropChance(this RarityType rarityType_, int level_, float rarityIncreasePerLevel_)
    // {
    //     float dropChance = ItemDatabase.RarityChance.GetChanceOfItem(rarityType_);
    //
    //     // Calculate the increase in drop chance based on player level
    //     float rarityIncrease = level_ * rarityIncreasePerLevel_;
    //     
    //     // Apply the increase to the base drop chance
    //     dropChance += rarityIncrease;
    //
    //     return dropChance;
    // }

    public static string ToText(this RarityType rarityType_)
    {
        string _rarityText = "";
        switch (rarityType_)
        {
            case RarityType.Uncommon:
                _rarityText = ("Uncommon").SurroundWithColor(ItemDatabase.RarityColorDictionary[RarityType.Uncommon]);
                break;
            case RarityType.Rare:
                _rarityText = ("Rare").SurroundWithColor(ItemDatabase.RarityColorDictionary[RarityType.Rare]);
                break;
            case RarityType.Epic:
                _rarityText = ("Epic").SurroundWithColor(ItemDatabase.RarityColorDictionary[RarityType.Epic]);
                break;
            case RarityType.Common:
            default:
                _rarityText = ("Common").SurroundWithColor(ItemDatabase.RarityColorDictionary[RarityType.Common]);
                break;
        }
        
        return _rarityText;
    }
    
    public static StatType GetStatType(this ConsumableData data_)
    {
        return GameDataBase.statsDataBase.GetStatTypeByConsumable(data_);
    }

    public static Sprite ToSprite(this StatType statType_)
    {
        return GameDataBase.statsDataBase.statSprites[statType_];
    }

    public static Sprite ToSprite(this ItemType itemType_)
    {
        return null;
    }
}
