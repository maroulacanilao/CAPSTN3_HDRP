using System;
using System.Collections;
using CustomHelpers;
using UnityEngine;

namespace EnemyController.EnemyStates
{
    [System.Serializable]
    public abstract class EnemyControllerState
    {
        public readonly EnemyAIController controller;
        public readonly EnemyStateMachine stateMachine;
        public bool isStateActive; 

        public EnemyControllerState(EnemyAIController aiController_, EnemyStateMachine stateMachine_)
        {
            controller = aiController_;
            stateMachine = stateMachine_;
        }

        public virtual void Enter()
        {
            isStateActive = true;
        }
        
        public virtual void Exit()
        {
            isStateActive = false;
            controller.animator.ResetTrigger(controller.AttackHash);
            controller.animator.ResetTrigger(controller.GroundedHash);
        }
        
        protected void DefaultState()
        {
            stateMachine.targetTile = null;
            stateMachine.tileCol = null;

            stateMachine.targetDestination = default;
            stateMachine.ChangeState(stateMachine.patrolState);
        }

        protected bool IsWithinAttackRange(Collider targetCol_)
        {
            return !targetCol_.IsEmptyOrDestroyed() && controller.attackRangeCollider.bounds.Intersects(targetCol_.bounds);
        }
        
        protected IEnumerator Co_Attack(Action attackAction_)
        {
            Debug.Log("ATTACK Coroutine");
            controller.animator.SetTrigger(controller.AttackHash);
            yield return controller.animator.WaitForAnimationEvent(controller.AttackHitEvent, 1f);
            attackAction_?.Invoke();

            yield return new WaitForSeconds(controller.attackCooldown);
            controller.animator.ResetTrigger(controller.AttackHash);
        }
    }
}
