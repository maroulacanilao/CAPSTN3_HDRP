using EnemyController.EnemyStates;

namespace EnemyController.Inheritors
{
    public class TikbalangStateMachine : EnemyStateMachine
    {
        public TikbalangStateMachine(EnemyAIController aiController_)
        {
            aiController = aiController_;
        }
        
        public override void Initialize()
        {
            patrolState = new TikbalangPatrolState(aiController, this);
            chasePlayerState = new EnemyChasePlayer(aiController, this);
            attackState = new EnemyAttackState(aiController, this);
            goToStationState = new EnemyGoToStationState(aiController, this);
            hitState = new EnemyHitState(aiController, this);
            
            ChangeState(patrolState);
        }
    }
}