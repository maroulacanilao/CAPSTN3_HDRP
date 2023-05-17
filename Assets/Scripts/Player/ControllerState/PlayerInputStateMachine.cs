using BaseCore;
using CustomHelpers;
using Managers;
using UnityEngine;

namespace Player.ControllerState
{
    [System.Serializable]
    public class PlayerInputStateMachine : UnitStateMachine
    {
        #region States

        public GroundedInputState GroundedState { get; private set; }
        public JumpInputState JumpState { get; private set; }
        public FallingPlayerInputState FallingState { get; private set; }

        #endregion
        
        public PlayerInputController player { get; private set; }
        
        public Vector3 playerVel
        {
            get => player.rb.velocity;
            set => player.rb.velocity = value;
        }
        
        public Vector3 direction;

        public Vector2 input;

        public Vector3 velocityOnExit { get; set; }
        
        public float timeLastJumpPressed { get; private set; }
        // public float currentAcceleration { get; private set; }
        // public float currentMoveSpeed { get; private set; }
        public bool IsInputIdle { get; private set; }

        public PlayerInputStateMachine(PlayerInputController player_)
        {
            player = player_;
        }

        public override void Initialize()
        {
            GroundedState = new GroundedInputState(this, 0);
            JumpState = new JumpInputState(this, player.JumpStartEvent.ToHash());
            FallingState = new FallingPlayerInputState(this, 0);
            timeLastJumpPressed = -1;
            
            CurrentState = GroundedState;
            CurrentState.Enter();
        }

        public void ChangeState(PlayerInputState newState_)
        {
            CurrentState?.Exit();

            CurrentState = newState_;
            CurrentState?.Enter();
        }

        public override void StateUpdate()
        {
            InputUpdate();
            CurrentState.HandleInput();
            CurrentState.LogicUpdate();
        }
    
        public override void StateFixedUpdate()
        {
            CurrentState.PhysicsUpdate();
        }

        private void InputUpdate()
        {
            input = InputManager.MoveDelta;

            // input smoothing x
            switch (input.x)
            {
                case < 0:
                {
                    if (playerVel.x > 0) input.x = 0;
                    input.x = Mathf.Lerp(input.x, -1, player.acceleration * Time.deltaTime);
                    break;
                }
                case > 0:
                {
                    if (input.x < 0) input.x = 0;
                    input.x = Mathf.Lerp(input.x, 1, player.acceleration * Time.deltaTime);
                    break;
                }
                default:
                {
                    input.x = Mathf.Lerp(input.x, 0, player.acceleration * Time.deltaTime);
                    break;
                }
            }
            
            // input smoothing x
            switch (input.y)
            {
                case < 0:
                {
                    if (playerVel.y > 0) input.y = 0;
                    input.y = Mathf.Lerp(input.y, -1, player.acceleration * Time.deltaTime);
                    break;
                }
                case > 0:
                {
                    if (input.y < 0) input.y = 0;
                    input.y = Mathf.Lerp(input.y, 1, player.acceleration * Time.deltaTime);
                    break;
                }
                default:
                {
                    input.y = Mathf.Lerp(input.y, 0, player.acceleration * Time.deltaTime);
                    break;
                }
            }
            
            IsInputIdle = input.x.IsApproximatelyTo(0) && input.y.IsApproximatelyTo(0);
            
            // jump input
            if (InputManager.JumpAction.triggered)
            {
                timeLastJumpPressed = Time.time;
            }
        }
    }
}