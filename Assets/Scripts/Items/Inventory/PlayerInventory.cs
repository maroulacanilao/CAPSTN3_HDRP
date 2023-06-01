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
        [ReadOnly] private SerializedDictionary<ItemData.ItemData, ItemStackable> stackableDictionary;
        [ReadOnly] private SerializedDictionary<int, Item> itemStorageDictionary;
        [ReadOnly] private SerializedDictionary<ItemData.ItemData, List<Item>> lookupDictionary;
        
        

        #region Getters

        public ItemWeapon WeaponEquipped => weaponEquipped;
        public ItemArmor ArmorEquipped => armorEquipped;
        public ItemGold Gold => gold;
        public Item[] ItemTools => itemTools;
        public int GoldAmount => gold.GoldAmount;
        public SerializedDictionary<ItemData.ItemData, List<Item>> itemsLookup => lookupDictionary;

        #endregion

        private bool hasInitialized = false;

        public void Initialize()
        {
            if(hasInitialized) return;
            gold = new ItemGold(itemDatabase.GoldItemData, startingMoney);
            itemTools = new Item[4];

            ResetItemStorage();

            for (int i = 0; i < startingTools.Length; i++)
            {
                var _tool = startingTools[i].GetItem();
                itemTools[i] = _tool;
                InventoryEvents.OnItemOnHandUpdate.Invoke(i, _tool);
            }
            
            armorEquipped = null;
            weaponEquipped = null;
            InventoryEvents.OnUpdateStackable.AddListener(UpdateStackable);
            hasInitialized = true;
        }

        public void DeInitializeInventory()
        {
            InventoryEvents.OnUpdateStackable.RemoveListener(UpdateStackable);
            
            itemStorageDictionary?.Clear();
            stackableDictionary?.Clear();
            lookupDictionary?.Clear();
            
            gold = null;
            itemTools = null;
            hasInitialized = false;
        }

        private void ResetItemStorage()
        {
            itemStorageDictionary = new SerializedDictionary<int, Item>();
            stackableDictionary = new SerializedDictionary<ItemData.ItemData, ItemStackable>();
            lookupDictionary = new SerializedDictionary<ItemData.ItemData, List<Item>>();
        }

        public bool AddItem(Item item_)
        {
            Initialize();
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
            
            if(lookupDictionary.TryGetValue(item_.Data, out var _list)) 
            {
                _list.Add(item_);
            }
            else
            {
                lookupDictionary.Add(item_.Data, new List<Item>(){item_});
            }
            
            InventoryEvents.OnUpdateInventory.Invoke(this);
            return true;
        }
        
        public bool IsSlotEmpty(int slotIndex)
        {
            return !itemStorageDictionary.ContainsKey(slotIndex);
        }

        public bool IsAnyOpenSlot(out int index_)
        {
            index_ = GetFirstEmptySlotIndex();
            return index_ != -1;
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

        public bool HasFreeSlotInToolBar()
        {
            return itemTools.Any(x => x == null);
        }
        
        public int GetFirstEmptySlotInToolBar()
        {
            for (int i = 0; i < itemTools.Length; i++)
            {
                if (itemTools[i] != null) continue;
                
                return i;
            }
            return -1;
        }
        
        /// <summary>
        /// remove item in storage, on hand or in equipped slot.
        /// </summary>
        /// <param name="item_"></param>
        public void RemoveItem(Item item_)
        {
            if(item_ == weaponEquipped) DiscardEquippedWeapon();
            else if(item_ == armorEquipped) DiscardEquippedArmor();
            else if(itemTools.Contains(item_)) RemoveItemInHand(item_);
            else RemoveItemInStorage(item_);
        }
        
        
        public void RemoveItemInStorage(int index_)
        {
            if(index_ == -1) return;
            if (itemStorageDictionary[index_].IsStackable)
            {
                stackableDictionary.Remove(itemStorageDictionary[index_].Data);
            }

            lookupDictionary[itemStorageDictionary[index_].Data].Remove(itemStorageDictionary[index_]);
            if(lookupDictionary[itemStorageDictionary[index_].Data].Count == 0) lookupDictionary.Remove(itemStorageDictionary[index_].Data);
            
            itemStorageDictionary.Remove(index_);
            InventoryEvents.OnUpdateInventory.Invoke(this);
        }
        
        /// <summary>
        /// Also Removes the whole stack for stackable items. Use RemoveStackable method instead if only remove stack count.
        /// </summary>
        /// <param name="item_"></param>
        public void RemoveItemInStorage(Item item_)
        {
            Initialize();
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

        public void RemoveItemInHand(Item item_)
        {
            if(!item_.IsToolable) return;
            for (int i = 0; i < itemTools.Length; i++)
            {
                if (itemTools[i] != item_) continue;
                RemoveItemInHand(i);
                return;
            }
        }

        public bool IsItemOnToolBar(Item item_, out int index_)
        {
            index_ = -1;
            if (!item_.IsToolable) return false;
            for (int i = 0; i < itemTools.Length; i++)
            {
                if (itemTools[i] == item_)
                {
                    index_ = i;
                    return true;
                }
            }
            return false;
        }

        public void RemoveStackable(ItemStackable stackableItem_, int count_)
        {
            Initialize();
            if (!stackableDictionary.TryGetValue(stackableItem_.Data, out var _stackable)) return;

            if (!_stackable.RemoveStack(count_)) return;
            RemoveItemInStorage(stackableItem_);
        }

        public void UpdateStackable(ItemStackable stackableItem_)
        {
            Initialize();

            if (!stackableDictionary.TryGetValue(stackableItem_.Data, out var _stackable)) return;

            if (_stackable.HasStack) return;

            if (IsItemOnToolBar(stackableItem_, out var _index))
            {
                RemoveItemInHand(_index);
                return;
            }
            RemoveItemInStorage(stackableItem_);
        }

        public void EquipWeapon(int storageIndex_)
        {
            Initialize();
            if (!itemStorageDictionary.TryGetValue(storageIndex_, out var _item)) return;
            if(_item is not ItemWeapon _itemWeapon) return;

            weaponEquipped?.OnUnEquip(playerData.statsData);
            var _prevWpn = weaponEquipped;
            _prevWpn?.OnUnEquip(playerData.statsData);
            
            weaponEquipped = _itemWeapon;
            weaponEquipped?.OnEquip(playerData.statsData);
            
            if (_prevWpn == null) itemStorageDictionary.Remove(storageIndex_);
            else itemStorageDictionary[storageIndex_] = _prevWpn;
            
            InventoryEvents.OnWeaponEquip.Invoke(weaponEquipped);
            InventoryEvents.OnUpdateInventory.Invoke(this);
        }
        
        public bool UnEquipWeapon()
        {
            if (!IsAnyOpenSlot(out var _index)) return false;
            
            itemStorageDictionary.Add(_index, weaponEquipped);
            weaponEquipped?.OnUnEquip(playerData.statsData);
            weaponEquipped = null;
            InventoryEvents.OnArmorEquip.Invoke(null);
            InventoryEvents.OnUpdateInventory.Invoke(this);
            return true;
        }

        public void DiscardEquippedWeapon()
        {
            weaponEquipped?.OnUnEquip(playerData.statsData);
            weaponEquipped = null;
        }

        public void EquipArmor(int storageIndex_)
        {
            Debug.Log(storageIndex_);
            Initialize();
            if (!itemStorageDictionary.TryGetValue(storageIndex_, out var _item)) return;
            if(_item is not ItemArmor _itemArmor) return;

            armorEquipped?.OnUnEquip(playerData.statsData);
            var _prevArm = armorEquipped;
            _prevArm?.OnUnEquip(playerData.statsData);
            
            armorEquipped = _itemArmor;
            armorEquipped?.OnEquip(playerData.statsData);
            
            if (_prevArm == null) itemStorageDictionary.Remove(storageIndex_);
            else itemStorageDictionary[storageIndex_] = _prevArm;
            
            InventoryEvents.OnArmorEquip.Invoke(armorEquipped);
            InventoryEvents.OnUpdateInventory.Invoke(this);
        }

        public void EquipTool(int index_)
        {
            if(!itemStorageDictionary.TryGetValue(index_, out Item _item)) return;
            if(_item == null) return;
            if(!_item.IsToolable) return;

            var _indexSlot = GetFirstEmptySlotInToolBar();
            
            if(_indexSlot == -1) return;
            SwapItemsInToolBarAndStorage(_indexSlot, index_);
        }
        
        public bool UnEquipArmor()
        {
            if (!IsAnyOpenSlot(out var _index)) return false;
            
            itemStorageDictionary.Add(_index, armorEquipped);
            armorEquipped?.OnUnEquip(playerData.statsData);
            armorEquipped = null;
            InventoryEvents.OnArmorEquip.Invoke(null);
            InventoryEvents.OnUpdateInventory.Invoke(this);
            return true;
        }
        
        public void DiscardEquippedArmor()
        {
            armorEquipped?.OnUnEquip(playerData.statsData);
            armorEquipped = null;
        }
        
        public void AddGold(int amount_)
        {
            Initialize();
            Gold.AddGold(amount_);
        }
        
        public void RemoveGold(int amount_)
        {
            Initialize();
            Gold.RemoveGold(amount_);
        }
        
        public void SwapItemsInStorage(int index1_, int index2_)
        {
            Initialize();
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
            Initialize();
            (itemTools[index1_], itemTools[index2_]) = (itemTools[index2_], itemTools[index1_]);
            InventoryEvents.OnUpdateInventory.Invoke(this);
            InventoryEvents.OnItemOnHandUpdate.Invoke(index1_, itemTools[index1_]);
            InventoryEvents.OnItemOnHandUpdate.Invoke(index2_, itemTools[index2_]);
        }
        
        public void SwapItemsInToolBarAndStorage(int toolBarIndex_, int storageIndex_)
        {
            Initialize();
            itemStorageDictionary.TryAdd(storageIndex_, null);
            var _storageItem = itemStorageDictionary[storageIndex_];
            
            if(_storageItem is {IsToolable: false}) return;

            // (itemTools[toolBarIndex_], itemStorageDictionary[storageIndex_]) = (itemStorageDictionary[storageIndex_], itemTools[toolBarIndex_]);

            
            itemStorageDictionary[storageIndex_] = itemTools[toolBarIndex_];
            itemTools[toolBarIndex_] = _storageItem;
            
            if(itemStorageDictionary[storageIndex_] == null)
            {
                itemStorageDictionary.Remove(storageIndex_);
            }
            InventoryEvents.OnUpdateInventory.Invoke(this);
            InventoryEvents.OnItemOnHandUpdate.Invoke(toolBarIndex_, itemTools[toolBarIndex_]);
        }

        // By Item Type
        public void ReOrganizeInventory()
        {
            var _temp = itemStorageDictionary
                .OrderBy(x => x.Key)
                .ThenBy(x => x.Value.ItemType)
                .ToDictionary(x => x.Key, x => x.Value);

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
            Initialize();
            return itemStorageDictionary.TryGetValue(index_, out var _value) ? _value : null;
        }
    }
}
