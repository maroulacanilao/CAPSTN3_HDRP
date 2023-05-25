using System.Collections;
using CustomHelpers;
using Farming;
using UnityEngine;

namespace EnemyController.EnemyStates
{
    [System.Serializable]
    public class EnemyAttackTileState : EnemyControllerState
    {
        IEnumerator attackCoroutine;
        public EnemyAttackTileState(EnemyAIController aiController_, EnemyStateMachine stateMachine_) : base(aiController_, stateMachine_)
        {
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
            Debug.Log("Damage " + stateMachine.targetTile.name);
            
        }

        private IEnumerator Co_AttackTile()
        {
            while (IsWithinAttackRange(stateMachine.tileCol) && this.isStateActive)
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