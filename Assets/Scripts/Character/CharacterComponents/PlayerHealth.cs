using BaseCore;
using Items.Inventory;
using ScriptableObjectData.CharacterData;
using UnityEngine;

namespace Character.CharacterComponents
{
    [System.Serializable]
    public class PlayerHealth : CharacterHealth
    {
        private PlayerData playerData;
        public override int MaxHp => playerData.statsData.GetTotalStats(playerData.LevelData.CurrentLevel).vitality;
        
        private float cachedHpPercentage;
        
        public PlayerHealth(PlayerData data_) : base(null)
        {
            playerData = data_;
            CurrentHp = MaxHp;
            OnManuallyUpdateHealth.Invoke(this);
            data_.statsData.OnBeforeChangeStats.AddListener(BeforeChangeStats);
            data_.statsData.OnAfterChangeStats.AddListener(AfterChangeStats);
            PlayerLevel.OnLevelUp.AddListener(RefillHealth);
        }
        
        ~PlayerHealth()
        {
            playerData.statsData.OnBeforeChangeStats.RemoveListener(BeforeChangeStats);
            playerData.statsData.OnAfterChangeStats.RemoveListener(AfterChangeStats);
            PlayerLevel.OnLevelUp.RemoveListener(RefillHealth);
        }

        public override void OnCharacterEnable()
        {
            OnManuallyUpdateHealth.Invoke(this);
        }
        
        void BeforeChangeStats()
        {
            cachedHpPercentage = (float) CurrentHp / MaxHp;
            cachedHpPercentage = Mathf.Clamp01(cachedHpPercentage);
        }
        
        void AfterChangeStats()
        {
            var _hp = Mathf.RoundToInt(MaxHp * cachedHpPercentage);
            SetCurrentHp(_hp);
            OnManuallyUpdateHealth.Invoke(this);
        }

        public override void RefillHealth()
        {
            base.RefillHealth();
            OnManuallyUpdateHealth.Invoke(this);
        }
    }
}