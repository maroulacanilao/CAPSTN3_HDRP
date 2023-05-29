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
        
        public CharacterHealth health { get; protected set; }
        public CharacterMana mana { get; protected set; }
        public StatusEffectReceiver statusEffectReceiver { get; protected set; }

        #endregion

        public int level { get; private set; }
        public StatsGrowth statsData => characterData.statsData;
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
    }
}