using Items.Inventory;
using ScriptableObjectData.CharacterData;
using UnityEngine;

namespace Character.CharacterComponents
{
    [System.Serializable]
    public class PlayerHealth : CharacterHealth
    {
        private PlayerData playerData;
        public override int MaxHp => playerData.statsData.GetTotalStats(playerData.LevelData.CurrentLevel).maxHp;
        
        public PlayerHealth(PlayerData data_) : base(null)
        {
            playerData = data_;
            InventoryEvents.OnUpdateInventory.AddListener(OnUpdateInventory);
            CurrentHp = MaxHp;
            OnManuallyUpdateHealth.Invoke(this);
        }
        
        ~PlayerHealth()
        {
            InventoryEvents.OnUpdateInventory.RemoveListener(OnUpdateInventory);
        }

        public override void OnCharacterEnable()
        {
            OnManuallyUpdateHealth.Invoke(this);
        }

        private void OnUpdateInventory(PlayerInventory inventory)
        {
            SetCurrentHp(CurrentHp);
            OnManuallyUpdateHealth.Invoke(this);
        }
    }
}