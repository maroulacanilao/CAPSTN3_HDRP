using Managers;
using ScriptableObjectData.CharacterData;
using UnityEngine;

namespace BattleSystem
{
    public class BattleStation : MonoBehaviour
    {
        [SerializeField] private Transform attackerTargetPos, projectileTargetPos, evadeTargetPos;

        public Vector3 attackPosition => attackerTargetPos.position;
        public Vector3 projectilePosition => projectileTargetPos.position;
        public Vector3 stationPosition => transform.position;
        
        public Vector3 evadePosition => evadeTargetPos.position;
        
        public BattleCharacter battleCharacter { get; private set; }

        public BattleCharacter Initialize(CharacterData characterData_, int level_)
        {
            // Instantiate
            battleCharacter = Instantiate(characterData_.battlePrefab).Initialize(this, level_);
                //characterData_.SpawnBattleCharacter(level_);
            
            Transform _transform = battleCharacter.transform;
            //
            // _transform.SetParent(transform);
            // _transform.localPosition = Vector3.zero;

            return battleCharacter;
        }
    }
}
