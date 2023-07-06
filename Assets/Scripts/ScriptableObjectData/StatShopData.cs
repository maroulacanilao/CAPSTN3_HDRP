using BaseCore;
using Character;
using CustomHelpers;
using NaughtyAttributes;
using ScriptableObjectData.CharacterData;
using UnityEngine;

namespace ScriptableObjectData
{
    [CreateAssetMenu(fileName = "StatShopData", menuName = "ScriptableObjects/StatShopData", order = 99)]
    public class StatShopData : ScriptableObject
    {
        [field: SerializeField] public PlayerData playerData { get; private set; }
        [field: SerializeField] public int maxCost { get; private set; } = 2_500;
        [field: SerializeField] public int maxStats { get; private set; } = 100;
        
        [CurveRange(0,0,1,1, EColor.Blue)]
        [SerializeField] private AnimationCurve costCurve;

        public CombatStats statsBought { get; private set; }
        
        private StatsGrowth statsData => playerData.statsData;
        public CombatStats playerBaseStats => playerData.baseStats;

        public void Initialize()
        {
            SetBoughtStats(new CombatStats());
        }
        
        public int GetCost(int level_)
        {
            return costCurve.EvaluateScaledCurve(level_, maxStats, maxCost);
        }
        
        public void BuyStats(CombatStats stats_, int level_)
        {
            var _cost = GetCost(level_);
            
            if (playerData.inventory.Gold.RemoveGold(_cost))
            {
                Debug.Log("Not enough gold");
                return;
            }
            
            statsBought = stats_;
            statsData.SetAdditionalStats(stats_);
        }
        
        public void SetBoughtStats(CombatStats stats_)
        {
            statsBought = stats_;
            statsData.SetAdditionalStats(stats_);
        }
    }
}
