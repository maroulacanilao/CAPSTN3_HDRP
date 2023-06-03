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
        private readonly Collider tileCol;
        
        public EnemyAttackTileState(EnemyAIController aiController_, EnemyStateMachine stateMachine_, FarmTile targetTile_) : base(aiController_, stateMachine_)
        {
            Debug.Log("TargetTile");
            stateName = "Attack Tile";
            damageInfo = new DamageInfo()
            {
                DamageAmount = 10,
                Source = controller.gameObject
            };
            targetTile = targetTile_;
            
            if(targetTile == null) return;
            tileCol = targetTile.GetComponent<Collider>();
        }

        public override void Enter()
        {
            base.Enter();
            if (targetTile == null)
            {
                DefaultState();
                return;
            }
            controller.aiPath.isStopped = true;
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
            if (!IsWithinAttackRange(tileCol))
            {
                return;
            }
            targetTile.TakeDamage(damageInfo);
        }

        private IEnumerator Co_AttackTile()
        {
            while (this.isStateActive &&
                   IsWithinAttackRange(tileCol) &&
                   targetTile.tileState!= TileState.Empty)
            {
                controller.animator.SetTrigger(controller.AttackHash);
                
                yield return controller.animator.WaitForAnimationEvent(controller.AttackHitEvent, 1f);
            
                AttackTile();
                yield return new WaitForSeconds(controller.attackCooldown);
                controller.animator.ResetTrigger(controller.AttackHash);
            }
            
            DefaultState();
        }
    }
}