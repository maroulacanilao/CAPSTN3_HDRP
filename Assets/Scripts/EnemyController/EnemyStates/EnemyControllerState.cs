using System;
using System.Collections;
using CustomHelpers;
using Farming;
using Managers;
using UnityEngine;

namespace EnemyController.EnemyStates
{
    [System.Serializable]
    public abstract class EnemyControllerState
    {
        public readonly EnemyAIController controller;
        public readonly EnemyStateMachine stateMachine;
        
        public bool isStateActive;
        public string stateName;
        
        protected Transform player => stateMachine.playerTransform;
        protected Transform station => controller.station.transform;
        protected Rigidbody playerRb => stateMachine.playerController.rb;

        public EnemyControllerState(EnemyAIController aiController_, EnemyStateMachine stateMachine_)
        {
            controller = aiController_;
            stateMachine = stateMachine_;
        }

        public virtual void Enter()
        {
            isStateActive = true;
        }
        
        public virtual void AnimationUpdate()
        {
            var _vel = controller.aiPath.velocity;
            bool _isIdle = _vel.magnitude < 0.1f;
            controller.animator.SetBool(controller.IsIdleHash, _isIdle);
            controller.animator.SetFloat(controller.xSpeedHash, controller.aiPath.velocity.x);
        }
        
        public virtual void FixedUpdate()
        {
            
        }

        public virtual void Enable()
        {
            
        }

        public virtual void Exit()
        {
            isStateActive = false;
            controller.animator.ResetTrigger(controller.AttackHash);
            controller.animator.ResetTrigger(controller.GroundedHash);
        }
        
        protected void DefaultState()
        {
            
        }

        protected bool IsWithinAttackRange(Collider targetCol_)
        {
            return !targetCol_.IsEmptyOrDestroyed() && controller.attackRangeCollider.bounds.Intersects(targetCol_.bounds);
        }
        
        protected bool IsWithinAttackRange(Transform target_)
        {
            return !target_.IsEmptyOrDestroyed() && Vector3.Distance(controller.transform.position, target_.position) <= controller.attackRange;
        }

        protected void GoToDefaultState()
        {
            stateMachine.ChangeState(stateMachine.patrolState);
        }
        
        protected bool IsWithinAlertRange(Transform target_)
        {
            return !target_.IsEmptyOrDestroyed() && controller.alertRange.IsPlayerNearby();
        }

        protected void FleeFromPlayer()
        {
            var _controllerPos = controller.transform.position;
            
            Vector3 _fleeDir = (player.position + playerRb.velocity) - _controllerPos;
            controller.aiPath.destination = _controllerPos - _fleeDir;
        }
        
        protected void ChasePlayer()
        {
            // controller.aiPath.destination = player.position + playerRb.velocity;
            controller.aiPath.destination = player.position;
        }
        
        public void StopMovement()
        {
            if(controller.IsEmptyOrDestroyed()) return;
            controller.aiPath.canMove = false;
            controller.aiPath.isStopped = true;
            controller.aiPath.maxSpeed = 0;
            controller.aiPath.enabled = false;
            controller.movementController.enabled = false;
        }
        
        public void ResumeMovement()
        {
            if(controller.IsEmptyOrDestroyed()) return;
            controller.aiPath.canMove = true;
            controller.aiPath.isStopped = false;
            controller.aiPath.maxSpeed = controller.patrolSpeed;
            controller.aiPath.enabled = true;
            controller.movementController.enabled = true;
        }
    }
}
