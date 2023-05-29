using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using CustomHelpers;
using Items.ItemData;
using UnityEngine;

namespace ScriptableObjectData
{
    [CreateAssetMenu(menuName = "ScriptableObjects/SeedDataBase", fileName = "New SeedDataBase")]
    public class SeedDataBase : ScriptableObject
    {
        [field: SerializedDictionary("Level","Seeds")]
        [field: SerializeField] public SerializedDictionary<int,List<SeedData>> SeedDictionary { get; private set; }

        private int currentLevel = 0;
        private HashSet<SeedData> viableSeedsCache = new HashSet<SeedData>();

        public void Initialize(int playerLevel_)
        {
            currentLevel = playerLevel_;
            viableSeedsCache = GetViableSeeds(playerLevel_);
        }
        
        public SeedData GetRandomSeed (int level_)
        {
            if(level_ !=  currentLevel)
            {
                currentLevel = level_;
                viableSeedsCache = GetViableSeeds(level_);
            }
            
            return viableSeedsCache.GetRandomFromCollection();
        }
        
        public ConsumableData GetRandomConsumable (int level_)
        {
            return GetRandomSeed(level_).produceData;
        }
        
        public HashSet<SeedData> GetViableSeeds (int level_)
        {
            var _viableSeeds = new HashSet<SeedData>();

            foreach (var _seedPair in SeedDictionary.Where(seedPair_ => seedPair_.Key <= level_))
            {
                _viableSeeds.UnionWith(_seedPair.Value);
            }

            return _viableSeeds;
        }
    }
}
