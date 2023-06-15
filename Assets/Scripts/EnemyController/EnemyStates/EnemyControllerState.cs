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
        
        protected FarmTile targetTile;
        public string stateName;

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
        
        public virtual void Exit()
        {
            isStateActive = false;
            controller.animator.ResetTrigger(controller.AttackHash);
            controller.animator.ResetTrigger(controller.GroundedHash);
        }
        
        protected void DefaultState()
        {
            if(EnemySpawner.Instance.IsEmptyOrDestroyed()) return;
            var _newTarget = EnemySpawner.Instance.GetNewTileTarget(controller, targetTile);

            if (_newTarget == null)
            {
                stateMachine.ChangeState(new EnemyChasePlayerState(controller,stateMachine));
                return;
            }
            
            stateMachine.ChangeState(new EnemyGoToTileState(controller,stateMachine,_newTarget));
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
