using System.Collections;
using CustomHelpers;
using Managers;
using UnityEngine;

namespace EnemyController.EnemyStates
{
    public class EnemyAttackState : EnemyControllerState
    {
        private Transform player => stateMachine.playerTransform;
        private Transform station;
        private CharacterController characterController;
        
        public EnemyAttackState(EnemyAIController aiController_, EnemyStateMachine stateMachine_) : base(aiController_, stateMachine_)
        {
            characterController = controller.GetComponent<CharacterController>();
            stateName = "Attack";
        }
        
        public override void Enter()
        {
            base.Enter();
            isStateActive = true;
            controller.aiPath.canMove = false;
            controller.aiPath.isStopped = true;
            controller.animator.ResetTrigger(controller.AttackHash);
            controller.animator.SetFloat(controller.attackAnimSpeedHash, controller.attackSpeed);
            controller.StartCoroutine(Co_Attack());

            controller.aiPath.maxSpeed = 0;
            controller.aiPath.enabled = false;
            characterController.enabled = false;
        }
        
        public override void Exit()
        {
            base.Exit();
            isStateActive = false;
            controller.animator.ResetTrigger(controller.AttackHash);
            controller.aiPath.enabled = true;
            characterController.enabled = true;
        }

        public override void AnimationUpdate()
        {
            return;
        }

        private IEnumerator Co_Attack()
        {
            var _animator = controller.animator;
            var _orientation = controller.transform.position.x > player.position.x ? -1 : 1;
            
            // Debug.Log(controller.transform.position.x + " " + player.position.x);
            
            _animator.SetFloat(controller.xSpeedHash, _orientation);
            yield return null;
            _animator.SetFloat(controller.attackAnimSpeedHash, controller.attackSpeed);
            
            _animator.SetTrigger(controller.AttackHash);



            yield return controller.animator.WaitForAnimationEvent(controller.AttackHitEvent,1f);
            
            if(IsWithinAttackRange(player))
            {
                GameManager.OnEnterBattle.Invoke(controller.enemyCharacter, false);
                yield break;
            }

            yield return _animator.WaitForAnimationEnd(controller.AnimationEndEvent, 0.5f);
            yield return new WaitForSeconds(controller.attackCooldown);
            
            stateMachine.ChangeState(stateMachine.chasePlayerState);
        }
    }
}