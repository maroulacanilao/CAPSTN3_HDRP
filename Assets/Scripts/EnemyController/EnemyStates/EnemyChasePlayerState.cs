using System.Collections;
using Pathfinding;
using UnityEngine;

namespace EnemyController.EnemyStates
{
    [System.Serializable]
    public class EnemyChasePlayerState : EnemyControllerState
    {
        public EnemyChasePlayerState(EnemyAIController aiController_, EnemyStateMachine stateMachine_) : base(aiController_, stateMachine_)
        {
        }
        
        public override void Enter()
        {
            base.Enter();
            controller.aiPath.maxSpeed = controller.chaseSpeed;
            controller.aiPath.whenCloseToDestination = CloseToDestinationMode.ContinueToExactDestination;
            controller.StartCoroutine(controller.RefreshDestination(stateMachine.player, 0.5f));
            controller.animator.SetTrigger(controller.GroundedHash);
        }
        
        public override void Exit()
        {
            base.Exit();
            controller.StopAllCoroutines();
            controller.animator.ResetTrigger(controller.GroundedHash);
        }
    }
}