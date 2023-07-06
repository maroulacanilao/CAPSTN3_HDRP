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
        public struct AddExpInfo
        {
            public int prevExp;
            public int newExp;
            public bool leveledUp;
            public int prevLevel;
            public int newLevel;
            public int addedExp;
            public int prevScaledLvlExp;
            public int prevScaledLvlExpNeeded;
            public int prevNeededExp;
        }
        
        [field: CurveRange(0, 0, 1, 1, EColor.Red)]
        [field: SerializeField] public AnimationCurve expLvlCurve { get; private set; }
        [field: SerializeField] public int LevelCap { get; private set; } = 100;
        [field: SerializeField] public int ExperienceCap { get; private set; } = 10000;

        private int level = 1;
        
        //Remove this later
        [SerializeField] private int totalExperience;

        public int CurrentLevel => level;
        public int TotalExperience => totalExperience;
        public int NextLevelExperience => EvaluateExperience(CurrentLevel + 1);
        public int PrevLevelExperience => CurrentLevel == 0 ? 0 : EvaluateExperience(CurrentLevel);
        public int CurrentLevelExperience => totalExperience - PrevLevelExperience;
        public int ExperienceNeededToLevelUp => NextLevelExperience - TotalExperience;
        
        public int CurrentExperienceNeeded => EvaluateExperience(CurrentLevel + 1) - EvaluateExperience(CurrentLevel);
        
        public static readonly Evt<AddExpInfo> OnExperienceChanged = new Evt<AddExpInfo>();

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
            var _prevNeededExp = NextLevelExperience;
            var _prevExpScaled = CurrentLevelExperience;
            var _prevExpNeededScaled = CurrentExperienceNeeded;
            
            var _prevExp = totalExperience;
            var _prevLevel = level;
            totalExperience += expAmount_;
            totalExperience = Mathf.Clamp(totalExperience, 0, ExperienceCap);
            
            
            level = EvaluateLevel(totalExperience);
            
            var _didLevelUp = _prevLevel < level;

            var _info = new AddExpInfo
            {
                prevExp = _prevExp,
                newExp = totalExperience,
                leveledUp = _didLevelUp,
                prevLevel = _prevLevel,
                newLevel = level,
                addedExp = expAmount_,
                prevScaledLvlExp = _prevExpScaled,
                prevScaledLvlExpNeeded = _prevExpNeededScaled,
                prevNeededExp = _prevNeededExp
            };
            
            Debug.Log($"prevExp: {_prevExp} | newExp: {totalExperience} | leveledUp: {_didLevelUp} | prevLevel: {_prevLevel} | newLevel: {level} | addedExp: {expAmount_} | prevScaledLvlExp: {_prevExpScaled} | prevScaledLvlExpNeeded: {_prevExpNeededScaled} | prevNeededExp: {_prevNeededExp}");
            
            OnExperienceChanged.Invoke(_info);
            return _didLevelUp;
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