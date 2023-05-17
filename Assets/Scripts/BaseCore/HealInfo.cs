using UnityEngine;

namespace BaseCore
{
    public struct HealInfo
    {
        public readonly int HealAmount;
        public readonly GameObject Source;
        public readonly bool CanOverHeal;

        public HealInfo(int healAmount_, GameObject source_ = null, bool canOverHeal_ = false)
        {
            HealAmount = healAmount_;
            Source = source_;
            CanOverHeal = canOverHeal_;
        }
    }
}