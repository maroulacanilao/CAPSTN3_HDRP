using Character;
using ScriptableObjectData.CharacterData;

namespace BattleSystem
{
    public class EnemyBattleCharacter : BattleCharacter
    {
        public EnemyData EnemyData => characterData as EnemyData;
    }
}