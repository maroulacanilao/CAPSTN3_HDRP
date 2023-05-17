﻿using System.Linq;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace Items.ItemData
{
    [CreateAssetMenu(fileName = "SeedData", menuName = "ScriptableObjects/ItemData/SeedData", order = 0)]
    public class SeedData : ItemData
    {
        [field: Header("Plant")]
        [field: SerializeField] public int minutesToGrow { get; set; }
        [field: SerializeField] public Vector2Int producePossibleCount { get; set; }
        [field: SerializeField] public ConsumableData produceData { get; private set; }
        
        [field: SerializeField] public Sprite soilSprite { get; private set; }
        [field: SerializeField] public Sprite plantSprite { get; private set; }
        [field: SerializeField] public Sprite harvestSprite { get; private set; }

        private void Reset()
        {
            itemType = ItemType.Seed;
        }

        private void OnValidate()
        {
            itemType = ItemType.Seed;
        }
    }
}