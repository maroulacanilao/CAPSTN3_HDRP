using System.Collections.Generic;
using BaseCore;
using Character;
using Character.CharacterComponents;
using Items.Inventory;
using NaughtyAttributes;
using Spells.Base;
using UnityEngine;

namespace ScriptableObjectData.CharacterData
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/CharacterData/PlayerData", order = 0)]
    public class PlayerData : CharacterData
    {
        [field: SerializeField] public PlayerInventory inventory { get; private set; }
        [field: SerializeField] public PlayerLevel LevelData { get; private set; }
        [field: SerializeField] public List<AllyData> alliesData { get; private set; }
        [field: SerializeField] public List<AllyData> offPartyData { get; private set; }
        
        [field: SerializeField] public BlessingMeter blessingMeter { get; private set; }
        [field: SerializeField] public PlayerHealth health { get; private set; }
        [field: SerializeField] public PlayerMana mana { get; private set; }
        public PlayerStatusEffectReceiver statusEffectReceiver { get; private set; }

        public int level => LevelData.CurrentLevel;
        public CombatStats totalStats => statsData.GetTotalStats(level);
        public CombatStats baseStats => statsData.GetTotalNonBonusStats(level);
        
        public void AddAlly(AllyData allyData_)
        {
            alliesData.Add(allyData_);
        }

        public void RemoveAlly(AllyData allydata_)
        {
            foreach (var a in alliesData)
            {
                if (a == allydata_)
                {
                    alliesData.Remove(a);
                }
            }
        }

        public void Initialize(GameDataBase gameDataBase)
        {
            inventory.Initialize();
            statsData.ClearAdditionalStats();
            health = new PlayerHealth(this);
            mana = new PlayerMana(this);
            statusEffectReceiver = new PlayerStatusEffectReceiver(this);
        }
        
        public void DeInitialize()
        {
            inventory.DeInitializeInventory();
            statsData.ClearAdditionalStats();
        }
        
        public CombatStats GetStats()
        {
            return statsData.GetTotalStats(LevelData.CurrentLevel);
        }


        [ContextMenu("Reset Data")]
        public void ResetData()
        {
            health.RefillHealth();
            mana.RefreshMana();
            LevelData.ResetExperience();
            spells.Clear();
        }
    }
}