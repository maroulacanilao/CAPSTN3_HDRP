using CustomHelpers;
using EnemyController.EnemyStates;
using Farming;
using Player;
using UnityEngine;

namespace EnemyController
{
    [System.Serializable]
    public class EnemyStateMachine
    {

        private EnemyAIController aiController;

        #region States
        
        public EnemyPatrolState patrolState { get; private set; }
        public EnemyChasePlayer chasePlayerState { get; private set; }
        public EnemyAttackState attackState { get; private set; }
        public EnemyGoToStationState goToStationState { get; private set; }

        #endregion

        public Transform playerTransform => playerController.transform;

        public PlayerInputController playerController
        {
            get
            {
                if(mPlayerController.IsValid()) return mPlayerController;
                mPlayerController = PlayerInputController.Instance;
                return mPlayerController;
            }
        }
        
        private PlayerInputController mPlayerController;

        [SerializeReference] private EnemyControllerState currentState;
        
        public EnemyStateMachine(EnemyAIController aiController_)
        {
            aiController = aiController_;

        }
        
        public void Initialize()
        {
            patrolState = new EnemyPatrolState(aiController, this);
            chasePlayerState = new EnemyChasePlayer(aiController, this);
            attackState = new EnemyAttackState(aiController, this);
            goToStationState = new EnemyGoToStationState(aiController, this);
            
            ChangeState(patrolState);
        }

        public void Enable()
        {
            if (aiController.station.IsEmptyOrDestroyed())
            {
                UnityEngine.Object.Destroy(aiController.gameObject);
                return;
            }
            
            currentState?.Enable();
        }

        public void FixedUpdate()
        {
            currentState?.AnimationUpdate();
            currentState?.FixedUpdate();
        }

        public void ChangeState(EnemyControllerState newState_)
        {
            currentState?.Exit();
            currentState = newState_;
            currentState?.Enter();
        }
    }
}
