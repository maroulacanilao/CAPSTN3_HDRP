using CustomEvent;
using CustomHelpers;
using NaughtyAttributes;
using ScriptableObjectData.CharacterData;
using UnityEngine;

namespace BaseCore
{
    [System.Serializable]
    public class PlayerLevel
    {
        [field: CurveRange(0, 0, 1, 1, EColor.Red)]
        [field: SerializeField] public AnimationCurve expLvlCurve { get; private set; }
        [field: SerializeField] public int LevelCap { get; private set; } = 100;
        [field: SerializeField] public int ExperienceCap { get; private set; } = 10000;

        private int level = 1;
        private int totalExperience;

        public int CurrentLevel => level;
        public int TotalExperience => totalExperience;
        public int NextLevelExperience => EvaluateExperience(CurrentLevel + 1);
        public int PrevLevelExperience => CurrentLevel == 0 ? 0 : EvaluateExperience(CurrentLevel);
        public int CurrentLevelExperience => totalExperience - PrevLevelExperience;
        public int ExperienceNeededToLevelUp => NextLevelExperience - TotalExperience;
        
        public static readonly Evt<PlayerLevel> OnLevelUp = new Evt<PlayerLevel>();
        
        public PlayerLevel Initialize(int totalExperience_)
        {
            totalExperience = totalExperience_;
            level = EvaluateLevel(totalExperience);
            return this;
        }

        /// <summary>
        /// return true if player leveled up
        /// </summary>
        /// <param name="expAmount_"></param>
        /// <returns></returns>
        public bool AddExp(int expAmount_)
        {
            var _prevLevel = level;
            totalExperience += expAmount_;
            totalExperience = Mathf.Clamp(totalExperience, 0, ExperienceCap);

            level = EvaluateLevel(totalExperience);
            
            if (_prevLevel == level) return false;
            
            OnLevelUp.Invoke(this);
            return true;
        }

        public int EvaluateExperience(int level_)
        {
            if(level_ <= 0) return 0;
            
            return expLvlCurve.EvaluateScaledCurve(level_, LevelCap, ExperienceCap);
        }

        public int EvaluateLevel(int experience_)
        {
            if(experience_ <= 0) return 1;
            
            for (int i = 0; i < LevelCap; i++)
            {
                if (experience_ >= EvaluateExperience(i) && experience_ < EvaluateExperience(i + 1))
                {
                    return Mathf.Clamp(i, 1, LevelCap);
                }
                // Debug.Log($"Current XP: {experience_} | Level: {i} | XP Needed: {EvaluateExperience(i)} | XP Needed Next Level: {EvaluateExperience(i + 1)}");
            }
            return LevelCap;
        }

        public void ResetExperience()
        {
            totalExperience = 0;
            level = 1;
        }
    }
}