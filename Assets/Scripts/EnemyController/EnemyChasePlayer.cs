using System.Collections;
using CustomHelpers;
using EnemyController.EnemyStates;
using Pathfinding;
using UnityEngine;

namespace EnemyController
{
    public class EnemyChasePlayer : EnemyControllerState
    {
        public EnemyChasePlayer(EnemyAIController aiController_, EnemyStateMachine stateMachine_) : base(aiController_, stateMachine_)
        {
            stateName = "ChasePlayer";
        }

        public override void Enter()
        {
            base.Enter();
            controller.animator.SetTrigger(controller.GroundedHash);
            isStateActive = true;
            controller.aiPath.canMove = true;
            controller.aiPath.isStopped = false;
            controller.aiPath.maxSpeed = controller.chaseSpeed;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            
            if(player == null)
            {
                stateMachine.ChangeState(stateMachine.goToStationState);
                return;
            }
            
            ChasePlayer();

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
            controller.animator.ResetTrigger(controller.GroundedHash);
            controller.StopAllCoroutines();
        }
    }
}
