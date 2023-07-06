using System;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using CustomHelpers;
using Items;
using Items.Inventory;
using Items.ItemData;
using ScriptableObjectData;
using UI.StatShop;
using UnityEngine;

namespace Shop
{
    [Serializable]
    public struct OfferRequirement
    {
        public ConsumableData consumableData;
        public int count;
    }

    public enum OfferingResult
    {
        Success,
        NoRequirementInInventoryStorage,
        NotEnoughRequirementsCount,
        NoOpenSlot,
        NotEnoughGold,
        SomethingWentWrong,
    }
    
    [CreateAssetMenu(menuName = "ScriptableObjects/ShrineData", fileName = "ShrineData")]
    public class ShrineData : ScriptableObject
    {
        private GameDataBase gameDataBase;

        private ItemDatabase itemDatabase => gameDataBase.itemDatabase;
        private PlayerInventory inventory => gameDataBase.playerInventory;
        private StatShopData statShopData => gameDataBase.statShopData;
        
        private List<SeedData> seedDataList = new List<SeedData>();

        private Dictionary<ItemGear, OfferRequirement> gearOffers;

        public void Initialize(GameDataBase gameDataBase_)
        {
            gameDataBase = gameDataBase_;
            gearOffers = new Dictionary<ItemGear, OfferRequirement>();
            
            seedDataList.Clear();
            seedDataList = itemDatabase.ItemDataByType[ItemType.Seed]
                .Where(id => id != null 
                             && id.ItemType == ItemType.Seed)
                .Select(id => id as SeedData)
                .ToList();
        }
        
        public void DeInitialize()
        {
            gearOffers.Clear();
            seedDataList.Clear();
        }

        public void CreateNewGearOffers()
        {
            gearOffers.Clear();
            gearOffers = GetNewGearOffers();
        }
        
        public OfferingResult OfferGear(ItemGear gear_, out ItemSeed result_)
        {
            result_ = null;
            if(!inventory.IsAnyOpenSlot()) return OfferingResult.NoOpenSlot;
            if (!inventory.itemsLookup.ContainsKey(gear_.Data)) return OfferingResult.NoRequirementInInventoryStorage;
            
            inventory.RemoveItem(gear_);
            
            Debug.Log(seedDataList.Count);
            
            result_ = seedDataList.GetRandomItem().GetSeedItem(gear_.Level);
            
            if(result_ == null) return OfferingResult.SomethingWentWrong;
            
            inventory.AddItem(result_);
            return OfferingResult.Success;
        }
        
        public OfferingResult OfferConsumable(ItemConsumable crop_, int count_ = 1)
        {
            if(crop_.StackCount < count_) return OfferingResult.NotEnoughRequirementsCount;
            if (!inventory.itemsLookup.ContainsKey(crop_.Data)) return OfferingResult.NoRequirementInInventoryStorage;
            if (!inventory.StackableDictionary.ContainsKey(crop_.Data)) return OfferingResult.NoRequirementInInventoryStorage;
            
            inventory.RemoveStackable(crop_, count_);

            var _stat = statShopData.statsBought;
            
            var _consumableData = crop_.Data as ConsumableData;
            
            if (_consumableData == null) return OfferingResult.SomethingWentWrong;
            //
            // switch (_consumableData.statType)
            // {
            //     case StatType.Health:
            //         _stat.vitality += count_;
            //         break;
            //     case StatType.Strength:
            //         _stat.strength += count_;
            //         break;
            //     case StatType.Intelligence:
            //         _stat.intelligence += count_;
            //         break;
            //     case StatType.Defense:
            //         _stat.defense += count_;
            //         break;
            //     case StatType.Speed:
            //         _stat.speed += count_;
            //         break;
            //     default:
            //         return OfferingResult.SomethingWentWrong;
            // }
            
            statShopData.SetBoughtStats(_stat);
            
            return OfferingResult.Success;
        }
        
        public OfferingResult BuySeed(SeedData seedData_, int count_ = 1)
        {
            if(!inventory.IsAnyOpenSlot()) return OfferingResult.NoOpenSlot;
            
            var _price = seedData_.SellValue * count_;
            
            if (!inventory.Gold.RemoveGold(_price)) return OfferingResult.NotEnoughGold;
            
            var _seed = seedData_.GetSeedItem(count_);

            inventory.AddItem(_seed);
            return OfferingResult.Success;
        }
        
        public OfferingResult BuyGear(ItemGear gear_)
        {
            // early returns
            if(!inventory.IsAnyOpenSlot()) return OfferingResult.NoOpenSlot;
            if(!gearOffers.TryGetValue(gear_, out var _requirement)) return OfferingResult.SomethingWentWrong;
            if (!inventory.StackableDictionary.TryGetValue(_requirement.consumableData, out var _consumable)) return OfferingResult.NoRequirementInInventoryStorage;
            if(_consumable.StackCount < _requirement.count) return OfferingResult.NotEnoughRequirementsCount;
            
            
            // logic
            inventory.RemoveStackable(_consumable, _requirement.count);
            inventory.AddItem(gear_);
            gearOffers.Remove(gear_);
            return OfferingResult.Success;
        }
        
        public Dictionary<ItemGear,OfferRequirement> GetAllGearOffers()
        {
            if(gearOffers ==null) gearOffers = GetNewGearOffers();
            return gearOffers;
        }

        public OfferingResult CanOffer(OfferRequirement offerRequirement_)
        {
            if(!inventory.IsAnyOpenSlot()) return OfferingResult.NoOpenSlot;
            if (!inventory.StackableDictionary.TryGetValue(offerRequirement_.consumableData, out var _consumable))
            {
                return OfferingResult.NoRequirementInInventoryStorage;
            }
            if(_consumable.StackCount < offerRequirement_.count) return OfferingResult.NotEnoughRequirementsCount;
            
            return OfferingResult.Success;
        }

        private Dictionary<ItemGear,OfferRequirement> GetNewGearOffers()
        {
            var _dict = new Dictionary<ItemGear,OfferRequirement>();

            var _gears = GetAllGearDataList();

            var _level = gameDataBase.playerData.level;

            var _count = Mathf.RoundToInt((_level * 1.5f) + 0.5f);

            for (int i = 0; i < _count; i++)
            {
                var _gearData = _gears.GetRandomItem();
                var _gearItem = _gearData.GetGearItem(_level);
                
                if(_gearItem == null) continue;

                var _consumableData = GetRandomConsumable();
                
                var _requirement = new OfferRequirement
                {
                    consumableData = _consumableData,
                    count = _level * 2
                };
                
                _dict.Add(_gearItem,_requirement);
                _gears.Remove(_gearData);
                
                if(_gears.Count == 0) break;
            }
            
            return _dict;
        }

        private ConsumableData GetRandomConsumable()
        {
            return itemDatabase.ItemDataByType[ItemType.Consumable]
                .Where(id => id != null 
                             && id.ItemType == ItemType.Consumable 
                             && id.IsSellable)
                .Select(id => id as ConsumableData)
                .ToList()
                .GetRandomItem();
        }

        private List<GearData> GetAllGearDataList()
        {
            var _dataList = itemDatabase.ItemDataByType[ItemType.Weapon];
            _dataList.AddRange(itemDatabase.ItemDataByType[ItemType.Armor]);
            
            var _gears = _dataList
                .Where(d => d != null && d is GearData && d.IsSellable)
                .Select(d => d as GearData)
                .ToList();

            return _gears;
        }
    }
}
