using BaseCore;
using CustomEvent;
using UnityEngine;

namespace Character.CharacterComponents
{
    [System.Serializable]
    public class CharacterHealth : CharacterCore
    {
        [field: Header("Events")]
        public readonly Evt<CharacterHealth, DamageInfo> OnTakeDamage = new Evt<CharacterHealth, DamageInfo>();
        public readonly Evt<CharacterHealth, DamageInfo> OnDeath = new Evt<CharacterHealth, DamageInfo>();
        public readonly Evt<CharacterHealth, HealInfo> OnHeal = new Evt<CharacterHealth, HealInfo>();
        public readonly Evt<CharacterHealth> OnManuallyUpdateHealth = new Evt<CharacterHealth>();

        public bool IsInvincible { get; set; }
        public bool CanOverHeal { get; set; }
        public virtual int MaxHp => character.stats.vitality;
        public int CurrentHp { get; protected set; }
        public float HpPercentage => (float) CurrentHp / MaxHp;
        public bool IsAlive => CurrentHp > 0;

        public CharacterHealth(CharacterBase character_) : base(character_)
        {
            if(character_ == null) return;
            CurrentHp = MaxHp;
        }

        protected virtual void SetCurrentHp(int newCurrentHP_)
        {
            CurrentHp = Mathf.Clamp(
                newCurrentHP_,
                0,
                CanOverHeal ? int.MaxValue : MaxHp);

            if (CurrentHp <= MaxHp) CanOverHeal = false;
        }

        public virtual void TakeDamage(DamageInfo damageInfo)
        {
            SetCurrentHp(CurrentHp - damageInfo.DamageAmount);

            OnTakeDamage.Invoke(this, damageInfo);

            if (CurrentHp > 0) return;

            OnDeath.Invoke(this, damageInfo);
        }

        public virtual void ReceiveHeal(HealInfo healInfo, bool canOverHeal_ = false)
        {
            CanOverHeal = canOverHeal_;

            SetCurrentHp(CurrentHp + healInfo.HealAmount);

            OnHeal.Invoke(this, healInfo);

            // if (CurrentHp < MaxHp) return;
            //
            // OnHealthUpdate.Invoke(this);
        }
        
        public virtual void RefillHealth()
        {
            SetCurrentHp(MaxHp);
            OnManuallyUpdateHealth.Invoke(this);
        }
    }
}