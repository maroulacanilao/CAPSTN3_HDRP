using System.Collections;
using CustomHelpers;
using UnityEngine;

namespace EnemyController.EnemyStates
{
    public class EnemyHitState : EnemyControllerState
    {
        public EnemyControllerState previousState;
        
        public EnemyHitState(EnemyAIController aiController_, EnemyStateMachine stateMachine_) : base(aiController_, stateMachine_)
        {
        }

        public override void Enter()
        {
            base.Enter();
            isStateActive = true;
            StopMovement();
            if(controller.IsEmptyOrDestroyed()) return;
            controller.animator.SetTrigger(controller.HitHash);
            if(controller.IsEmptyOrDestroyed()) return;
            controller.StartCoroutine(Co_Hit());
        }
        
        
        private IEnumerator Co_Hit()
        {
            yield return new WaitForSeconds(2f);
            if (previousState == null || previousState == this)
            {
                previousState = stateMachine.patrolState;
            }
            stateMachine.ChangeState(previousState);
        }
    }
}