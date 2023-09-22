using System;
using BaseCore;
using Character.CharacterComponents;
using CustomEvent;
using Items.Inventory;
using NaughtyAttributes;
using Player;
using ScriptableObjectData.CharacterData;
using UnityEngine;

namespace Character
{
    public class AllyCharacter : CharacterBase
    {
        private AllyData allyData;
        public override CharacterHealth health => allyData.health;
        public override CharacterMana mana => allyData.mana;

        public override StatusEffectReceiver statusEffectReceiver => allyData.statusEffectReceiver;
        public override int level => allyData.LevelData.CurrentLevel;
        public override StatsGrowth statsData => allyData.statsData;
        
        protected override void Awake()
        {
            statusEffectReceiver = new StatusEffectReceiver(this);
            allyData = characterData as AllyData;
        }
        
        private void OnEnable()
        {
            health.OnCharacterEnable();
            mana.OnCharacterEnable();
            allyData.statusEffectReceiver.SetCharacter(this);
        }
        
        public override void SetLevel(int level_)
        {
        }
        
        [Button("Remove HP and Mana")]
        public void Refill()
        {
            health.RefillHealth();
            mana.RefreshMana();
        }

        [Button("Remove some mana")]
        private void RemoveMana()
        {
            mana.UseMana(10);
        }

    }
}
