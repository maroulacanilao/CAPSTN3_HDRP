using BaseCore;
using Character;
using Character.CharacterComponents;
using CustomHelpers;
using Managers;
using ScriptableObjectData.CharacterData;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.ControllerState
{
    [System.Serializable]
    public class JumpInputState: PlayerInputState
    {
        private bool IsHoldingButton => InputManager.JumpAction.phase == InputActionPhase.Performed;
        private float jumpTimer;
        public JumpInputState(PlayerInputStateMachine stateMachine_) : base(stateMachine_)
        { }
    
        public override void Enter()
        {
            base.Enter();
            rb.velocity = new Vector3(StateMachine.velocityOnExit.x, player.jumpForce, StateMachine.velocityOnExit.z);
            rb.SetVelocity(StateMachine.velocityOnExit.SetY(player.jumpForce));
            jumpTimer = 0;
            playerState = PlayerSate.Jumping;
            player.animator.SetTrigger(player.jumpHash);
            AudioManager.PlayJump();
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
            player.animator.ResetTrigger(player.jumpHash);
            jumpTimer = 0;
        }
    }

    [System.Serializable]
    public class FallingPlayerInputState : PlayerInputState
    {
        private PlayerHealth playerHealth;
        private Vector3 startingPos;
        public FallingPlayerInputState(PlayerInputStateMachine stateMachine_) : base(stateMachine_)
        {
            var _data = player.GetComponent<CharacterBase>().characterData as PlayerData;
            if(_data == null) return;
            playerHealth = _data.health;
        }
    
        public override void Enter()
        {
            base.Enter();
            //player.animator.SetTrigger(player.fallHash);
            playerState = PlayerSate.Falling;
            startingPos = player.transform.position;
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
            FallDamage();
            //player.animator.ResetTrigger(player.fallHash);
        }

        public void FallDamage()
        {
            if(playerHealth == null) return;
            var _pos = player.transform.position;
            
            var _fallDist = Mathf.Abs(startingPos.y - _pos.y);
            if(_fallDist < 5f) return;
            var _dam = new DamageInfo((int)_fallDist, null);
            playerHealth.TakeDamage(_dam);
        }
    }
}