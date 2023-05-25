using System;
using CustomEvent;
using Items.Inventory;
using Player;
using UnityEngine;

namespace Character
{
    public class PlayerCharacter : CharacterBase
    {
        [field: SerializeField] public PlayerInventory playerInventory { get; private set; }
    }
}