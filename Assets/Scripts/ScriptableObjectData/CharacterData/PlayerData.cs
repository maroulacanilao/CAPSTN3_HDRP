using System.Collections.Generic;
using BaseCore;
using Character;
using Items.Inventory;
using Spells.Base;
using UnityEngine;

namespace ScriptableObjectData.CharacterData
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/CharacterData/PlayerData", order = 0)]
    public class PlayerData : CharacterData
    {
        [field: SerializeField] public PlayerInventory playerInventory { get; private set; }
        [field: SerializeField] public PlayerLevel playerLevelData { get; private set; }

        [field: SerializeField] public AllyData allyData { get; private set; }
        [field: SerializeField] public BlessingMeter blessingMeter { get; private set; }
        
        public int CurrentHp { get; set; }
        public int CurrentMana { get; set; }
        
        public void SetAlly(AllyData allyData_)
        {
            allyData = allyData_;
        }

        public void Initialize(GameDataBase gameDataBase)
        {
            playerInventory.Initialize();
            playerLevelData.ResetExperience();
            statsData.ClearAdditionalStats();
        }
        
        public void DeInitialize()
        {
            playerInventory.DeInitializeInventory();
            statsData.ClearAdditionalStats();
        }
        
        public CombatStats GetStats()
        {
            return statsData.GetTotalStats(playerLevelData.CurrentLevel);
        }


#if UNITY_EDITOR

        [NaughtyAttributes.Button("Reset HP & Mana")]
        [ContextMenu("Reset HP & Mana")]
        private void ResetHpAndMana()
        {
            CurrentHp = 0;
            CurrentMana = 0;
        }
#endif
    }
}