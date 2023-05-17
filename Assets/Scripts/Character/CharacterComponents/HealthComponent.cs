using BaseCore;
using CustomEvent;
using UnityEngine;

namespace Character
{
    public class HealthComponent : MonoBehaviour
    {
        [field: Header("Properties")]
        [field: SerializeField] public int MaxHp { get; private set; }

        [field: Header("Events")]
        public readonly Evt<HealthComponent, DamageInfo> OnTakeDamage = new Evt<HealthComponent, DamageInfo>();
        public readonly Evt<HealthComponent, DamageInfo> OnDeath = new Evt<HealthComponent, DamageInfo>();
        public readonly Evt<HealthComponent, HealInfo> OnHeal = new Evt<HealthComponent, HealInfo>();

        public bool IsInvincible { get; set; }
        public bool CanOverHeal { get; set; }
        [field: NaughtyAttributes.ReadOnly] public int CurrentHp { get; private set; }
        public float HpPercentage => (float) CurrentHp / MaxHp;
        public bool IsAlive => CurrentHp > 0;

        private void Start()
        {
            CurrentHp = MaxHp;
        }
        
        public void Initialize(int maxHp_)
        {
            MaxHp = maxHp_;
            CurrentHp = maxHp_;
        }

        private void SetCurrentHP(int newCurrentHP_)
        {
            CurrentHp = Mathf.Clamp(
                newCurrentHP_,
                0,
                CanOverHeal ? int.MaxValue : MaxHp);

            if (CurrentHp <= MaxHp) CanOverHeal = false;
        }

        public void SetMaxHP(int newMaxHP_, bool willScaleCurrentHP = false)
        {
            MaxHp = newMaxHP_;

            if (willScaleCurrentHP)
            {
                var _scale = newMaxHP_ / MaxHp;
                SetCurrentHP(CurrentHp * _scale);
            }
        }

        public void TakeDamage(DamageInfo damageInfo)
        {
            SetCurrentHP(CurrentHp - damageInfo.DamageAmount);

            OnTakeDamage.Invoke(this, damageInfo);

            if (CurrentHp > 0) return;

            OnDeath.Invoke(this, damageInfo);
        }

        public void ReceiveHeal(HealInfo healInfo, bool canOverHeal_ = false)
        {
            CanOverHeal = canOverHeal_;

            SetCurrentHP(CurrentHp + healInfo.HealAmount);

            OnHeal.Invoke(this, healInfo);

            // if (CurrentHp < MaxHp) return;
            //
            // OnHealthUpdate.Invoke(this);
        }
        
        #if UNITY_EDITOR
        
        [NaughtyAttributes.Button("Take Damage")]
        public void TestTakeDamage()
        {
            var _info = new DamageInfo()
            {
                DamageAmount = 20,
            };
            TakeDamage(_info);
        }
        
        #endif
    }
}