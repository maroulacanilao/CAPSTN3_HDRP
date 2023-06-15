using System;
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
    }
}