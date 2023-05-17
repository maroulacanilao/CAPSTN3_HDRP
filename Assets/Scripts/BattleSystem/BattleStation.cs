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

        public BattleCharacter Initialize(BattleCharacter battleCharacter_)
        {
            // Instantiate
            // set parent here
            return null;
        }
    }
}
