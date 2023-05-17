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
        
        [Header("Components To Disable On Freeze")]
        [SerializeField] private MonoBehaviour[] scriptsToDisable;
        
        public static readonly Evt<bool> OnPlayerCanMove = new Evt<bool>();
        protected override void Awake()
        {
            base.Awake();
            OnPlayerCanMove.AddListener(CanMove);
        }
        
        protected void Start()
        {
            playerInventory.InitializeInventory();
        }

        private void OnDestroy()
        {
            OnPlayerCanMove.RemoveListener(CanMove);
            playerInventory.DeInitializeInventory();
        }
        
        public void CanMove(bool canMove_)
        {
            foreach (var _component in scriptsToDisable)
            {
                _component.enabled = canMove_;
            }
        }
    }
}