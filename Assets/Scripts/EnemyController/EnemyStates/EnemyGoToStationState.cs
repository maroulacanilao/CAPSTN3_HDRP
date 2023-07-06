namespace EnemyController.EnemyStates
{
    public class EnemyGoToStationState : EnemyControllerState
    {

        public EnemyGoToStationState(EnemyAIController aiController_, EnemyStateMachine stateMachine_) : base(aiController_, stateMachine_)
        {
            stateName = "GoToStation";
        }
        
        public override void Enter()
        {
            base.Enter();
            isStateActive = true;
            controller.aiPath.canMove = true;
            controller.aiPath.isStopped = false;
            controller.aiPath.maxSpeed = controller.chaseSpeed;
            controller.aiPath.destination = controller.station.transform.position;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            
            if(controller.aiPath.reachedDestination) GoToDefaultState();
        }

        public override void Exit()
        {
            base.Exit();
            isStateActive = false;
        }
    }
}