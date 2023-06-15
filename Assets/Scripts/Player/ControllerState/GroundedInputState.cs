using System.Collections;
using CustomHelpers;
using Managers;
using UI;
using UnityEngine;

namespace Player.ControllerState
{
    [System.Serializable]
    public class GroundedInputState : PlayerInputState
    {
        private bool canInput;
        public GroundedInputState(PlayerInputStateMachine stateMachine_, int animNameHash_) : base(stateMachine_, animNameHash_)
        { }
        
        public override void Enter()
        {
            base.Enter();
            player.animator.SetTrigger(player.groundedHash);
            canInput = false;
            player.StartCoroutine(InputDelay());
            playerState = PlayerSate.Grounded;
        }

        public override void HandleInput()
        {
            if(!canInput) return;
            if (InputManager.InteractAction.triggered)
            {
                player.playerEquipment.UseTool();
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
        }
        
        private IEnumerator InputDelay()
        {
            canInput = false;
            yield return new WaitForSeconds(0.1f);
            canInput = true;
        }
    }
}