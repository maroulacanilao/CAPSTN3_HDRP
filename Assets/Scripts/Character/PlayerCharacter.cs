using System;
using Character.CharacterComponents;
using CustomEvent;
using Items.Inventory;
using Player;
using UnityEngine;

namespace Character
{
    public class PlayerCharacter : CharacterBase
    {
        [field: SerializeField] public PlayerInventory playerInventory { get; private set; }

        protected override void Awake()
        {
            health = new PlayerCharacterHealth(this);
            mana = new PlayerCharacterMana(this);
            statusEffectReceiver = new StatusEffectReceiver(this);
        }

        private void OnEnable()
        {
            health.OnCharacterEnable();
            mana.OnCharacterEnable();
        }
    }
}