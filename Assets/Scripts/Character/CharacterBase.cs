using System;
using BaseCore;
using Character.CharacterComponents;
using ScriptableObjectData.CharacterData;
using UnityEngine;

namespace Character
{
    public class CharacterBase : MonoBehaviour, IDamagable, IHealable
    {
        [field: SerializeField]public CharacterData characterData { get; protected set; }
        
        #region Core Components
        
        public virtual CharacterHealth health { get; protected set; }
        public virtual CharacterMana mana { get; protected set; }
        public virtual StatusEffectReceiver statusEffectReceiver { get; protected set; }

        public virtual StatsGrowth statsData
        {
            get
            {
                return mStatsData ??= characterData.statsData.Clone();
            }
        }

        #endregion

        public virtual int level { get; protected set; }
        
        private StatsGrowth mStatsData;

        public CombatStats stats => statsData.GetTotalStats(level);
        public bool IsAlive => health.IsAlive;

        protected virtual void Awake()
        {
            health = new CharacterHealth(this);
            mana = new CharacterMana(this);
            statusEffectReceiver = new StatusEffectReceiver(this);
        }
        
        public void TakeDamage(DamageInfo damageInfo_)
        {
            health.TakeDamage(damageInfo_);
        }

        public void Heal(HealInfo healInfo_, bool isOverHeal_ = false)
        {
            health.ReceiveHeal(healInfo_, isOverHeal_);
        }
        
        public virtual void SetLevel(int level_)
        {
            level = level_;
            health.RefillHealth();
            mana.RefreshMana();
        }
    }
}