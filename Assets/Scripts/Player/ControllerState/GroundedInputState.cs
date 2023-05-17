using CustomHelpers;
using Managers;
using UnityEngine;

namespace Player.ControllerState
{
    [System.Serializable]
    public class GroundedInputState : PlayerInputState
    {
        public GroundedInputState(PlayerInputStateMachine stateMachine_, int animNameHash_) : base(stateMachine_, animNameHash_) { }

        public override void Enter()
        {
            base.Enter();
            player.animator.SetTrigger(player.groundedHash);
        }

        public override void HandleInput()
        {
            if (InputManager.InteractAction.triggered)
            {
                player.interactDetector.Interact();
            }
            if (InputManager.UseToolAction.triggered)
            {
                //TODO: Add Use Tool State
                player.playerEquipment.UseTool();
            }
            if(InputManager.InventoryMenuAction.triggered)
            {
                UI.InventoryMenu.InventoryMenu.OnOpenInventoryMenu.Invoke(true);
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
            UpdateToolArea();
            if(IsPlayerFalling()) return;
            rb.velocity = rb.velocity.SetY(0);
        }

        public override void Exit()
        {
            base.Exit();
            player.animator.ResetTrigger(player.groundedHash);
            StateMachine.velocityOnExit = rb.velocity;
            Debug.Log($"Exit @ {StateMachine.velocityOnExit}");
        }
    }
}