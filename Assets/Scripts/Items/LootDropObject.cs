using System;
using BaseCore;
using CustomEvent;
using ObjectPool;
using ScriptableObjectData;
using UI;
using UI.LootMenu;
using UnityEngine;

namespace Items
{
    public class LootDropObject : InteractableObject, IPoolable
    {
        [SerializeField] GameObject ghostObject;
        public LootDrop lootDrop { get; protected set; }
        
        public static readonly Evt<LootDropObject> OnLootInteract = new Evt<LootDropObject>();

        public LootDropObject Initialize(LootDrop lootDrop_)
        {
            lootDrop = lootDrop_;

            return this;
        }
        
        public void OnSpawn()
        {
            
        }
        
        public void OnDeSpawn()
        {
            lootDrop = default;
            OnExit.Invoke(this);
        }

        protected override void Interact()
        {
            OnLootInteract.Invoke(this);
        }

        protected override void Enter()
        {
            ghostObject.SetActive(true);
        }

        protected override void Exit()
        {
            ghostObject.SetActive(false);
        }

        private void OnDestroy()
        {
            OnExit.Invoke(this);
        }
    }
}