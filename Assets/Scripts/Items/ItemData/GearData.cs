using Character;
using CustomHelpers;
using NaughtyAttributes;
using UnityEngine;

namespace Items.ItemData
{
    public abstract class GearData : ItemData
    {
        [field: MinMaxSlider(0, 15)] [field: BoxGroup("Possible Stats")]
        [field: SerializeField] public Vector2Int VitalityRange { get; private set; }
        
        [field: MinMaxSlider(0, 15)] [field: BoxGroup("Possible Stats")]
        [field: SerializeField] public Vector2Int StrengthRange { get; private set; }
        
        [field: MinMaxSlider(0, 15)] [field: BoxGroup("Possible Stats")]
        [field: SerializeField] public Vector2Int IntelligenceRange { get; private set; }
        
        [field: MinMaxSlider(0, 15)] [field: BoxGroup("Possible Stats")]
        [field: SerializeField] public Vector2Int DefenseRange { get; private set; }

        [field: MinMaxSlider(0, 15)] [field: BoxGroup("Possible Stats")]
        [field: SerializeField] public Vector2Int SpeedRange { get; private set; }


        public virtual CombatStats GetRandomStats()
        {
            return new CombatStats()
            {
                vitality = VitalityRange.GetRandomInRange(),
                strength = StrengthRange.GetRandomInRange(),
                intelligence = IntelligenceRange.GetRandomInRange(),
                defense = DefenseRange.GetRandomInRange(),
                speed = SpeedRange.GetRandomInRange()
            };
        }
        
        public virtual CombatStats GetRandomStats(float modifier_)
        {
            return new CombatStats()
            {
                vitality = VitalityRange.GetRandomInRange(modifier_),
                strength = StrengthRange.GetRandomInRange(modifier_),
                intelligence = IntelligenceRange.GetRandomInRange(modifier_),
                defense = DefenseRange.GetRandomInRange(modifier_),
                speed = SpeedRange.GetRandomInRange(modifier_)
            };
        }
    }
}