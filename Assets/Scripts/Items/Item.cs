using System;
using BaseCore;
using Character;
using Items.ItemData;
using Items.ItemData.Tools;
using Player;
using UnityEngine;

namespace Items
{
    public enum ItemType
    {
        None = 0,
        Weapon = 1,
        Armor = 2,
        Consumable = 3,
        QuestItem = 4,
        Tool = 5,
        Gold = 6,
        Seed = 7,
    }

    public enum RarityType
    {
        Common = 0,
        Uncommon = 1,
        Rare = 2,
        Epic = 3,
    }

    [Serializable]
    public abstract class Item
    {
        protected ItemData.ItemData data;
        protected string dataID;
        protected CombatStats stats;
        protected bool isGearEquipped;
        protected bool isGear;
        protected int stackCount;
        protected bool isStackable;
        protected bool isToolable;
        protected bool isDiscardable;

        public ItemData.ItemData Data => data;
        public ItemType ItemType => Data.ItemType;
        public RarityType RarityType => Data.RarityType;

        public CombatStats Stats => stats;
        public bool IsGearEquipped => isGearEquipped;
        public bool IsDiscardable = true;
        
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

        protected Item(ItemData.ItemData data_)
        {
            data = data_;
            dataID = data_.ItemID;
        }
        
        public virtual void OnEquip(StatsGrowth statsData_) {}
        public virtual void OnUnEquip(StatsGrowth statsData_) {}
        public virtual bool Consume(StatusEffectReceiver statusEffectReceiver_) { return false; }
        public virtual void UseTool(PlayerEquipment playerEquipment_) {}
    }

    [Serializable]
    public abstract class ItemGear : Item
    {
        protected ItemGear(ItemData.ItemData data_, CombatStats stats_) : base(data_)
        {
            data = data_;
            isGearEquipped = false;
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
        public ItemWeapon(ItemData.ItemData data_, CombatStats stats_) : base(data_, stats_)
        {
        }
    }

    [Serializable]
    public class ItemArmor : ItemGear
    {
        public ItemArmor(ItemData.ItemData data_, CombatStats stats_) : base(data_, stats_)
        {
            
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
            statusEffectReceiver_.ApplyStatusEffect(_data.ConsumeEffect);

            RemoveStack();
            return HasStack;
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
    }
    
    [Serializable]
    public class ItemSeed : ItemStackable
    {
        public ItemSeed(ItemData.ItemData data_, int count_) : base(data_, count_)
        {
            isToolable = true;
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
    }
}