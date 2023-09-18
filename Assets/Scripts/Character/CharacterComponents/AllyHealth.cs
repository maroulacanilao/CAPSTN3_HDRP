using BaseCore;
using Items.Inventory;
using ScriptableObjectData.CharacterData;
using UnityEngine;

namespace Character.CharacterComponents
{
    [System.Serializable]
    public class AllyHealth : CharacterHealth
    {
        private AllyData allyData;
        public override int MaxHp => allyData.statsData.GetTotalStats(allyData.LevelData.CurrentLevel).vitality;
        
        private float cachedHpPercentage;
        
        public AllyHealth (AllyData data_) : base(null)
        {
            allyData = data_;
            CurrentHp = MaxHp;
            OnManuallyUpdateHealth.Invoke(this);
            data_.statsData.OnBeforeChangeStats.AddListener(BeforeChangeStats);
            data_.statsData.OnAfterChangeStats.AddListener(AfterChangeStats);
            PlayerLevel.OnLevelUp.AddListener(RefillHealth);
        }
        
        ~AllyHealth()
        {
            allyData.statsData.OnBeforeChangeStats.RemoveListener(BeforeChangeStats);
            allyData.statsData.OnAfterChangeStats.RemoveListener(AfterChangeStats);
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