using BaseCore;
using Items.Inventory;
using ScriptableObjectData.CharacterData;

namespace Character.CharacterComponents
{
    [System.Serializable]
    public class AllyMana : CharacterMana
    {
        private AllyData allyData;
        public override int MaxMana => allyData.statsData.GetTotalStats(allyData.LevelData.CurrentLevel).mana;

        public AllyMana(AllyData data_) : base(null)
        {
            allyData = data_;
            PlayerLevel.OnLevelUp.AddListener(RefreshMana);
            
            CurrentMana = MaxMana;

            OnManuallyUpdateMana.Invoke(this);
        }
        
        ~AllyMana()
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