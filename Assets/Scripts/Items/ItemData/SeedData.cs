using System.Linq;
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
        [field: SerializeField] public Sprite readyToHarvestSprite { get; private set; }

        [field: SerializeField] public int expReward { get; private set; }

        private void Reset()
        {
            ItemType = ItemType.Seed;
            IsStackable = true;
        }

        protected override void OnValidate()
        {
            ItemType = ItemType.Seed;
            IsStackable = true;
        }
    }
}