using System;

namespace Character
{
    [Serializable]
    public struct CombatStats
    {
        public int vitality;
        public int mana => this.GetMana();
        public int strength;
        public int intelligence;
        public int defense;
        public int speed;

        public static CombatStats operator +(CombatStats a_, CombatStats b_)
        {
            return new CombatStats
            {
                vitality = a_.vitality + b_.vitality,
                strength = a_.strength + b_.strength,
                intelligence = a_.intelligence + b_.intelligence,
                defense = a_.defense + b_.defense,
                speed = a_.speed + b_.speed,
            };
        }

        public static CombatStats operator -(CombatStats a_, CombatStats b_)
        {
            return new CombatStats
            {
                vitality = a_.vitality - b_.vitality,
                strength = a_.strength - b_.strength,
                intelligence = a_.intelligence - b_.intelligence,
                defense = a_.defense - b_.defense,
                speed = a_.speed - b_.speed,
            };
        }
        
        public static CombatStats operator *(CombatStats a_, CombatStats b_)
        {
            return new CombatStats
            {
                vitality = UnityEngine.Mathf.RoundToInt(a_.vitality * b_.defense),
                strength = UnityEngine.Mathf.RoundToInt(a_.strength * b_.strength),
                intelligence = UnityEngine.Mathf.RoundToInt(a_.intelligence * b_.intelligence),
                defense = UnityEngine.Mathf.RoundToInt(a_.defense * b_.defense),
                speed = UnityEngine.Mathf.RoundToInt(a_.speed * b_.speed),
            };
        }
        
        public static CombatStats operator *(CombatStats a_, float b_)
        {
            return new CombatStats
            {
                vitality = UnityEngine.Mathf.RoundToInt(a_.vitality * b_),
                strength = UnityEngine.Mathf.RoundToInt(a_.strength * b_),
                intelligence = UnityEngine.Mathf.RoundToInt(a_.intelligence * b_),
                defense = UnityEngine.Mathf.RoundToInt(a_.defense * b_),
                speed = UnityEngine.Mathf.RoundToInt(a_.speed * b_),
            };
        }

        public static CombatStats operator /(CombatStats a_, CombatStats b_)
        {
            return new CombatStats
            {
                vitality = a_.vitality / b_.vitality,
                strength = a_.strength / b_.strength,
                intelligence = a_.intelligence / b_.intelligence,
                defense = a_.defense / b_.defense,
                speed = a_.speed / b_.speed,
            };
        }
    }

    public static class StatsHelper
    {
        public static float intelToManaMultiplier = 1f;
        
        public static bool IsEmpty(this CombatStats source)
        {
            return source is {vitality: 0, mana: 0, strength: 0, defense: 0};
        }

        public static int GetMana(this CombatStats source)
        {
            return UnityEngine.Mathf.RoundToInt(source.intelligence * intelToManaMultiplier);
        }
    }
}