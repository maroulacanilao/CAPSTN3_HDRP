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

        private int currLvl => EvaluateExperience(totalExperience);
        private int totalExperience;

        public int CurrentLevel => currLvl;
        public int TotalExperience => totalExperience;
        public int NextLevelExperience => EvaluateExperience(currLvl + 1);
        public int PrevLevelExperience => currLvl <= 0 ? 0 : EvaluateExperience(currLvl - 1);
        public int CurrentLevelExperience => totalExperience - PrevLevelExperience;
        public int ExperienceNeededToLevelUp => NextLevelExperience - totalExperience;

        /// <summary>
        /// return true if player leveled up
        /// </summary>
        /// <param name="expAmount_"></param>
        /// <returns></returns>
        public bool AddExp(int expAmount_)
        {
            var _prevLvl = currLvl;
            
            totalExperience += expAmount_;
            totalExperience = Mathf.Clamp(totalExperience, 0, ExperienceCap);
            
            return _prevLvl != currLvl;
        }

        public int EvaluateExperience(int level_)
        {
            return expLvlCurve.EvaluateScaledCurve(level_, LevelCap, ExperienceCap);
        }
    }
}