using System.Collections.Generic;
using UnityEngine;

namespace BaseCore
{
    public enum DamageType
    {
        Weapon,
        Physical,
        Magical,
    }

    public struct DamageInfo
    {
        public int DamageAmount;
        public GameObject Source;
        public DamageType DamageType;
        public List<string> tags;

        public DamageInfo(int damageAmount_, GameObject source_, List<string> tags_ = null ,DamageType damageType_ = DamageType.Weapon)
        {
            DamageAmount = damageAmount_;
            Source = source_;
            DamageType = damageType_;
            tags = tags_;
        }
    }
}