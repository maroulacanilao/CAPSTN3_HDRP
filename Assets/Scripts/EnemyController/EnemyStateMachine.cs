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

        protected EnemyAIController aiController;

        #region States
        
        public EnemyPatrolState patrolState { get; protected set; }
        public EnemyChasePlayer chasePlayerState { get; protected set; }
        public EnemyAttackState attackState { get; protected set; }
        public EnemyGoToStationState goToStationState { get; protected set; }
        public EnemyHitState hitState { get; protected set; }

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
        
        protected PlayerInputController mPlayerController;

        [SerializeReference] protected EnemyControllerState currentState;
        
        public EnemyControllerState CurrentState => currentState;
        
        public EnemyStateMachine(EnemyAIController aiController_)
        {
            aiController = aiController_;
        }
        protected EnemyStateMachine()
        {
        }

        public virtual void Initialize()
        {
            patrolState = new EnemyPatrolState(aiController, this);
            chasePlayerState = new EnemyChasePlayer(aiController, this);
            attackState = new EnemyAttackState(aiController, this);
            goToStationState = new EnemyGoToStationState(aiController, this);
            hitState = new EnemyHitState(aiController, this);
            
            ChangeState(patrolState);
        }

        public virtual void Enable()
        {
            if (aiController.station.IsEmptyOrDestroyed())
            {
                UnityEngine.Object.Destroy(aiController.gameObject);
                return;
            }
            
            currentState?.Enable();
        }

        public virtual void FixedUpdate()
        {
            currentState?.AnimationUpdate();
            currentState?.FixedUpdate();
        }

        public virtual void ChangeState(EnemyControllerState newState_)
        {
            currentState?.Exit();
            currentState = newState_;
            currentState?.Enter();
        }
    }
}
