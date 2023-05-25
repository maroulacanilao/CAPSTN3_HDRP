using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Farming;
using Managers;
using Pathfinding;
using UnityEngine;

namespace EnemyController.EnemyStates
{
    [System.Serializable]
    public class EnemyPatrolState : EnemyControllerState
    {
        private Transform patrolWaypoint;


        public EnemyPatrolState(EnemyAIController aiController_, EnemyStateMachine stateMachine_) : base(aiController_, stateMachine_)
        {
        }

        public override void Enter()
        {
            base.Enter();
            controller.StopAllCoroutines();
            controller.aiPath.maxSpeed = controller.movementSpeed;
            controller.aiPath.whenCloseToDestination = CloseToDestinationMode.Stop;
            controller.animator.SetTrigger(controller.GroundedHash);
            
            patrolWaypoint = patrolWaypoint == null ? 
                PatrolWaypointManager.Instance.GetRandomTransform() : 
                PatrolWaypointManager.Instance.GetRandomTransform(patrolWaypoint);

            controller.aiPath.destination = patrolWaypoint.position;

            controller.StartCoroutine(OnReachDestination());
            controller.StartCoroutine(Co_CheckForTiles());
        }

        public override void Exit()
        {
            base.Exit();
            controller.StopAllCoroutines();
            controller.animator.ResetTrigger(controller.GroundedHash);
        }

        IEnumerator OnReachDestination()
        {
            yield return controller.WaitUntilReachDestination();
            if (FarmTileManager.Instance.HasNonEmptyTile())
            {
                stateMachine.ChangeState(stateMachine.chaseTileState);
            }
            else stateMachine.ChangeState(stateMachine.patrolState);
        }

        IEnumerator Co_CheckForTiles()
        {
            var _waiter = new WaitForSeconds(1.25f);
            while (controller.gameObject.activeSelf)
            {
                if (FarmTileManager.Instance.HasNonEmptyTile())
                {
                    stateMachine.ChangeState(stateMachine.chaseTileState);
                    yield break;
                }
                yield return _waiter;
            }
        }
    }
}