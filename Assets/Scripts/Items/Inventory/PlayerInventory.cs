using System;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using CustomEvent;
using CustomHelpers;
using Items.ItemData;
using Items.ItemData.Tools;
using NaughtyAttributes;
using ScriptableObjectData;
using ScriptableObjectData.CharacterData;
using UnityEngine;

namespace Items.Inventory
{
    public static class InventoryEvents
    {
        public static readonly Evt<int, Item> OnItemOnHandUpdate = new Evt<int, Item>();
        public static readonly Evt<PlayerInventory> OnUpdateInventory = new Evt<PlayerInventory>();
        public static readonly Evt<ItemWeapon> OnWeaponEquip = new Evt<ItemWeapon>();
        public static readonly Evt<ItemArmor> OnArmorEquip = new Evt<ItemArmor>();
        public static readonly Evt<ItemStackable> OnUpdateStackable = new Evt<ItemStackable>();
        public static readonly Evt OrganizeInventory = new Evt();
    }

    [CreateAssetMenu(menuName = "ScriptableObjects/InventoryData", fileName = "PlayerInventoryData")]
    public class PlayerInventory : ScriptableObject
    {
        [SerializeField] private ItemDatabase itemDatabase;
        [SerializeField] private PlayerData playerData;
        [SerializeField] private int maxInventorySize = 24;

        [Header("Starter Items")]
        [SerializeField] private int startingMoney = 100;
        [SerializeField] private ToolData[] startingTools;
        [SerializeField] private ArmorData startingArmor;
        [SerializeField] private WeaponData startingWeapon;

        private ItemGold gold;
        private ItemWeapon weaponEquipped;
        private ItemArmor armorEquipped;

        [ReadOnly] [SerializeReference] private Item[] itemTools;
        private Dictionary<ItemData.ItemData, ItemStackable> stackableDictionary;
        [ReadOnly]  private SerializedDictionary<int, Item> itemStorageDictionary = new SerializedDictionary<int, Item>();

        #region Getters

        public ItemWeapon WeaponEquipped => weaponEquipped;
        public ItemArmor ArmorEquipped => armorEquipped;
        public ItemGold Gold => gold;
        public Item[] ItemTools => itemTools;
        public int GoldAmount => gold.GoldAmount;

        #endregion

        private bool hasInitialized = false;

        public void InitializeInventory()
        {
            if(hasInitialized) return;
            gold = new ItemGold(itemDatabase.GoldItemData, startingMoney);
            itemTools = new Item[4];

            stackableDictionary = new Dictionary<ItemData.ItemData, ItemStackable>();
            
            ResetItemStorage();

            for (int i = 0; i < startingTools.Length; i++)
            {
                var _tool = startingTools[i].GetItem();
                itemTools[i] = _tool;
                InventoryEvents.OnItemOnHandUpdate.Invoke(i, _tool);
            }
            InventoryEvents.OnUpdateStackable.AddListener(UpdateStackable);
            hasInitialized = true;
        }

        public void DeInitializeInventory()
        {
            InventoryEvents.OnUpdateStackable.RemoveListener(UpdateStackable);
            itemStorageDictionary = new SerializedDictionary<int, Item>();
            stackableDictionary?.Clear();
            gold = null;
            itemTools = null;
            hasInitialized = false;
        }

        private void ResetItemStorage()
        {
            itemStorageDictionary = new SerializedDictionary<int, Item>();
        }

        public bool AddItem(Item item_)
        {
            InitializeInventory();
            if (itemStorageDictionary == null)
            {
                throw new Exception("Item Storage is not initialized.");
            }
            if (itemStorageDictionary.Count >= maxInventorySize) return false;

            if (item_ is ItemGold _goldItem)
            {
                gold.AddGold(_goldItem.GoldAmount);
                return true;
            }

            if (item_.IsStackable)
            {
                if (stackableDictionary.TryGetValue(item_.Data, out var stackable))
                {
                    stackable.AddStack(item_.StackCount);
                    return true;
                }
                stackableDictionary.Add(item_.Data, item_ as ItemStackable);
            }
            var _openIndex = GetFirstEmptySlotIndex();
            if (_openIndex == -1) return false;
            
            itemStorageDictionary.Add(_openIndex, item_);
            InventoryEvents.OnUpdateInventory.Invoke(this);
            return true;
        }
        
        public bool IsSlotEmpty(int slotIndex)
        {
            return !itemStorageDictionary.ContainsKey(slotIndex);
        }

        public int GetFirstEmptySlotIndex()
        {
            for (int i = 0; i < maxInventorySize; i++)
            {
                if (IsSlotEmpty(i))
                {
                    return i;
                }
            }
            return -1;
        }

        public void RemoveItemInStorage(int index_)
        {
            if (itemStorageDictionary[index_].IsStackable)
            {
                stackableDictionary.Remove(itemStorageDictionary[index_].Data);
            }
            
            itemStorageDictionary.Remove(index_);
            InventoryEvents.OnUpdateInventory.Invoke(this);
        }
        
        /// <summary>
        /// Also Removes the whole stack for stackable items. Use RemoveStackable method instead if only remove stack count.
        /// </summary>
        /// <param name="item_"></param>
        public void RemoveItemInStorage(Item item_)
        {
            InitializeInventory();
            Debug.Log("remove + " + item_.Data.ItemName );
            var _key = itemStorageDictionary.FirstOrDefault(x => x.Value == item_).Key;
            RemoveItemInStorage(_key);
        }
        
        public void RemoveItemInHand(int index_)
        {
            if(itemTools[index_] == null) return;
            if(itemTools[index_].IsStackable) stackableDictionary.Remove(itemTools[index_].Data);
            itemTools[index_] = null;
            InventoryEvents.OnItemOnHandUpdate.Invoke(index_, null);
        }

        public void RemoveStackable(ItemStackable stackableItem_, int count_)
        {
            InitializeInventory();
            if (!stackableDictionary.TryGetValue(stackableItem_.Data, out var _stackable)) return;

            if (!_stackable.RemoveStack(count_)) return;
            RemoveItemInStorage(stackableItem_);
        }

        public void UpdateStackable(ItemStackable stackableItem_)
        {
            InitializeInventory();

            if (!stackableDictionary.TryGetValue(stackableItem_.Data, out var _stackable)) return;
            Debug.Log(_stackable.HasStack);
            if (_stackable.HasStack) return;
            RemoveItemInStorage(stackableItem_);
        }

        public void EquipWeapon(ItemWeapon weapon_)
        {
            InitializeInventory();
            weaponEquipped?.OnUnEquip(playerData.statsData);
            weaponEquipped = weapon_;
            weaponEquipped?.OnEquip(playerData.statsData);
            InventoryEvents.OnWeaponEquip.Invoke(weapon_);
        }
        
        public void EquipArmor(ItemArmor armor_)
        {
            InitializeInventory();
            armorEquipped?.OnUnEquip(playerData.statsData);
            armorEquipped = armor_;
            armorEquipped?.OnEquip(playerData.statsData);
            InventoryEvents.OnArmorEquip.Invoke(armor_);
        }
        
        public void AddGold(int amount_)
        {
            InitializeInventory();
            Gold.AddGold(amount_);
        }
        
        public void RemoveGold(int amount_)
        {
            InitializeInventory();
            Gold.RemoveGold(amount_);
        }
        
        public void SwapItemsInStorage(int index1_, int index2_)
        {
            InitializeInventory();
            //
            if(itemStorageDictionary.ContainsKey(index1_))
            {
                if (itemStorageDictionary.ContainsKey(index2_))
                {
                    (itemStorageDictionary[index1_], itemStorageDictionary[index2_]) = (itemStorageDictionary[index2_], itemStorageDictionary[index1_]);
                }
                else
                {
                    itemStorageDictionary.Add(index2_, itemStorageDictionary[index1_]);
                    itemStorageDictionary.Remove(index1_);
                }
            }
            else
            {
                if(itemStorageDictionary.TryGetValue(index2_, out var _value))
                {
                    itemStorageDictionary.Add(index1_, _value);
                    itemStorageDictionary.Remove(index2_);
                }
                else
                {
                    return;
                }
            }
            
            InventoryEvents.OnUpdateInventory.Invoke(this);
        }
        public void SwapItemInToolBar(int index1_, int index2_)
        {
            InitializeInventory();
            (itemTools[index1_], itemTools[index2_]) = (itemTools[index2_], itemTools[index1_]);
            InventoryEvents.OnUpdateInventory.Invoke(this);
            InventoryEvents.OnItemOnHandUpdate.Invoke(index1_, itemTools[index1_]);
            InventoryEvents.OnItemOnHandUpdate.Invoke(index2_, itemTools[index2_]);
        }
        
        public void SwapItemsInToolBarAndStorage(int toolBarIndex_, int storageIndex_)
        {
            InitializeInventory();
            var _storageItem = itemStorageDictionary[storageIndex_];
            
            if(_storageItem is {IsToolable: false}) return;

            // (itemTools[toolBarIndex_], itemStorage[storageIndex_]) = (itemStorage[storageIndex_], itemTools[toolBarIndex_]);
            
            itemStorageDictionary[storageIndex_] = itemTools[toolBarIndex_];
            itemTools[toolBarIndex_] = _storageItem;
            
            if(itemStorageDictionary[storageIndex_] == null)
            {
                itemStorageDictionary.Remove(storageIndex_);
            }
            InventoryEvents.OnUpdateInventory.Invoke(this);
            InventoryEvents.OnItemOnHandUpdate.Invoke(toolBarIndex_, itemTools[toolBarIndex_]);
        }

        public void ReOrganizeInventory()
        {
            var _temp = itemStorageDictionary.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
            itemStorageDictionary = new SerializedDictionary<int, Item>();
            
            int _newIndex = 0;
            for (int i = 0; i < maxInventorySize; i++)
            {
                if(!_temp.ContainsKey(i)) continue;
                itemStorageDictionary.Add(_newIndex, _temp[i]);
                _newIndex++;
            }
            InventoryEvents.OrganizeInventory.Invoke();
        }

        public Item GetItemInStorage(int index_)
        {
            InitializeInventory();
            return itemStorageDictionary.TryGetValue(index_, out var _value) ? _value : null;
        }
    }
}
