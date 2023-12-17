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
    public class PlayerCharacter : CharacterBase
    {
        [field: SerializeField] public PlayerInventory playerInventory { get; private set; }

        private PlayerData playerData;
        public override CharacterHealth health => playerData.health;
        public override CharacterMana mana => playerData.mana;
        public override StatusEffectReceiver statusEffectReceiver => playerData.statusEffectReceiver;
        public override int level => playerData.LevelData.CurrentLevel;
        public override StatsGrowth statsData => playerData.statsData;

        protected override void Awake()
        {
            statusEffectReceiver = new StatusEffectReceiver(this);
            playerData = characterData as PlayerData;
        }

        private void OnEnable()
        {
            health.OnCharacterEnable();
            mana.OnCharacterEnable();
            playerData.statusEffectReceiver.SetCharacter(this);
        }

        public override void SetLevel(int level_)
        {
        }

        [Button("Remove HP and Mana")]
        public void Refill()
        {
            health.RefillHealth();
            mana.RefreshMana();

            if (playerData.totalPartyData != null)
            {
                foreach (AllyData o in playerData.totalPartyData)
                {
                    o.Refill();
                }
            }
        }

        [Button("Remove some mana")]
        private void RemoveMana()
        {
            mana.UseMana(10);
        }
        
    }
}