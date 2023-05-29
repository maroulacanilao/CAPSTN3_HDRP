using Items.Inventory;
using ScriptableObjectData.CharacterData;

namespace Character.CharacterComponents
{
    [System.Serializable]
    public class PlayerCharacterHealth : CharacterHealth
    {
        private PlayerData playerData;
        
        public PlayerCharacterHealth(CharacterBase character_) : base(character_)
        {
            playerData = characterData as PlayerData;
            InventoryEvents.OnUpdateInventory.AddListener(OnUpdateInventory);
            playerData.CurrentHp = CurrentHp;
            OnManuallyUpdateHealth.Invoke(this);
        }
        
        ~PlayerCharacterHealth()
        {
            InventoryEvents.OnUpdateInventory.RemoveListener(OnUpdateInventory);
        }

        public override void OnCharacterEnable()
        {
            var _hp = playerData != null ? playerData.CurrentHp : 0;
            if(_hp <= 0) return;
            CurrentHp = playerData.CurrentHp;
            OnManuallyUpdateHealth.Invoke(this);
        }
        
        protected override void SetCurrentHp(int newCurrentHP_)
        {
            base.SetCurrentHp(newCurrentHP_);
            playerData.CurrentHp = CurrentHp;
        }
        
        private void OnUpdateInventory(PlayerInventory inventory)
        {
            SetCurrentHp(CurrentHp);
            OnManuallyUpdateHealth.Invoke(this);
        }
    }
}