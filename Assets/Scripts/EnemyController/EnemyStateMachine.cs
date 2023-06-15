using EnemyController.EnemyStates;
using Farming;
using UnityEngine;

namespace EnemyController
{
    [System.Serializable]
    public class EnemyStateMachine
    {
        public bool isTileOnRight { get; set; }
        
        public Vector3 targetDestination { get; set; }
        
        private EnemyAIController aiController;
        
        [SerializeReference] private EnemyControllerState currentState;
        
        public EnemyStateMachine(EnemyAIController aiController_)
        {
            aiController = aiController_;
        }
        
        public void Initialize()
        {
            ChangeState(new EnemyPatrolState(aiController, this));
        }

        public void AnimationUpdate()
        {
            currentState?.AnimationUpdate();
        }

        public void ChangeState(EnemyControllerState newState_)
        {
            currentState?.Exit();
            Debug.Log(newState_.stateName);
            currentState = newState_;
            currentState?.Enter();
        }
    }
}
