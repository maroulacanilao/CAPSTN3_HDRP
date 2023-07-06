using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using BaseCore;
using CustomHelpers;
using Items;
using Items.ItemData;
using NaughtyAttributes;
using UnityEngine;

namespace ScriptableObjectData
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Persistent/ItemDatabase", fileName = "New ItemDatabase")]
    public class ItemDatabase : ScriptableObject
    {
        [field: SerializeField] public GoldData GoldItemData { get; private set; }
        [field: SerializeField] [field: BoxGroup("Item Data")] public SerializedDictionary<string, ItemData> ItemDataDictionary { get; private set; }

        [field: SerializeField] [field: BoxGroup("Item Data")] public SerializedDictionary<ItemType, List<ItemData>> ItemDataByType { get; private set; }
        
        [field: BoxGroup("Rarity Properties")]
        [field: SerializeField]  public SerializedDictionary<RarityType, Color> RarityColorDictionary { get; private set; }
        [field: BoxGroup("Rarity Properties")]
        [field: SerializeField] public SerializedDictionary<RarityType, float> RarityModiferValue { get; private set; }
        [field: BoxGroup("Rarity Properties")]
        [field: SerializeField] public SerializedDictionary<RarityType, Vector2> RarityChanceMinMaxValue { get; private set; }
        [field: BoxGroup("Rarity Properties")]
        [field: SerializeField] public SerializedDictionary<RarityType,AnimationCurve> RarityChanceCurve{ get; private set; }
        [field: BoxGroup("Rarity Properties")]
        [field: SerializeField] public SerializedDictionary<int,WeightedDictionary<RarityType>> RarityChance{ get; private set; }


        private void OnEnable()
        {
            foreach (var rarityPair in RarityChance)
            {
                rarityPair.Value.ForceInitialize();
            }
        }
        
        public RarityType GetRandomRarity(int level_, out float modifer_)
        {
            var _rarity = RarityChance[level_].GetWeightedRandom();
            
            modifer_ = RarityModiferValue[_rarity];
            
            return _rarity;
        }


#if UNITY_EDITOR
        [Button("Set Item Dictionary")]
        private void SetItemDictionary()
        {
            var _datas = Resources.LoadAll<ItemData>("Data/ItemData");
            ItemDataDictionary = new SerializedDictionary<string, ItemData>();
            ItemDataByType = new SerializedDictionary<ItemType, List<ItemData>>();
            
            foreach (var _itemData in _datas)
            {
                ItemDataDictionary.Add(_itemData.ItemID,_itemData);
                
                if(ItemDataByType.TryGetValue(_itemData.ItemType, out var _value))
                {
                    _value.Add(_itemData);
                }
                else
                {
                    ItemDataByType.Add(_itemData.ItemType, new List<ItemData>()
                    {
                        _itemData
                    });
                }
            }
        }
        
        [Button("Set Rarity Chance Per Level")]
        public void SetRarityChance()
        {
            RarityChance = new SerializedDictionary<int, WeightedDictionary<RarityType>>();
            
            for (int i = 1; i <= 10; i++)
            {
                RarityChance.Add(i, GetRarityChance(i));
                RarityChance[i].ForceInitialize();
            }
        }

        public WeightedDictionary<RarityType> GetRarityChance(int level_)
        {
            // var _leveledHp = healthGrowthCurve.EvaluateScaledCurve
            //     (currentLevel_, levelCap_,maxLevelCombatStats.vitality - baseCombatStats.vitality);
            
            var _commonMinMax = RarityChanceMinMaxValue[RarityType.Common];
            var _commonChance = _commonMinMax.x + RarityChanceCurve[RarityType.Common]
                .EvaluateScaledCurve(level_, 10, _commonMinMax.y - _commonMinMax.x);

            var _uncommonMinMax = RarityChanceMinMaxValue[RarityType.Uncommon];
            var _uncommonChance = _uncommonMinMax.x + RarityChanceCurve[RarityType.Uncommon]
                .EvaluateScaledCurve(level_, 10, _uncommonMinMax.y - _uncommonMinMax.x);
            
            var _rareMinMax = RarityChanceMinMaxValue[RarityType.Rare];
            var _rareChance = _rareMinMax.x + RarityChanceCurve[RarityType.Rare]
                .EvaluateScaledCurve(level_, 10, _rareMinMax.y - _rareMinMax.x);
            
            var _epicMinMax = RarityChanceMinMaxValue[RarityType.Epic];
            var _epicChance = _epicMinMax.x + RarityChanceCurve[RarityType.Epic]
                .EvaluateScaledCurve(level_, 10, _epicMinMax.y - _epicMinMax.x);

            var _temp = new SerializedDictionary<RarityType, float>();
            _temp.Add(RarityType.Common, _commonChance);
            _temp.Add(RarityType.Uncommon, _uncommonChance);
            _temp.Add(RarityType.Rare, _rareChance);
            _temp.Add(RarityType.Epic, _epicChance);
            
            return new WeightedDictionary<RarityType>(_temp);
        }
#endif
    }
}