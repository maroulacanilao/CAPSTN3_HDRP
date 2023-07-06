using System.Collections;
using CustomHelpers;
using Managers;
using UI;
using UI.Farming;
using UnityEngine;

namespace Player.ControllerState
{
    [System.Serializable]
    public class GroundedInputState : PlayerInputState
    {
        private bool canInput;
        private PlayerEquipment farmEquipment;
        private InteractDetector interactDetector;

        public GroundedInputState(PlayerInputStateMachine stateMachine_) : base(stateMachine_)
        {
            farmEquipment = player.playerEquipment;
            interactDetector = player.interactDetector;
        }
        
        public override void Enter()
        {
            base.Enter();
            player.animator.SetTrigger(player.groundedHash);
            canInput = false;
            player.StartCoroutine(InputDelay());
            playerState = PlayerSate.Grounded;
            PlayerEquipment.OnTillAction.AddListener(Till);
            PlayerEquipment.OnWaterAction.AddListener(Watering);
            PlayerEquipment.OnUnTillAction.AddListener(UnTill);
            StateMachine.canMove = true;
        }

        public override void HandleInput()
        {
            if(!canInput) return;
            if(PlayerMenu.OpenedMenu.IsValid() && PlayerMenu.OpenedMenu.isActiveAndEnabled) return;

            if (InputManager.UseToolAction.triggered)
            {
                if(player.CanUseFarmTools) farmEquipment.UseTool();
                
                else StateMachine.ChangeState(StateMachine.AttackState);
                
                return;
            }
            
            if(interactDetector.CanInteract() && InputManager.InteractAction.triggered)
            {
                interactDetector.Interact();
                return;
            }
            
            if (CanJump())
            {
                StateMachine.ChangeState(StateMachine.JumpState);
                return;
            }
        }

        public override void LogicUpdate()
        {
            AnimParamUpdate();
        }

        public override void PhysicsUpdate()
        {
            MovementUpdate();
            if(IsPlayerFalling()) return;
            rb.velocity = rb.velocity.SetY(0);
        }

        public override void Exit()
        {
            base.Exit();
            player.StopAllCoroutines();
            player.animator.ResetTrigger(player.groundedHash);
            StateMachine.velocityOnExit = rb.velocity;
            canInput = false;
            PlayerEquipment.OnTillAction.RemoveListener(Till);
            PlayerEquipment.OnWaterAction.RemoveListener(Watering);
            PlayerEquipment.OnUnTillAction.RemoveListener(UnTill);
        }
        
        private IEnumerator InputDelay()
        {
            canInput = false;
            yield return new WaitForSeconds(0.1f);
            canInput = true;
        }

        private void Till()
        {
            StateMachine.ChangeState(StateMachine.TillState);
        }
        
        private void UnTill()
        {
            StateMachine.ChangeState(StateMachine.UnTillState);
        }

        private void Watering()
        {
            StateMachine.ChangeState(StateMachine.WateringState);
        }
    }
}