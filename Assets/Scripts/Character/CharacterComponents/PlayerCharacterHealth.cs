using System;
using BaseCore;
using Items.Inventory;
using ScriptableObjectData.CharacterData;
using UnityEngine;

namespace Character
{
    public class PlayerCharacterHealth : CharacterHealth
    {
        private PlayerData playerData;
        
        protected override void Initialize()
        {
            base.Initialize();
            playerData = characterData as PlayerData;
            InventoryEvents.OnUpdateInventory.AddListener(OnUpdateInventory);
            if(playerData.CurrentHp > 0) return;
            playerData.CurrentHp = CurrentHp;
            OnManuallyUpdateHealth.Invoke(this);
        }

        protected void OnDestroy()
        {
            InventoryEvents.OnUpdateInventory.RemoveListener(OnUpdateInventory);
        }

        protected override void SetCurrentHp(int newCurrentHP_)
        {
            base.SetCurrentHp(newCurrentHP_);
            playerData.CurrentHp = CurrentHp;
        }

        protected void OnEnable()
        {
            var _hp = playerData != null ? playerData.CurrentHp : 0;
            if(_hp <= 0) return;
            CurrentHp = playerData.CurrentHp;
            OnManuallyUpdateHealth.Invoke(this);
        }
        
        private void OnUpdateInventory(PlayerInventory inventory)
        {
            SetCurrentHp(CurrentHp);
            OnManuallyUpdateHealth.Invoke(this);
        }
    }
}