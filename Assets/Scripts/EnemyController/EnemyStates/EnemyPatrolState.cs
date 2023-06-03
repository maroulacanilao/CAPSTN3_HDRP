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
            stateName = "Patrol";
        }
    }
}