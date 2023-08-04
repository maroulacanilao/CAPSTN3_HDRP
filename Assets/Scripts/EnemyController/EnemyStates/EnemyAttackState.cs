using System.Collections;
using CustomHelpers;
using Interface;
using Managers;
using ObjectPool;
using UnityEngine;

namespace EnemyController.EnemyStates
{
    public class EnemyAttackState : EnemyControllerState
    {
        private Transform player => stateMachine.playerTransform;
        private Transform station;

        public EnemyAttackState(EnemyAIController aiController_, EnemyStateMachine stateMachine_) : base(aiController_, stateMachine_)
        {
            stateName = "Attack";
        }
        
        public override void Enter()
        {
            base.Enter();
            isStateActive = true;
            StopMovement();
            controller.animator.ResetTrigger(controller.AttackHash);
            controller.animator.SetFloat(controller.attackAnimSpeedHash, controller.attackSpeed);
            controller.StartCoroutine(Co_Attack());
        }
        
        public override void Exit()
        {
            base.Exit();
            isStateActive = false;
            controller.animator.ResetTrigger(controller.AttackHash);
            ResumeMovement();
        }

        public override void AnimationUpdate()
        {
            return;
        }

        private IEnumerator Co_Attack()
        {
            var _animator = controller.animator;
            if(player == null)
            {
                stateMachine.ChangeState(stateMachine.patrolState);
                yield break;
            }
            var _orientation = controller.transform.position.x > player.position.x ? -1 : 1;
            
            // Debug.Log(controller.transform.position.x + " " + player.position.x);
            
            _animator.SetFloat(controller.xSpeedHash, _orientation);
            yield return null;
            _animator.SetFloat(controller.attackAnimSpeedHash, controller.attackSpeed);
            
            _animator.SetTrigger(controller.AttackHash);

            
            yield return controller.animator.WaitForAnimationEvent(controller.AttackHitEvent,1f);
            
            if(IsWithinAttackRange(player))
            {
                if(player == null)
                {
                    stateMachine.ChangeState(stateMachine.patrolState);
                    yield break;
                }
                var _pos = controller.transform.GetMiddlePosition(player.transform).AddY(0.5f);
                
                AssetHelper.PlayHitEffect(_pos, Quaternion.identity);
                
                var _hit = player.GetComponent<IHittable>();
                _hit?.Hit();
                yield return new WaitForSeconds(0.2f);
                
                GameManager.OnEnterBattle.Invoke(controller.enemyCharacter, false);
                yield break;
            }

            yield return _animator.WaitForAnimationEnd(controller.AnimationEndEvent, 0.5f);
            yield return new WaitForSeconds(controller.attackCooldown);
            
            stateMachine.ChangeState(stateMachine.chasePlayerState);
        }
    }
}