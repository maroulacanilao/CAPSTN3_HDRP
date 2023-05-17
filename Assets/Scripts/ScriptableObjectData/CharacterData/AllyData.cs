using ObjectPool;
using UnityEngine;

namespace ScriptableObjectData.CharacterData
{
    [CreateAssetMenu(fileName = "AllyData", menuName = "ScriptableObjects/CharacterData/AllyData", order = 0)]
    public class AllyData : CharacterData
    {
        // public BattleCharacterController SpawnBattleAlly(int level_)
        // {
        //     var _enemy = battlePrefab.GetInstance();
        //     //return _enemy.GetComponent<BattleCharacterController>().Initialize(this, level_) as EnemyBattleCharacter;
        // }
    }
}