using Items.Inventory;
using ScriptableObjectData.CharacterData;

namespace Character.CharacterComponents
{
    [System.Serializable]
    public class PlayerMana : CharacterMana
    {
        private PlayerData playerData;
        public override int MaxMana => playerData.statsData.GetTotalStats(playerData.LevelData.CurrentLevel).maxMana;

        public PlayerMana(PlayerData data_) : base(null)
        {
            playerData = data_;
            InventoryEvents.OnUpdateInventory.AddListener(OnUpdateInventory);
            
            CurrentMana = MaxMana;

            OnManuallyUpdateMana.Invoke(this);
        }
        
        ~PlayerMana()
        {
            InventoryEvents.OnUpdateInventory.RemoveListener(OnUpdateInventory);
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
    }
}
