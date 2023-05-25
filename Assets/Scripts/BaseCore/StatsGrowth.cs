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
        [SerializeField] private AnimationCurve manaGrowthCurve;
    
        [CurveRange(0, 0, 1, 1, EColor.Yellow)] [BoxGroup("Stats Growth")]
        [SerializeField] private AnimationCurve weaponDamageGrowthCurve;
    
        [CurveRange(0, 0, 1, 1, EColor.Yellow)] [BoxGroup("Stats Growth")]
        [SerializeField] private AnimationCurve armorGrowthCurve;

        [CurveRange(0, 0, 1, 1, EColor.Yellow)] [BoxGroup("Stats Growth")]
        [SerializeField] private AnimationCurve magicDamageGrowthCurve;

        [CurveRange(0, 0, 1, 1, EColor.Yellow)] [BoxGroup("Stats Growth")]
        [SerializeField] private AnimationCurve magicResistanceGrowthCurve;
        
        [CurveRange(0, 0, 1, 1, EColor.Yellow)] [BoxGroup("Stats Growth")]
        [SerializeField] private AnimationCurve accuracyGrowthCurve;
        
        [CurveRange(0, 0, 1, 1, EColor.Yellow)] [BoxGroup("Stats Growth")]
        [SerializeField] private AnimationCurve speedGrowthCurve;
        public CombatStats equipmentStats { get; private set; }
        public CombatStats bonusStats { get; private set; }

        public HashSet<Item> equipmentStatsSources { get; private set; } = new HashSet<Item>();

        public CombatStats GetTotalStats(int level_)
        {
            return GetLeveledStats(level_) + equipmentStats + bonusStats;
        }

        public void AddEquipmentStats(CombatStats stats_, Item item_)
        {
            if (!equipmentStatsSources.Add(item_)) return;

            equipmentStats += stats_;
        }

        public void RemoveEquipmentStats(CombatStats stats_, Item item_)
        {
            if (!equipmentStatsSources.Remove(item_)) return;

            equipmentStats -= stats_;
        }

        /// <summary>
        ///     For consumables or other temporary stats
        /// </summary>
        public void AddBonusStats(CombatStats stats_)
        {
            bonusStats += stats_;
        }

        /// <summary>
        ///     For consumables or other temporary stats
        /// </summary>
        public void RemoveBonusStats(CombatStats stats_)
        {
            bonusStats -= stats_;
        }
        
        public void ClearAdditionalStats()
        {
            equipmentStats = new CombatStats();
            bonusStats = new CombatStats();
            equipmentStatsSources.Clear();
        }
        
        #region Stats Growth Calculation
        
        public CombatStats GetLeveledStats(int level_, int levelCap_ = 10)
        {
            return new CombatStats
            {
                maxHp = GetLeveledMaxHealth(level_, levelCap_),
                maxMana = GetLeveledMaxMana(level_, levelCap_),
                physicalDamage = GetLeveledWeaponDamage(level_, levelCap_),
                armor = GetLeveledArmor(level_, levelCap_),
                magicDamage =  GetLeveledMagicDamage(level_, levelCap_),
                magicResistance = GetLeveledMagicResistance(level_, levelCap_),
                accuracy = GetLeveledAccuracy(level_, levelCap_),
                speed = GetLeveledSpeed(level_, levelCap_)
            };
        }

        public int GetLeveledMaxHealth(int currentLevel_, int levelCap_ = 10)
        {
            var _leveledHp = healthGrowthCurve.EvaluateScaledCurve
                (currentLevel_, levelCap_,maxLevelCombatStats.maxHp - baseCombatStats.maxHp);
            
            return baseCombatStats.maxHp + _leveledHp;
        }

        public int GetLeveledMaxMana(int currentLevel_, int levelCap_ = 10)
        {
            var _leveledMana = manaGrowthCurve.EvaluateScaledCurve
                (currentLevel_, levelCap_, maxLevelCombatStats.maxMana - baseCombatStats.maxMana);
            
            return baseCombatStats.maxMana + _leveledMana;
        }

        public int GetLeveledWeaponDamage(int currentLevel_, int levelCap_ = 10)
        {
            var _leveledWpnDmg =  weaponDamageGrowthCurve.EvaluateScaledCurve
                (currentLevel_, levelCap_, maxLevelCombatStats.physicalDamage - baseCombatStats.physicalDamage);
            
            return baseCombatStats.physicalDamage + _leveledWpnDmg;
        }

        public int GetLeveledAccuracy(int currentLevel_, int levelCap_ = 10)
        {
            var _leveledAcc = accuracyGrowthCurve.EvaluateScaledCurve
                (currentLevel_, levelCap_, maxLevelCombatStats.accuracy - baseCombatStats.accuracy);
            
            return baseCombatStats.accuracy + _leveledAcc;
        }

        public int GetLeveledMagicDamage(int currentLevel_, int levelCap_ = 10)
        {
            var _leveledMagDmg = magicDamageGrowthCurve.EvaluateScaledCurve
                (currentLevel_, levelCap_, maxLevelCombatStats.magicDamage - baseCombatStats.magicDamage);
            
            return baseCombatStats.magicDamage + _leveledMagDmg;
        }
        
        public int GetLeveledMagicResistance(int currentLevel_, int levelCap_ = 10)
        {
            var _leveledMagRes = accuracyGrowthCurve.EvaluateScaledCurve
                (currentLevel_, levelCap_, maxLevelCombatStats.magicResistance - baseCombatStats.magicResistance);
            
            return baseCombatStats.magicResistance + _leveledMagRes;
        }

        public int GetLeveledArmor(int currentLevel_, int levelCap_ = 10)
        {
            var _leveledArm = armorGrowthCurve.EvaluateScaledCurve
                (currentLevel_, levelCap_, maxLevelCombatStats.armor - baseCombatStats.armor);
            
            return baseCombatStats.armor + _leveledArm;
        }

        public int GetLeveledSpeed(int currentLevel_, int levelCap_ = 10)
        {
            var _leveledSpeed = speedGrowthCurve.EvaluateScaledCurve
                (currentLevel_, levelCap_, maxLevelCombatStats.speed - baseCombatStats.speed);
            
            return baseCombatStats.armor + _leveledSpeed;
        }
        
        #endregion
    }
}