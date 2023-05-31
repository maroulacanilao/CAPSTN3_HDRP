using System.Collections;
using CustomHelpers;
using NaughtyAttributes;
using ScriptableObjectData.CharacterData;
using UnityEngine;

namespace BattleSystem
{
    public class PlayerBattleCharacter : BattleCharacter
    {
        public PlayerData PlayerData => characterData as PlayerData;

        public override BattleCharacter Initialize(BattleStation battleStation_, int level_)
        {
            base.Initialize(battleStation_, level_);
            Level = PlayerData.playerLevelData.CurrentLevel;
            return this;
        }
    }
}
