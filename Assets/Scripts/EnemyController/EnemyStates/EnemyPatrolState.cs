using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CustomHelpers;
using Farming;
using Managers;
using Pathfinding;
using UnityEngine;

namespace EnemyController.EnemyStates
{
    [System.Serializable]
    public class EnemyPatrolState : EnemyControllerState
    {
        protected Transform[] waypoints;
        
        protected int currentWaypointIndex;
        protected AIPath aiPath;
        
        public EnemyPatrolState(EnemyAIController aiController_, EnemyStateMachine stateMachine_) 
            : base(aiController_, stateMachine_)
        {
            stateName = "Patrol";
            waypoints = controller.station.PatrolPoints;
            aiPath = controller.aiPath;
        }

        public override void Enter()
        {
            base.Enter();
            isStateActive = true;
            aiPath.maxSpeed = controller.patrolSpeed;
            controller.aiPath.isStopped = false;
            aiPath.canMove = true;
            currentWaypointIndex = UnityEngine.Random.Range(0, waypoints.Length);
            aiPath.destination = waypoints[currentWaypointIndex].position;
            controller.StartCoroutine(Co_Patrol());
            
            controller.alertRange.OnPlayerNearby.AddListener(OnPlayerNearby);
        }

        public override void Enable()
        {
            base.Enable();
            controller.StartCoroutine(Co_Patrol());
        }

        public override void Exit()
        {
            base.Exit();
            isStateActive = false;
            controller.animator.ResetTrigger(controller.GroundedHash);
            controller.alertRange.OnPlayerNearby.RemoveListener(OnPlayerNearby);
            controller.StopAllCoroutines();
        }

        private IEnumerator Co_Patrol()
        {
            var _waiter = new WaitForSeconds(0.5f);

            while (controller.IsValid())
            {
                yield return _waiter;
                if (!controller.aiPath.reachedDestination) continue;
                
                SetWaypoint();
            }
        }

        private void SetWaypoint()
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0;
            }
            controller.aiPath.destination = waypoints[currentWaypointIndex].position;
        }
        
        private void OnPlayerNearby(Transform player_)
        {
            if(player_ != stateMachine.playerTransform) return;
            
            stateMachine.ChangeState(stateMachine.chasePlayerState);
        }
        
        public void TeleportToRandomWaypoint()
        {
            currentWaypointIndex = UnityEngine.Random.Range(0, waypoints.Length);
            controller.transform.position = waypoints[currentWaypointIndex].position;
        }
    }
}