using Character;
using ScriptableObjectData.CharacterData;
using UnityEngine;

namespace BattleSystem
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Persistent/BattleData", fileName = "BattleData")]
    public class BattleData : ScriptableObject
    {
        [field: SerializeField] public PlayerData playerData { get; private set; }

        public EnemyData currentEnemyData { get; private set; }
        public Vector3 currentEnemyPosition { get; private set; }

        public void ResetData()
        {
            currentEnemyData = null;
        }

        public void EnterBattle(EnemyCharacter enemy_)
        {
            if (enemy_ == null)
            {
                Debug.LogWarning("Enemy is null");
            }
            currentEnemyData = enemy_.characterData as EnemyData;
            currentEnemyPosition = enemy_.transform.position;
        }
    }
}