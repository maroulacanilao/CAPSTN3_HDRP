using Items.Inventory;
using ScriptableObjectData.CharacterData;

namespace Character.CharacterComponents
{
    public class PlayerCharacterMana : CharacterMana
    {
        private PlayerData playerData;
        
        protected override void Initialize()
        {
            base.Initialize();
            playerData = characterData as PlayerData;
            InventoryEvents.OnUpdateInventory.AddListener(OnUpdateInventory);
            if(playerData.CurrentMana > 0) return;
            playerData.CurrentMana = CurrentMana;
            OnManuallyUpdateMana.Invoke(this);
        }
        
        protected void OnDestroy()
        {
            InventoryEvents.OnUpdateInventory.RemoveListener(OnUpdateInventory);
        }

        protected override void SetCurrentMana(int newCurrMana_)
        {
            base.SetCurrentMana(newCurrMana_);
            playerData.CurrentMana = CurrentMana;
        }
        
        protected void OnEnable()
        {
            var _hp = playerData != null ? playerData.CurrentHp : 0;
            if(_hp <= 0) return;
            CurrentMana = playerData.CurrentMana;
            OnManuallyUpdateMana.Invoke(this);
        }
        
        private void OnUpdateInventory(PlayerInventory inventory)
        {
            SetCurrentMana(CurrentMana);
            OnManuallyUpdateMana.Invoke(this);
        }
    }
}
