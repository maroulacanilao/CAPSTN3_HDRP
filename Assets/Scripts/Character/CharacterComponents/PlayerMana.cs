using BaseCore;
using Items.Inventory;
using ScriptableObjectData.CharacterData;

namespace Character.CharacterComponents
{
    [System.Serializable]
    public class PlayerMana : CharacterMana
    {
        private PlayerData playerData;
        public override int MaxMana => playerData.statsData.GetTotalStats(playerData.LevelData.CurrentLevel).mana;

        public PlayerMana(PlayerData data_) : base(null)
        {
            playerData = data_;
            InventoryEvents.OnUpdateInventory.AddListener(OnUpdateInventory);
            PlayerLevel.OnLevelUp.AddListener(RefreshMana);
            
            CurrentMana = MaxMana;

            OnManuallyUpdateMana.Invoke(this);
        }
        
        ~PlayerMana()
        {
            InventoryEvents.OnUpdateInventory.RemoveListener(OnUpdateInventory);
            PlayerLevel.OnLevelUp.RemoveListener(RefreshMana);
        }

        public override void OnCharacterEnable()
        {
            OnManuallyUpdateMana.Invoke(this);
        }

        private void OnUpdateInventory(PlayerInventory inventory)
        {
            SetCurrentMana(CurrentMana);
            OnManuallyUpdateMana.Invoke(this);
        }

        public override void RefreshMana()
        {
            base.RefreshMana();
            OnManuallyUpdateMana.Invoke(this);
        }
    }
}
