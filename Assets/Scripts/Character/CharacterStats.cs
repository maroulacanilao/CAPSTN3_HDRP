using System;

namespace Character
{
    [Serializable]
    public struct CombatStats
    {
        public int maxHp;
        public int maxMana;
        public int physicalDamage;
        public int armor;
        public int magicDamage;
        public int magicResistance;
        public int accuracy;
        public int speed;

        public static CombatStats operator +(CombatStats a_, CombatStats b_)
        {
            return new CombatStats
            {
                maxHp = a_.maxHp + b_.maxHp,
                maxMana = a_.maxMana + b_.maxMana,
                physicalDamage = a_.physicalDamage + b_.physicalDamage,
                armor = a_.armor + b_.armor,
                magicDamage = a_.magicDamage + b_.magicDamage,
                magicResistance = a_.magicResistance + b_.magicResistance,
                accuracy = a_.accuracy + b_.accuracy,
                speed = a_.speed + b_.speed
            };
        }

        public static CombatStats operator -(CombatStats a_, CombatStats b_)
        {
            return new CombatStats
            {
                maxHp = a_.maxHp - b_.maxHp,
                maxMana = a_.maxMana - b_.maxMana,
                physicalDamage = a_.physicalDamage - b_.physicalDamage,
                armor = a_.armor - b_.armor,
                magicDamage = a_.magicDamage - b_.magicDamage,
                magicResistance = a_.magicResistance - b_.magicResistance,
                accuracy = a_.accuracy - b_.accuracy,
                speed = a_.speed - b_.speed
            };
        }
        
        public static CombatStats operator *(CombatStats a_, CombatStats b_)
        {
            return new CombatStats
            {
                maxHp = a_.maxHp * b_.maxHp,
                maxMana = a_.maxMana * b_.maxMana,
                physicalDamage = a_.physicalDamage * b_.physicalDamage,
                armor = a_.armor * b_.armor,
                magicDamage = a_.magicDamage * b_.magicDamage,
                magicResistance = a_.magicResistance * b_.magicResistance,
                accuracy = a_.accuracy * b_.accuracy,
                speed = a_.speed * b_.speed
            };
        }
        
        public static CombatStats operator *(CombatStats a_, float b_)
        {
            return new CombatStats
            {
                maxHp = UnityEngine.Mathf.RoundToInt(a_.maxHp * b_),
                maxMana = UnityEngine.Mathf.RoundToInt(a_.maxMana * b_),
                physicalDamage = UnityEngine.Mathf.RoundToInt(a_.physicalDamage * b_),
                armor = UnityEngine.Mathf.RoundToInt(a_.armor * b_),
                magicDamage = UnityEngine.Mathf.RoundToInt(a_.magicDamage * b_),
                magicResistance = UnityEngine.Mathf.RoundToInt(a_.magicResistance * b_),
                accuracy = UnityEngine.Mathf.RoundToInt(a_.accuracy * b_),
                speed = UnityEngine.Mathf.RoundToInt(a_.speed * b_)
            };
        }
        
        public static CombatStats operator /(CombatStats a_, CombatStats b_)
        {
            return new CombatStats
            {
                maxHp = a_.maxHp / b_.maxHp,
                maxMana = a_.maxMana / b_.maxMana,
                physicalDamage = a_.physicalDamage / b_.physicalDamage,
                armor = a_.armor / b_.armor,
                magicDamage = a_.magicDamage / b_.magicDamage,
                magicResistance = a_.magicResistance / b_.magicResistance,
                accuracy = a_.accuracy / b_.accuracy,
                speed = a_.speed / b_.speed
            };
        }
    }

    public static class CombatStatsHelper
    {
        public static bool IsEmpty(this CombatStats source)
        {
            return source is {maxHp: 0, maxMana: 0, physicalDamage: 0, accuracy: 0, magicResistance: 0, armor: 0, magicDamage: 0, speed: 0};
        }
    }
}