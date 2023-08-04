using EnemyController.EnemyStates;
using UnityEngine;

namespace EnemyController.Inheritors
{
    public class TiyanakDisguiseState : EnemyControllerState
    {
        private TiyanakController tiyanakController;
        public TiyanakDisguiseState(EnemyAIController aiController_, EnemyStateMachine stateMachine_) : base(aiController_, stateMachine_)
        {
        }
        
        public override void Enter()
        {
            base.Enter();
            controller.animator.ResetTrigger(controller.GroundedHash);
            StopMovement();

            isStateActive = true;
            if (controller is not TiyanakController tiyanakController_)
            {
                stateMachine.ChangeState(stateMachine.patrolState);
                return;
            }
            
            tiyanakController = tiyanakController_;
            tiyanakController.animator.SetTrigger(tiyanakController.idleDisguiseHash);
            
            controller.alertRange.OnPlayerNearby.AddListener(OnPlayerNearby);
        }
        
        public override void Exit()
        {
            base.Exit();
            isStateActive = false;
            controller.alertRange.OnPlayerNearby.RemoveListener(OnPlayerNearby);
            tiyanakController.animator.ResetTrigger(tiyanakController.idleDisguiseHash);
            ResumeMovement();
        }
        
        private void OnPlayerNearby(Transform player_)
        {
            if(player_ != stateMachine.playerTransform) return;
            stateMachine.ChangeState(stateMachine.chasePlayerState);
        }


    }
}