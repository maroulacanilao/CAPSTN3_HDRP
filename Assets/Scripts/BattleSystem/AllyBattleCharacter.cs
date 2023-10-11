using System.Collections;
using CustomHelpers;
using NaughtyAttributes;
using ScriptableObjectData.CharacterData;
using UnityEngine;

namespace BattleSystem
{
    public class AllyBattleCharacter : BattleCharacter
    {
        public AllyData AllyData => characterData as AllyData;
        
        public override BattleCharacter Initialize(BattleStation battleStation_, int level_, float xOffset_)
        {
            base.Initialize(battleStation_, level_, xOffset_);
            Level = AllyData.LevelData.CurrentLevel;
            return this;
        }
    }
}
