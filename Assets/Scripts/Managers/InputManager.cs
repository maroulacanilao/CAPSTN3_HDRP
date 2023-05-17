using BaseCore;
using CustomEvent;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Managers
{

    public class InputManager : SingletonPersistent<InputManager>, PlayerInputAction.IExploreActions
    {
        private PlayerInput _playerInput;
        public static InputAction MoveAction { get; private set; }
        public static InputAction JumpAction { get; private set; }
        public static InputAction InteractAction { get; private set; }
        public static InputAction SprintAction { get; private set; }
        public static InputAction UseToolAction { get; private set; }
        public static InputAction InventoryMenuAction { get; private set; }
        
        public static Vector2 MoveDelta { get; private set; }

        #region Events

        public static readonly Evt<InputAction.CallbackContext> OnMoveAction = new Evt<InputAction.CallbackContext>();
        public static readonly Evt<InputAction.CallbackContext> OnJumpAction = new Evt<InputAction.CallbackContext>();
        public static readonly Evt<InputAction.CallbackContext> OnInteractAction = new Evt<InputAction.CallbackContext>();
        public static readonly Evt<InputAction.CallbackContext> OnUseToolAction = new Evt<InputAction.CallbackContext>();
        public static readonly Evt<InputAction.CallbackContext> OnSprintAction = new Evt<InputAction.CallbackContext>();
        public static readonly Evt<InputAction.CallbackContext> OnInventoryMenuAction = new Evt<InputAction.CallbackContext>();
        public static readonly Evt<bool> OnCycleTool= new Evt<bool>();
        public static readonly Evt<int> OnSelectTool = new Evt<int>();
        #endregion

        protected override void Awake()
        {
            base.Awake();
            MoveDelta = new Vector2();
            _playerInput = GetComponent<PlayerInput>();
            MoveAction = _playerInput.actions["Move"];
            JumpAction = _playerInput.actions["Jump"];
            InteractAction = _playerInput.actions["Interact"];
            SprintAction = _playerInput.actions["Sprint"];
            UseToolAction = _playerInput.actions["UseTool"];
            InventoryMenuAction = _playerInput.actions["InventoryMenu"];
        }

        public void OnMove(InputAction.CallbackContext inputContext_)
        {
            MoveDelta = inputContext_.ReadValue<Vector2>();
            OnMoveAction.Invoke(inputContext_);
        }

        public void OnJump(InputAction.CallbackContext inputContext_)
        {
            OnJumpAction.Invoke(inputContext_);
        }
        
        public void OnInteract(InputAction.CallbackContext context)
        {
            OnInteractAction.Invoke(context);
        }
        
        public void OnInventoryMenu(InputAction.CallbackContext context)
        {
            OnInventoryMenuAction.Invoke(context);
        }
        
        public void OnUseTool(InputAction.CallbackContext context)
        {
            OnUseToolAction.Invoke(context);
        }

        public void OnFirstTool(InputAction.CallbackContext context)
        {
            if(!context.started) return;
            OnSelectTool.Invoke(0);
        }
        
        public void OnSecondTool(InputAction.CallbackContext context)
        {
            if(!context.started) return;
            OnSelectTool.Invoke(1);
        }
        
        public void OnThirdTool(InputAction.CallbackContext context)
        {
            if(!context.started) return;
            OnSelectTool.Invoke(2);
        }
        
        public void OnFourthTool(InputAction.CallbackContext context)
        {
            if(!context.started) return;
            OnSelectTool.Invoke(3);
        }

        public void OnNextTool(InputAction.CallbackContext context)
        {
            if(!context.started) return;
            OnCycleTool.Invoke(true);
        }
        
        public void OnPrevTool(InputAction.CallbackContext context)
        {
            if(!context.started) return;
            OnCycleTool.Invoke(false);
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            OnSprintAction.Invoke(context);
        }
    }
}