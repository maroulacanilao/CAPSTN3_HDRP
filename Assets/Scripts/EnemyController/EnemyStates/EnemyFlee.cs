using CustomHelpers;
using UnityEngine;

namespace EnemyController.EnemyStates
{
    public class EnemyFlee : EnemyControllerState
    {
        public EnemyFlee(EnemyAIController aiController_, EnemyStateMachine stateMachine_) : base(aiController_, stateMachine_)
        {
        }
        
        public override void Enter()
        {
            base.Enter();
            isStateActive = true;
            controller.aiPath.canMove = true;
            controller.aiPath.isStopped = false;
            controller.aiPath.maxSpeed = controller.chaseSpeed;
        }
        
        public override void FixedUpdate()
        {
            if(player.IsEmptyOrDestroyed())
            {
                stateMachine.ChangeState(stateMachine.goToStationState);
                return;
            }
            
            FleeFromPlayer();

            if (IsWithinAttackRange(player))
            {
                stateMachine.ChangeState(stateMachine.attackState);
                return;
            }
            
            if(!IsWithinAlertRange(player))
            {
                stateMachine.ChangeState(stateMachine.goToStationState);
                return;
            }
            
            if(Vector3.Distance(controller.transform.position, station.position) > controller.chaseRange)
            {
                stateMachine.ChangeState(stateMachine.goToStationState);
                return;
            }
        }
        
        public override void Exit()
        {
            base.Exit();
            isStateActive = false;
            controller.StopAllCoroutines();
        }
    }
}