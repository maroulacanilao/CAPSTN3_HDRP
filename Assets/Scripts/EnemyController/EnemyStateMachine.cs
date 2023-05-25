using EnemyController.EnemyStates;
using Farming;
using UnityEngine;

namespace EnemyController
{
    [System.Serializable]
    public class EnemyStateMachine
    {
        public Transform player { get; private set; }
        public bool isTileOnRight { get; set; }
        
        public FarmTile targetTile { get; set; }
        public Collider tileCol { get; set; }
        public Vector3 targetDestination { get; set; }

        #region States

        public EnemyChasePlayerState chasePlayerState { get; private set; }
        public EnemyChaseTileState chaseTileState { get; private set; }
        
        public EnemyPatrolState patrolState { get; private set; }
        
        public EnemyAttackTileState attackTileState { get; private set; }

        #endregion
        
        [SerializeReference] private EnemyControllerState currentState;
        
        public EnemyStateMachine(EnemyAIController aiController_)
        {
            patrolState = new EnemyPatrolState(aiController_, this);
            chasePlayerState = new EnemyChasePlayerState(aiController_, this);
            chaseTileState = new EnemyChaseTileState(aiController_, this);
            attackTileState = new EnemyAttackTileState(aiController_, this);
        }
        
        public void Initialize()
        {
            player = FarmSceneManager.Instance.player.transform;
            ChangeState(patrolState);
        }

        public void ChangeState(EnemyControllerState newState_)
        {
            currentState?.Exit();
            currentState = newState_;
            currentState?.Enter();
        }
    }
}
