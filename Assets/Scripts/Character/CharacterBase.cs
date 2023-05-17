using BaseCore;
using ScriptableObjectData.CharacterData;
using UnityEngine;

namespace Character
{
    public class CharacterBase : MonoBehaviour
    {
        [field: SerializeField] public HealthComponent healthComponent { get; private set; }
        [field: SerializeField] public ManaComponent manaComponent { get; private set; }
        [field: SerializeField] public StatusEffectReceiver statusEffectReceiver { get; protected set; }
        [field: SerializeField] public CharacterData characterData { get; private set; }

        public int level { get; private set; }
        public StatsGrowth statsData => characterData.statsData;
        public CombatStats stats => statsData.GetLeveledStats(level);
        public bool IsAlive => healthComponent.IsAlive;

        protected virtual void Awake()
        {
            SetMaxHpAndMana();
        }
        
        private void SetMaxHpAndMana()
        {
            healthComponent.Initialize(stats.maxHp);
            manaComponent.Initialize(stats.maxMana);
        }
    }
}