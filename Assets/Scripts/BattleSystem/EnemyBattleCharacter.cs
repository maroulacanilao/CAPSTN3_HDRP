﻿using Character;
using ScriptableObjectData.CharacterData;

namespace BattleSystem
{
    public class EnemyBattleCharacter : BattleCharacter
    {
        public EnemyData EnemyData => characterData as EnemyData;
        public override BattleCharacter Initialize(CharacterData characterData_, int level_)
        {
            // characterData = characterData_;
            Level = level_;
            return this;
        }
    }
}