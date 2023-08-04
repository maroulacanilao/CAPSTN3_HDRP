using System;
using System.Collections.Generic;
using BaseCore;
using Character;
using ScriptableObjectData.CharacterData;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BattleSystem
{
    public enum AttackResultType { Miss, Hit, Critical,Weakness }
    
    public struct AttackResult
    {
        public AttackResultType attackResultType;
        public DamageInfo damageInfo;
    }
    
    public static class BattleHelper
    {
        public static BattleData mBattleData;

        public static BattleData battleData
        {
            get
            {
                if (mBattleData == null)
                {
                    mBattleData = Resources.Load<BattleData>("Data");
                }
                return mBattleData;
            }
            set => mBattleData = value;
        }
        
        public static AttackResult GetAttackResult(this BattleCharacter attacker_, BattleCharacter target_, DamageInfo baseDamageInfo_)
        {
            Random.InitState(Time.timeSinceLevelLoad.GetHashCode());
            var _attackResult = new AttackResult() { attackResultType = AttackResultType.Miss, damageInfo = baseDamageInfo_};
            
            if (IsAttackEvaded(attacker_.TotalStats, target_.TotalStats))
            {
                _attackResult.attackResultType = AttackResultType.Miss;
                _attackResult.damageInfo.DamageAmount = 0;
                return _attackResult;
            }
            
            var _isWeak = target_.character.characterData.IsWeakAgainst(baseDamageInfo_.tags);
            
            // if weak against
            if (_isWeak)
            {
                _attackResult.damageInfo.DamageAmount = Mathf.RoundToInt(_attackResult.damageInfo.DamageAmount * battleData.weakDmgMod);
            }
            

            // if magic Damage
            if (_attackResult.damageInfo.DamageType == DamageType.Magical)
            {
                _attackResult.damageInfo.DamageAmount = CalculateMagicDamage(_attackResult.damageInfo.DamageAmount, target_.TotalStats);
                _attackResult.attackResultType = _isWeak ? AttackResultType.Weakness : AttackResultType.Hit;
                return _attackResult;
            }
            
            // if physical / weapon Damage
            var _dmg  = GetArmorDamageMultiplier(target_.TotalStats) * _attackResult.damageInfo.DamageAmount;

            var _isCritical = IsAttackCritical(attacker_.TotalStats, target_.TotalStats);

            if (_isCritical) _dmg = Mathf.RoundToInt(_dmg * battleData.crtDmgMod);

            _attackResult.damageInfo.DamageAmount = Mathf.RoundToInt(_dmg);
            _attackResult.damageInfo.DamageAmount = Mathf.Clamp(_attackResult.damageInfo.DamageAmount, 1, int.MaxValue);

            _attackResult.attackResultType = _isCritical ? AttackResultType.Critical : AttackResultType.Hit;
            
            return _attackResult;
        }
        
        private static float GetArmorDamageMultiplier(CombatStats targetStats_)
        {
            return 1 - 0.052f * targetStats_.defense / (0.9f + 0.048f * Mathf.Abs(targetStats_.defense));
        }

        private static int CalculateMagicDamage(int magicDamage_, CombatStats targetStats_)
        {
            float _dmgMult = 1f - (targetStats_.defense / 100f);
            int _totalDmg = (int)(magicDamage_ * _dmgMult);
            _totalDmg = Mathf.Clamp(_totalDmg, 5, int.MaxValue);
            return _totalDmg;
        }
        
        // private static bool IsAttackEvaded(CombatStats attacker_, CombatStats target_)
        // {
        //     float _attackerDamage = attacker_.strength * battleData.evaStrModifier;
        //     
        //     float _evadeChance = (1 - (_attackerDamage / (attacker_.strength + target_.defense) ) ) * battleData.evasionChcMod;
        //     _evadeChance = Mathf.Clamp(_evadeChance, 0.02f, 0.95f);
        //     
        //     Debug.Log($"Evade Chance: {_evadeChance}");
        //     return Random.value < _evadeChance;
        // }
        
        public static bool IsAttackEvaded(CombatStats attacker_, CombatStats target_)
        {
            var _evasionChance = (target_.speed) / (float)(attacker_.speed + target_.speed);
            _evasionChance *= battleData.evasionChcMod;
            
            _evasionChance = Mathf.Clamp(_evasionChance, battleData.evaMinMax.x, battleData.evaMinMax.y);
            
            var _rng = UnityEngine.Random.value;
            
            Debug.Log($"<color=yellow>Evade Chance: {_evasionChance} || RNG: {_rng}</color>");
            return _rng < _evasionChance;
        }

        
        // private static bool IsAttackCritical(CombatStats attacker_, CombatStats target_)
        // {
        //     float _targetArmor = target_.defense * battleData.crtChcArmorMod;
        //     
        //     float _criticalChance = (1 - (_targetArmor / (attacker_.defense + target_.strength) ) ) * battleData.crtChcMod;
        //     _criticalChance = Mathf.Clamp(_criticalChance, 0.02f, 0.95f);
        //     
        //     Debug.Log($"Critical Chance: {_criticalChance}");
        //     return Random.value < _criticalChance;
        // }

        private static bool IsAttackCritical(CombatStats attacker_, CombatStats target_)
        {
            // float _crtChc = ((battleData.crtChcAtkStrModifier * attacker_.strength) + (battleData.crtChcAtkSpdModifier * attacker_.speed)) 
            //                           - (battleData.crtChcTarSpdModifier * target_.speed);
            
            var _crtChc = attacker_.speed / (float)(attacker_.strength + target_.defense);
            _crtChc *= battleData.crtChcMod;
            _crtChc = Mathf.Clamp(_crtChc, battleData.crtMinMax.x, battleData.crtMinMax.y);
            
            float _rng = UnityEngine.Random.value;

            Debug.Log($"<color=red>Critical Chance: {_crtChc} || RNG: {_rng}</color>");
            return _rng < _crtChc;
        }

        private static bool IsWeakAgainst(this CharacterData target_, List<string> tags)
        {
            if(tags == null || tags.Count == 0) return false;
            
            foreach (var _tag in tags)
            {
                if (target_.weaknessTags
                    .Exists(tag =>
                        string.Equals(tag, _tag, StringComparison.InvariantCultureIgnoreCase)))
                {
                    return true;
                }
            }
            
            return false;
        }
    }
}