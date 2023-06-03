using System.Collections;
using Managers;
using Pathfinding;
using UnityEngine;

namespace EnemyController.EnemyStates
{
    [System.Serializable]
    public class EnemyChasePlayerState : EnemyControllerState
    {
        private readonly Transform playerTransform; 
        public EnemyChasePlayerState(EnemyAIController aiController_, EnemyStateMachine stateMachine_) : base(aiController_, stateMachine_)
        {
            stateName = "Chase Player";
            playerTransform = GameManager.Instance.playerOnFarm.transform;
        }
        
        public override void Enter()
        {
            base.Enter();
            controller.aiPath.maxSpeed = controller.chaseSpeed;
            controller.aiPath.whenCloseToDestination = CloseToDestinationMode.ContinueToExactDestination;
            controller.StartCoroutine(controller.RefreshDestination(playerTransform, 0.5f));
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