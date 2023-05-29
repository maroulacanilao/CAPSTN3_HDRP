using System.Collections;
using BaseCore;
using CustomHelpers;
using Farming;
using UnityEngine;

namespace EnemyController.EnemyStates
{
    [System.Serializable]
    public class EnemyAttackTileState : EnemyControllerState
    {
        private readonly DamageInfo damageInfo;
        public EnemyAttackTileState(EnemyAIController aiController_, EnemyStateMachine stateMachine_) : base(aiController_, stateMachine_)
        {
            damageInfo = new DamageInfo()
            {
                DamageAmount = 10,
                Source = controller.gameObject
            };
        }

        public override void Enter()
        {
            base.Enter();
            controller.aiPath.isStopped = true;
            Debug.Log("Attack Tile");
            controller.StartCoroutine(Co_AttackTile());
        }
        
        public override void Exit()
        {
            base.Exit();
            controller.StopAllCoroutines();
            controller.aiPath.isStopped = false;
            controller.animator.ResetTrigger(controller.AttackHash);
        }

        public void AttackTile()
        {
            if (!IsWithinAttackRange(stateMachine.tileCol))
            {
                Debug.Log("Not Within Attack Range");
                return;
            }
            stateMachine.targetTile.TakeDamage(damageInfo);
        }

        private IEnumerator Co_AttackTile()
        {
            while (this.isStateActive 
                   && IsWithinAttackRange(stateMachine.tileCol) 
                   && stateMachine.targetTile.tileState!= TileState.Empty)
            {
                Debug.Log("ATTACK Coroutine");
                controller.animator.SetTrigger(controller.AttackHash);
                Debug.Log("ATTACK Trigger");
            
            
                yield return controller.animator.WaitForAnimationEvent(controller.AttackHitEvent, 0.25f);
            
                AttackTile();
                yield return new WaitForSeconds(controller.attackCooldown);
                controller.animator.ResetTrigger(controller.AttackHash);
            }
            
            if (FarmTileManager.Instance.HasNonEmptyTile())
            {
                stateMachine.ChangeState(stateMachine.chaseTileState);
                yield break;
            }
            else
            {
                stateMachine.ChangeState(stateMachine.chasePlayerState);
            }
        }
    }
}