using System;
using BaseCore;
using Character;
using Character.CharacterComponents;
using Items.ItemData;
using Items.ItemData.Tools;
using Player;
using UnityEngine;

namespace Items
{
    public enum ItemType
    {
        None = 0,
        Gold = 1,
        Seed = 2,
        Consumable = 3,
        QuestItem = 4,
        Tool = 5,
        Armor = 6,
        Weapon = 7,
        Craftable = 8,
    }

    public enum RarityType
    {
        Common = 1,
        Uncommon = 2,
        Rare = 3,
        Epic = 4,
    }

    [Serializable]
    public abstract class Item
    {
        protected ItemData.ItemData data;
        protected string dataID;
        protected CombatStats stats;
        protected bool isGearEquipped;
        protected bool isGear;
        protected int level = 1;
        protected int stackCount;
        protected bool isStackable;
        protected bool isToolable;
        protected bool isDiscardable = true;
        protected RarityType rarityType;

        public ItemData.ItemData Data => data;
        public ItemType ItemType => Data.ItemType;
        public RarityType RarityType => rarityType;

        public CombatStats Stats => stats;
        public bool IsGearEquipped => isGearEquipped;
        public bool IsDiscardable => isDiscardable;
        public int Level => level;
        
        /// <summary>
        /// If the class is a child of ItemGear, ItemWeapon, or ItemArmor
        /// </summary>
        public bool IsGear => isGear;
        public int StackCount => stackCount;
        public bool IsStackable => isStackable;
        public bool HasStack => stackCount > 0;
        
        /// <summary>
        /// If it can be equipped in Tool bar
        /// </summary>
        public bool IsToolable => isToolable;

        protected Item(ItemData.ItemData data_, RarityType rarityType_ = RarityType.Common)
        {
            data = data_;
            dataID = data_.ItemID;
            rarityType = rarityType_;
        }
        
        public virtual void OnEquip(StatsGrowth statsData_) {}
        public virtual void OnUnEquip(StatsGrowth statsData_) {}
        public virtual bool Consume(StatusEffectReceiver statusEffectReceiver_) { return false; }
        public virtual void UseTool(PlayerEquipment playerEquipment_) {}
        
        public virtual void SetStats(CombatStats stats_)
        {
            stats = stats_;
        }
        
        public virtual void SetLevel(int level_)
        {
            level = level_;
        }
        
        public virtual void SetStack(int amount_)
        {
            stackCount = amount_;
        }

        public abstract Item Clone();
    }

    [Serializable]
    public abstract class ItemGear : Item
    {
        protected ItemGear(ItemData.ItemData data_, CombatStats stats_, int level_ = 1, RarityType rarityType_ = RarityType.Common) : base(data_, rarityType_)
        {
            data = data_;
            isGearEquipped = false;
            level = level_;
            stats = stats_;
        }

        public override void OnEquip(StatsGrowth statsData_)
        {
            statsData_.AddEquipmentStats(stats, this);
            isGearEquipped = true;
        }

        public override void OnUnEquip(StatsGrowth statsData_)
        {
            statsData_.RemoveEquipmentStats(stats, this);
            isGearEquipped = false;
        }
    }

    public abstract class ItemStackable : Item
    {
        protected ItemStackable(ItemData.ItemData data_, int count_) 
            : base(data_)
        {
            data = data_;
            isStackable = true;
            stackCount = count_;
        }
        
        public void AddStack()
        {
            stackCount++;
        }

        public void AddStack(int amount_)
        {
            stackCount += amount_;
        }

        /// <summary>
        /// returns true if there is still stack
        /// </summary>
        /// <returns></returns>
        public bool RemoveStack()
        {
            stackCount--;
            return HasStack;
        }

        // returns true if there is still stack
        public bool RemoveStack(int amount_)
        {
            stackCount -= amount_;
            return HasStack;
        }

        public void ClearStack()
        {
            stackCount = 0;
        }
    }

    [Serializable]
    public class ItemWeapon : ItemGear
    {
        public ItemWeapon(ItemData.ItemData data_, CombatStats stats_, int level_ = 1, RarityType rarityType_ = RarityType.Common) 
            : base(data_, stats_, level_, rarityType_)
        {
            
        }
        public override Item Clone()
        {
            var _clone = new ItemWeapon(data, stats)
            {
                data = this.data,
                isGearEquipped = this.isGearEquipped,
                stats = this.stats,
                level = this.level,
                rarityType = this.rarityType
            };
            return _clone;
        }
    }

    [Serializable]
    public class ItemArmor : ItemGear
    {
        public ItemArmor(ItemData.ItemData data_, CombatStats stats_, int level_ = 1, RarityType rarityType_ = RarityType.Common) 
            : base(data_, stats_, level_, rarityType_)
        {
            
        }
        
        public override Item Clone()
        {
            var _clone = new ItemArmor(data, stats)
            {
                data = this.data,
                isGearEquipped = this.isGearEquipped,
                stats = this.stats,
                level = this.level,
                rarityType = this.rarityType
            };
            return _clone;
        }
    }

    [Serializable]
    public class ItemConsumable : ItemStackable
    {
        public ItemConsumable(ItemData.ItemData data_, int count_) : base(data_, count_)
        {
            
        }
        /// <summary>
        /// returns true if there is still stack available
        /// </summary>
        /// <returns></returns>
        public override bool Consume(StatusEffectReceiver statusEffectReceiver_)
        {
            var _data = (ConsumableData) data;
            
            var _effectInstance = UnityEngine.Object.Instantiate(_data.ConsumeEffect, Vector3.zero, Quaternion.identity);
            
            statusEffectReceiver_.ApplyStatusEffect(_effectInstance);

            RemoveStack();
            return HasStack;
        }
        
        public override Item Clone()
        {
            var _clone = new ItemConsumable(data, stackCount);
            return _clone;
        }
    }

    [Serializable]
    public class ItemGold : Item
    {
        private int goldAmount;
        public int GoldAmount => goldAmount;

        public ItemGold(ItemData.ItemData data_, int amount_) : base(data_)
        {
            goldAmount = amount_;
            isDiscardable = false;
        }

        public void AddGold(int amount_)
        {
            goldAmount += amount_;
        }

        /// <summary>
        /// returns true if there is enough gold to be removed
        /// </summary>
        /// <param name="amount_"></param>
        /// <returns></returns>
        public bool RemoveGold(int amount_)
        {
            if (amount_ > goldAmount) return false;

            goldAmount -= amount_;
            return true;
        }
        
        public void SetAmount(int amount_)
        {
            goldAmount = amount_;
        }
        
        public override Item Clone()
        {
            var _clone = new ItemGold(data, goldAmount);
            return _clone;
        }
    }
    
    [Serializable]
    public class ItemSeed : ItemStackable
    {
        public ItemSeed(ItemData.ItemData data_, int count_) : base(data_, count_)
        {
            isToolable = true;
        }
        
        public override Item Clone()
        {
            var _clone = new ItemSeed(data, stackCount);
            return _clone;
        }
    }
    
    [Serializable]
    public class ItemTool : Item
    {
        public ItemTool(ItemData.ItemData data_) : base(data_)
        {
            isToolable = true;
            isDiscardable = false;
        }
        
        public override void UseTool(PlayerEquipment playerEquipment_)
        {
            var _data = (ToolData) data;
            _data.UseTool(playerEquipment_);
        }

        public override Item Clone()
        {
            var _clone = new ItemTool(data)
            {
                isToolable = true,
                isDiscardable = false,
            };
            return _clone;
        }
    }
    
    [Serializable]
    public class ItemCraftable : ItemStackable
    {
        
        public ItemCraftable(ItemData.ItemData data_, int count_) : base(data_, count_)
        {
            isToolable = true;
        }
        
        public override Item Clone()
        {
            var _clone = new ItemCraftable(data, stackCount)
            {
                isToolable = true,
                isDiscardable = false,
            };
            return _clone;
        }
    }
}