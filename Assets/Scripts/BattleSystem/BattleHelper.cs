using BaseCore;
using Character;
using UnityEngine;

namespace BattleSystem
{
    public enum AttackResultType { Miss, Hit, Critical }
    
    public struct AttackResult
    {
        public AttackResultType attackResultType;
        public DamageInfo damageInfo;
    }
    
    public static class BattleHelper
    {
        public static AttackResult DamageTarget(this BattleCharacter attacker_, BattleCharacter target_, DamageInfo baseDamageInfo_)
        {
            var _attackResult = new AttackResult() { attackResultType = AttackResultType.Miss, damageInfo = baseDamageInfo_};
            if (IsAttackEvaded(attacker_.TotalStats, target_.TotalStats))
            {
                _attackResult.attackResultType = AttackResultType.Miss;
                _attackResult.damageInfo.DamageAmount = 0;
                return _attackResult;
            }

            // if magic Damage
            if (_attackResult.damageInfo.DamageType == DamageType.Magical)
            {
                _attackResult.damageInfo.DamageAmount = CalculateMagicDamage(_attackResult.damageInfo.DamageAmount, target_.TotalStats);
                _attackResult.attackResultType = AttackResultType.Hit;
                return _attackResult;
            }
            
            // if physical / weapon Damage
            var _dmg  = GetArmorDamageMultiplier(target_.TotalStats) * _attackResult.damageInfo.DamageAmount;
            
            bool _isCritical = IsAttackCritical(attacker_.TotalStats, target_.TotalStats);
            if (_isCritical) _dmg *= 1.5f;
            
            _attackResult.damageInfo.DamageAmount = Mathf.RoundToInt(_dmg);

            _attackResult.attackResultType = _isCritical ? AttackResultType.Critical : AttackResultType.Hit;
            
            return _attackResult;
        }
        
        private static float GetArmorDamageMultiplier(CombatStats targetStats_)
        {
            return 1 - 0.052f * targetStats_.armor / (0.9f + 0.048f * Mathf.Abs(targetStats_.armor));
        }

        private static int CalculateMagicDamage(int magicDamage_, CombatStats targetStats_)
        {
            float _dmgMult = 1f - (targetStats_.magicResistance / 100f);
            int _totalDmg = (int)(magicDamage_ * _dmgMult);
            return _totalDmg;
        }
        
        private static bool IsAttackEvaded(CombatStats attacker_, CombatStats target_)
        {
            float _evadeChance = 1 - ((float)attacker_.accuracy / (attacker_.accuracy + target_.speed));
            return Random.value < _evadeChance;
        }
        
        private static bool IsAttackCritical(CombatStats attacker_, CombatStats target_)
        {
            // Calculate the attacker's base critical hit chance
            float _baseCriticalChance = Mathf.Clamp01(attacker_.accuracy / 100f) * 0.05f;

            // Calculate the attacker's bonus critical hit chance based on speed
            float _speedBonus = Mathf.Clamp01(attacker_.speed / 100f) * 0.1f;

            // Calculate the final critical hit chance
            float _criticalChance = Mathf.Clamp01(_baseCriticalChance + _speedBonus);
            
            _criticalChance *= Random.Range(0.9f, 1.1f);

            if (IsAttackEvaded(attacker_, target_)) return false;

            return Random.value < _criticalChance;
        }
    }
}