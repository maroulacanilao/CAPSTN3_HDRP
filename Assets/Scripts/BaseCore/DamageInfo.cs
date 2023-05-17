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
        public RaycastHit HitInfo;
        public Vector3 Position;

        public DamageInfo(int damageAmount_, GameObject source_, DamageType damageType_ = DamageType.Weapon, RaycastHit hitInfo_ = default, Vector3 position_ = default)
        {
            DamageAmount = damageAmount_;
            Source = source_;
            DamageType = damageType_;
            HitInfo = hitInfo_;
            Position = position_;
        }
    }
}