using Character;
using ScriptableObjectData.CharacterData;
using UnityEngine;

namespace BattleSystem
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Persistent/BattleData", fileName = "BattleData")]
    public class BattleData : ScriptableObject
    {
        [field: SerializeField] public PlayerData playerData { get; private set; }
        public PlayerCharacter playerCharacter { get; private set; }
        public CharacterBase enemyCharacter { get; private set; }
        public int playerHp { get; private set; }
        public int playerMana { get; private set; }

        public void ResetData()
        {
            playerCharacter = null;
            enemyCharacter = null;
        }

        public void EnterBattle(PlayerCharacter player_, CharacterBase enemy_)
        {
            if (player_ == null)
            {
                Debug.LogWarning("Player is null");
                return;
            }
            if (enemy_ == null)
            {
                Debug.LogWarning("Enemy is null");
            }

            playerCharacter = player_;
            enemyCharacter = enemy_;
            playerHp = player_.healthComponent.CurrentHp;
            playerMana = player_.manaComponent.CurrentMana;
        }
    }
}