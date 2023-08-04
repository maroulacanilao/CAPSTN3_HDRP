using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Items;
using Items.ItemData;
using UI.ShrineUI;
using UnityEngine;

public class SeedDisplay : ConsumableDisplay
{
    private SeedData[] GetAllConsumableData()
    {
        if (!itemDatabase.ItemDataByType.TryGetValue(ItemType.Seed, out var _list))
        {
            throw new Exception("No consumable data found");
        }
        
        return _list
            .Where(d => d != null && d is SeedData)
            .Select(d => d as SeedData)
            .ToArray();
    }

    private SeedData[] seedDatas;

    private void Awake()
    {
        seedDatas = GetAllConsumableData();
    }


    private int GetCount(SeedData seedData_)
    {
        if (!inventory.itemsLookup.TryGetValue(seedData_, out var _itemList))
        {
            return 0;
        }
        
        if(_itemList == null) return 0;
        if(_itemList.Count == 0) return 0;
        var _item = _itemList.FirstOrDefault(i => i != null);
        if(_item == null) return 0;
            
        return _item is not ItemSeed _seed ? 0 : _seed.StackCount;
    }

    public override void UpdateDisplay()
    {
        if (seedDatas == null)
        {
            seedDatas = GetAllConsumableData();
        }
        
        for (var i = 0; i < consumableDisplayItems.Count; i++)
        {
            if(i >= seedDatas.Length) continue;
            var _data = seedDatas[i];
            if(_data == null) continue;
            var _count = GetCount(_data);
            var _displayItem = consumableDisplayItems[i];
            _displayItem.panel.SetActive(_data != null);
            _displayItem.icon.sprite = _data.produceData.Icon;
            _displayItem.countTxt.text = $"x{_count}";
        }
    }
}
