using System.Collections;
using CustomHelpers;
using Managers;
using UnityEngine;

namespace Player.ControllerState
{
    public abstract class ToolInputSate : PlayerInputState
    {
        public ToolInputSate(PlayerInputStateMachine stateMachine_) : base(stateMachine_)
        { }
        
        public override void Enter()
        {
            base.Enter();
            playerState = PlayerSate.UsingTool;
            player.StartCoroutine(Co_WaitForAnimEvent());
            StateMachine.canMove = false;
            player.rb.velocity = Vector3.zero;
        }
        
        public override void Exit()
        {
            base.Exit();
            player.StopAllCoroutines();
            StateMachine.canMove = true;
        }
        
        public IEnumerator Co_WaitForAnimEvent()
        {
            yield return player.animator.WaitForAnimationEvent(animEndEvent, 2f);
            ToolAction();
            StateMachine.ChangeState(StateMachine.GroundedState);
        }
        
        protected virtual void ToolAction() { }
    }
    
    public class TillInputState : ToolInputSate
    {
        public TillInputState(PlayerInputStateMachine stateMachine_) : base(stateMachine_)
        {
            animEndEvent = player.hoeEnd;
        }
        
        public override void Enter()
        {
            player.animator.SetTrigger(player.hoeHash);
            base.Enter();
            AudioManager.PlayPlow();
        }
        
        public override void Exit()
        {
            player.animator.ResetTrigger(player.hoeHash);
            base.Exit();
        }

        protected override void ToolAction()
        {
            player.playerEquipment.Till();
        }
    }

    public class UnTillInputState : TillInputState
    {
        public UnTillInputState(PlayerInputStateMachine stateMachine_) : base(stateMachine_)
        {
            animEndEvent = player.hoeEnd;
        }

        protected override void ToolAction()
        {
            player.playerEquipment.UnTill();
        }
    }

    public class WateringInputState : ToolInputSate
    {
        public WateringInputState(PlayerInputStateMachine stateMachine_) : base(stateMachine_)
        {
            animEndEvent = player.waterEnd;
        }


        public override void Enter()
        {
            player.animator.SetTrigger(player.wateringHash);
            base.Enter();
            AudioManager.PlayWatering();
        }
        
        public override void Exit()
        {
            player.animator.ResetTrigger(player.wateringHash);
            base.Exit();
        }

        protected override void ToolAction()
        {
            player.playerEquipment.Water();
        }
    }
}