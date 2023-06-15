using System.Collections;
using BaseCore;
using CustomHelpers;
using Managers;
using UnityEngine;

namespace Player.ControllerState
{
    public enum PlayerSate { Grounded, Jumping, Falling, Swimming, Climbing, Dashing, Attacking, UsingTool, Interacting, Menu }
    
    [System.Serializable]
    public abstract class PlayerInputState : UnitState
    {
        protected readonly PlayerInputStateMachine StateMachine;
        protected readonly PlayerInputController player;

        protected readonly Rigidbody rb;
        protected bool isStateActive;
        public PlayerSate playerState { get; protected set; }

        protected PlayerInputState(PlayerInputStateMachine stateMachine_, int animEndEventHash_)
        {
            animEndEventHash = animEndEventHash_;
            player = stateMachine_.player;
            rb = player.rb;
            StateMachine = stateMachine_;
        }

        public override void Enter()
        {
            isStateActive = true;
        }
        
        public override void Exit()
        {
            isStateActive = false;
        }

        protected void FallUpdate(float fallVal_)
        {
            if(player.IsGrounded) return;
            rb.velocity += Vector3.up * (fallVal_ * Physics.gravity.y * Time.deltaTime);
        }

        protected void MovementUpdate()
        {
            // Mathf.Lerp(player.transform.forward.x, StateMachine.input.x, player.currentMoveLerpSpeed * Time.deltaTime);
            
            var _newX = StateMachine.input.x * player.movementSpeed;
            var _newZ = StateMachine.input.y * player.movementSpeed;

            var _newVelocity = new Vector3(_newX, rb.velocity.y, _newZ);

            StateMachine.playerVel = Vector3.Slerp(StateMachine.playerVel, _newVelocity, player.currentMoveLerpSpeed * Time.deltaTime);
        }
        
        protected bool CanJump()
        {
            if (Time.time > StateMachine.timeLastJumpPressed + player.jumpBufferTime) return false;
            if (Time.time > StateMachine.timeLastJumpPressed + player.coyoteTime) return false;
        
            return true;
        }
        
        protected void AnimParamUpdate()
        {
            Vector2 _input = InputManager.MoveDelta;
            var _isIdle = _input.magnitude.IsApproximatelyTo(0);
            player.animator.SetBool(player.isIdleHash, _isIdle);
            
            if(_isIdle) return;
            
            if (Mathf.Abs(_input.x) < 0.01f)
            {
                var _val = Mathf.Sign(_input.y);
                player.animator.SetFloat(player.ySpeedHash, _val);
                player.animator.SetFloat(player.xSpeedHash, 0);
                StateMachine.direction = new Vector3(0,0,_val);
            }
            else
            {
                var _val = Mathf.Sign(_input.x);
                player.animator.SetFloat(player.xSpeedHash, _val);
                player.animator.SetFloat(player.ySpeedHash, 0);
                StateMachine.direction = new Vector3(_val,0,0);
            }
        }

        protected bool IsPlayerFalling()
        {
            if(player.IsGrounded) return false;
        
            StateMachine.ChangeState(StateMachine.FallingState);
            return true;
        }

        protected void DefaultState()
        {
            if(player.IsGrounded) StateMachine.ChangeState(StateMachine.GroundedState);
            else StateMachine.ChangeState(StateMachine.FallingState);
        }
        
        protected IEnumerator FallbackState(float waitTime_)
        {
            yield return new WaitForSeconds(waitTime_);
            StateMachine.ChangeState(StateMachine.GroundedState);
        }
    }
}