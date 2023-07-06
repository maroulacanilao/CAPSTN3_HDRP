using System.Collections.Generic;
using Character;
using CustomEvent;
using CustomHelpers;
using Items;
using NaughtyAttributes;
using UnityEngine;

namespace BaseCore
{
    [System.Serializable]
    public class StatsGrowth
    {
        [field: BoxGroup("Character Stats")]
        [field: SerializeField] public CombatStats baseCombatStats { get; private set; }

        [field: BoxGroup("Character Stats")]
        [field: SerializeField] public CombatStats maxLevelCombatStats { get; private set; }

        [CurveRange(0, 0, 1, 1, EColor.Yellow)] [BoxGroup("Stats Growth")]
        [SerializeField] private AnimationCurve healthGrowthCurve;
    
        [CurveRange(0, 0, 1, 1, EColor.Yellow)] [BoxGroup("Stats Growth")]
        [SerializeField] private AnimationCurve intelligenceGrowthCurve;
    
        [CurveRange(0, 0, 1, 1, EColor.Yellow)] [BoxGroup("Stats Growth")]
        [SerializeField] private AnimationCurve strengthGrowthCurve;
    
        [CurveRange(0, 0, 1, 1, EColor.Yellow)] [BoxGroup("Stats Growth")]
        [SerializeField] private AnimationCurve defenseGrowthCurve;
        
        [CurveRange(0, 0, 1, 1, EColor.Yellow)] [BoxGroup("Stats Growth")]
        [SerializeField] private AnimationCurve speedGrowthCurve;
        
        public CombatStats equipmentStats { get; protected set; }
        public CombatStats bonusStats { get; protected set; }

        public CombatStats additionalStats { get; protected set; }

        public readonly HashSet<Item> EquipmentStatsSources  = new HashSet<Item>();

        public readonly Evt OnBeforeChangeStats = new Evt();
        public readonly Evt OnAfterChangeStats = new Evt();
        
        public CombatStats GetTotalStats(int level_)
        {
            return GetLeveledStats(level_) + equipmentStats + bonusStats + additionalStats;
        }
        
        public CombatStats GetTotalNonBonusStats(int level_)
        {
            return GetLeveledStats(level_) + equipmentStats + additionalStats;
        }

        public virtual void SetAdditionalStats(CombatStats stats_)
        {
            OnBeforeChangeStats?.Invoke();
            
            additionalStats = stats_;
            
            OnAfterChangeStats?.Invoke();
        }

        public virtual void AddEquipmentStats(CombatStats stats_, Item item_)
        {
            if (!EquipmentStatsSources.Add(item_)) return;

            OnBeforeChangeStats?.Invoke();
            
            equipmentStats += stats_;
            
            OnAfterChangeStats?.Invoke();
        }

        public virtual void RemoveEquipmentStats(CombatStats stats_, Item item_)
        {
            if (!EquipmentStatsSources.Remove(item_)) return;

            OnBeforeChangeStats?.Invoke();
            
            equipmentStats -= stats_;
            
            OnAfterChangeStats?.Invoke();
        }

        /// <summary>
        ///     For consumables or other temporary stats
        /// </summary>
        public virtual void AddBonusStats(CombatStats stats_)
        {
            OnBeforeChangeStats?.Invoke();
            
            bonusStats += stats_;
            
            OnAfterChangeStats?.Invoke();
        }

        /// <summary>
        ///     For consumables or other temporary stats
        /// </summary>
        public virtual void RemoveBonusStats(CombatStats stats_)
        {
            OnBeforeChangeStats?.Invoke();
            
            bonusStats -= stats_;
            
            OnAfterChangeStats?.Invoke();
        }
        
        public virtual void ClearAdditionalStats()
        {
            equipmentStats = new CombatStats();
            bonusStats = new CombatStats();
            EquipmentStatsSources.Clear();
        }

        public StatsGrowth Clone()
        {
            return new StatsGrowth
            {
                baseCombatStats = this.baseCombatStats,
                maxLevelCombatStats = this.maxLevelCombatStats,
                healthGrowthCurve = this.healthGrowthCurve,
                strengthGrowthCurve = this.strengthGrowthCurve,
                defenseGrowthCurve = this.defenseGrowthCurve,
                speedGrowthCurve = this.speedGrowthCurve,
                intelligenceGrowthCurve = this.intelligenceGrowthCurve,
                additionalStats = this.additionalStats,
                equipmentStats = this.equipmentStats,
                bonusStats = this.bonusStats,
            };
        }
        
        #region Stats Growth Calculation
        
        public CombatStats GetLeveledStats(int level_, int levelCap_ = 10)
        {
            return new CombatStats
            {
                vitality = GetLeveledVitality(level_, levelCap_),
                strength = GetLeveledStrength(level_, levelCap_),
                intelligence = GetLeveledIntelligence(level_,levelCap_),
                defense = GetLeveledDefense(level_, levelCap_),
                speed = GetLeveledSpeed(level_, levelCap_),
            };
        }

        public int GetLeveledVitality(int currentLevel_, int levelCap_ = 10)
        {
            var _leveledHp = healthGrowthCurve.EvaluateScaledCurve
                (currentLevel_, levelCap_,maxLevelCombatStats.vitality - baseCombatStats.vitality);
            
            return baseCombatStats.vitality + _leveledHp;
        }


        public int GetLeveledStrength(int currentLevel_, int levelCap_ = 10)
        {
            var _leveledStrength =  strengthGrowthCurve.EvaluateScaledCurve
                (currentLevel_, levelCap_, maxLevelCombatStats.strength - baseCombatStats.strength);
            
            return baseCombatStats.strength + _leveledStrength;
        }
        
        public int GetLeveledIntelligence(int currentLevel_, int levelCap_ = 10)
        {
            var _leveledIntel =  intelligenceGrowthCurve.EvaluateScaledCurve
                (currentLevel_, levelCap_, 
                    maxLevelCombatStats.intelligence - baseCombatStats.intelligence);
            
            return baseCombatStats.intelligence + _leveledIntel;
        }

        public int GetLeveledDefense(int currentLevel_, int levelCap_ = 10)
        {
            var _leveledDef = defenseGrowthCurve.EvaluateScaledCurve
                (currentLevel_, levelCap_, maxLevelCombatStats.defense - baseCombatStats.defense);
            
            return baseCombatStats.defense + _leveledDef;
        }
        
        public int GetLeveledSpeed(int currentLevel_, int levelCap_ = 10)
        {
            var _leveledSpeed = speedGrowthCurve.EvaluateScaledCurve
                (currentLevel_, levelCap_, maxLevelCombatStats.speed - baseCombatStats.speed);
            
            return baseCombatStats.speed + _leveledSpeed;
        }

        #endregion
    }
}