﻿using System.Collections.Generic;
using BaseCore;
using Character;
using Character.CharacterComponents;
using Items.Inventory;
using NaughtyAttributes;
using Spells.Base;
using UnityEngine;
namespace ScriptableObjectData.CharacterData
{
    [CreateAssetMenu(fileName = "AllyData", menuName = "ScriptableObjects/CharacterData/AllyData", order = 0)]
    public class AllyData : CharacterData
    {
        [field: SerializeField] public PlayerLevel LevelData { get; private set; }
        [field: SerializeField] public AllyHealth health { get; private set; }
        [field: SerializeField] public AllyMana mana { get; private set; }
        
        public int level => LevelData.CurrentLevel;
        public CombatStats totalStats => statsData.GetTotalStats(level);
        public CombatStats baseStats => statsData.GetTotalNonBonusStats(level);
        
        public void Initialize(GameDataBase gameDataBase)
        {
            statsData.ClearAdditionalStats();
            health = new AllyHealth(this);
            mana = new AllyMana(this);
        }
        
        public CombatStats GetStats()
        {
            return statsData.GetTotalStats(LevelData.CurrentLevel);
        }
        // public BattleCharacterController SpawnBattleAlly(int level_)
        // {
        //     var _enemy = battlePrefab.GetInstance();
        //     //return _enemy.GetComponent<BattleCharacterController>().Initialize(this, level_) as EnemyBattleCharacter;
        // }
    }
}