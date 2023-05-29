using Items.Inventory;
using ScriptableObjectData.CharacterData;

namespace Character.CharacterComponents
{
    [System.Serializable]
    public class PlayerCharacterMana : CharacterMana
    {
        private PlayerData playerData;
        
        public PlayerCharacterMana(CharacterBase character_) : base(character_)
        {
            playerData = characterData as PlayerData;
            InventoryEvents.OnUpdateInventory.AddListener(OnUpdateInventory);
            if(playerData.CurrentMana > 0) return;
            playerData.CurrentMana = CurrentMana;
            OnManuallyUpdateMana.Invoke(this);
        }
        
        ~PlayerCharacterMana()
        {
            InventoryEvents.OnUpdateInventory.RemoveListener(OnUpdateInventory);
        }

        public override void OnCharacterEnable()
        {
            var _hp = playerData != null ? playerData.CurrentHp : 0;
            if(_hp <= 0) return;
            CurrentMana = playerData.CurrentMana;
            OnManuallyUpdateMana.Invoke(this);
        }

        protected override void SetCurrentMana(int newCurrMana_)
        {
            base.SetCurrentMana(newCurrMana_);
            playerData.CurrentMana = CurrentMana;
        }

        private void OnUpdateInventory(PlayerInventory inventory)
        {
            SetCurrentMana(CurrentMana);
            OnManuallyUpdateMana.Invoke(this);
        }
    }
}
