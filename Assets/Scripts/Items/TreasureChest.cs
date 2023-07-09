using System.Collections.Generic;
using BaseCore;
using Items;
using Items.ItemData;
using Managers;
using UnityEngine;

public class TreasureChest : LootDropObject
{
    [SerializeField] private LootTable lootTable;
    [SerializeField] private int level;
    
    
    private void Start()
    {
        lootDrop = lootTable.GetDrop(GameManager.Instance.GameDataBase.itemDatabase, level);
    }

    public override void ReturnToPool()
    {
        Destroy(gameObject, 2f);
    }
}
