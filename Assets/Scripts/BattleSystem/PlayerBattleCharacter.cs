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

        public override BattleCharacter Initialize(CharacterData characterData_, int level_)
        {
            // characterData = characterData;
            Level = PlayerData.playerLevelData.CurrentLevel;
            return this;
        }
    }
}
