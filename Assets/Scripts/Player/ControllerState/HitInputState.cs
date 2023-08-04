using System.Collections;
using UnityEngine;

namespace Player.ControllerState
{
    public class HitInputState : PlayerInputState
    {
        public HitInputState(PlayerInputStateMachine stateMachine_) : base(stateMachine_)
        {
        }
        
        public override void Enter()
        {
            base.Enter();
            player.animator.ResetTrigger(player.groundedHash);
            player.animator.ResetTrigger(player.jumpHash);
            player.animator.ResetTrigger(player.attackHash);
            player.animator.SetTrigger(player.hitHash);
            playerState = PlayerSate.Hit;
            StateMachine.canMove = false;
            player.rb.velocity = Vector3.zero;
            player.StartCoroutine(Co_Hit());
        }
        
        public override void Exit()
        {
            base.Exit();
            StateMachine.canMove = true;
            player.StopAllCoroutines();
            player.animator.ResetTrigger(player.hitHash);
        }

        private IEnumerator Co_Hit()
        {
            yield return new WaitForSeconds(2f);
            StateMachine.ChangeState(StateMachine.GroundedState);
        }
    }
}