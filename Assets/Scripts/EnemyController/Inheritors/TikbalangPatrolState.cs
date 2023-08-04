using CustomHelpers;
using EnemyController.EnemyStates;

namespace EnemyController.Inheritors
{
    public class TikbalangPatrolState : EnemyPatrolState
    {
        protected TikbalangController tikbalangController;
        
        public TikbalangPatrolState(EnemyAIController aiController_, EnemyStateMachine stateMachine_) : base(aiController_, stateMachine_)
        {
            tikbalangController = aiController_.GetComponent<TikbalangController>();
        }
        
        public override void Enter()
        {
            tikbalangController.ChangeDisguise(tikbalangController.disguises.GetRandomItem());
            base.Enter();
            controller.animator.SetTrigger(controller.GroundedHash);
        }

        public override void Exit()
        {
            tikbalangController.ChangeDisguise(tikbalangController.defaultDisguise);
            base.Exit();
        }
    }
}