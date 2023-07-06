using System;
using BaseCore;
using Character.CharacterComponents;
using CustomEvent;
using Items.Inventory;
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

        public void Refill()
        {
            health.RefillHealth();
            mana.RefreshMana();
        }
    }
}