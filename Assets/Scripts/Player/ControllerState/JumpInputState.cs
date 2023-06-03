using CustomHelpers;
using Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.ControllerState
{
    [System.Serializable]
    public class JumpInputState: PlayerInputState
    {
        private bool IsHoldingButton => InputManager.JumpAction.phase == InputActionPhase.Performed;
        private float jumpTimer;
        public JumpInputState(PlayerInputStateMachine stateMachine_, int animNameHash_) : base(stateMachine_, animNameHash_)
        { }
    
        public override void Enter()
        {
            base.Enter();
            rb.velocity = new Vector3(StateMachine.velocityOnExit.x, player.jumpForce, StateMachine.velocityOnExit.z);
            rb.SetVelocity(StateMachine.velocityOnExit.SetY(player.jumpForce));
            jumpTimer = 0;
            playerState = PlayerSate.Jumping;
        }

        public override void LogicUpdate()
        {
            AnimParamUpdate();

            jumpTimer += Time.deltaTime;
        
            if(jumpTimer < player.jumpMaxTime) return;
            
            StateMachine.ChangeState(StateMachine.FallingState);
        }
        
        public override void PhysicsUpdate()
        {
            if (!player.IsGrounded && rb.velocity.y < 0 )
            {
                StateMachine.ChangeState(StateMachine.FallingState);
                return;
            }
            if (player.IsGrounded && rb.velocity.y < 0)
            {
                StateMachine.ChangeState(StateMachine.GroundedState);
            }
            
            FallUpdate(IsHoldingButton ? player.fallMult : player.jumpVelFallOff);
            MovementUpdate();
        }
        
        public override void Exit()
        {
            base.Exit();
            // player.animator.ResetTrigger(player.jumpHash);
            jumpTimer = 0;
        }
    }

    [System.Serializable]
    public class FallingPlayerInputState : PlayerInputState
    {
        public FallingPlayerInputState(PlayerInputStateMachine stateMachine_, int animNameHash_) : base(stateMachine_, animNameHash_)
        { }
    
        public override void Enter()
        {
            base.Enter();
            //player.animator.SetTrigger(player.fallHash);
            playerState = PlayerSate.Falling;
        }

        public override void LogicUpdate()
        {
            AnimParamUpdate();
        }
        
        public override void PhysicsUpdate()
        {
            if (player.IsGrounded)
            {
                StateMachine.ChangeState(StateMachine.GroundedState);
                return;
            }
            MovementUpdate();
            FallUpdate(player.jumpVelFallOff);
        }
        
        public override void Exit()
        {
            base.Exit();
            //player.animator.ResetTrigger(player.fallHash);
        }
    }
}