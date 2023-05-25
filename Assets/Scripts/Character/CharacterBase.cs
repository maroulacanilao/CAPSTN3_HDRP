using BaseCore;
using ScriptableObjectData.CharacterData;
using UnityEngine;

namespace Character
{
    public class CharacterBase : MonoBehaviour, IDamagable, IHealable
    {
        [field: SerializeField] public CharacterHealth health { get; private set; }
        [field: SerializeField] public CharacterMana mana { get; private set; }
        [field: SerializeField] public StatusEffectReceiver statusEffectReceiver { get; protected set; }
        [field: SerializeField] public CharacterData characterData { get; private set; }

        public int level { get; private set; }
        public StatsGrowth statsData => characterData.statsData;
        public CombatStats stats => statsData.GetTotalStats(level);
        public bool IsAlive => health.IsAlive;
        
        public void TakeDamage(DamageInfo damageInfo_) => health.TakeDamage(damageInfo_);
        
        public void Heal(HealInfo healInfo_, bool isOverHeal_ = false) => health.ReceiveHeal(healInfo_, isOverHeal_);
    }
}