using System;
using CustomEvent;
using CustomHelpers;
using Farming;
using NaughtyAttributes;
using Player.ControllerState;
using UnityEngine;

namespace Player
{
    public class PlayerInputController : MonoBehaviour
    {
        #region Movement Properties


        [field: Header("Movement Properties")] 
        [field: SerializeField] public float movementSpeed { get; private set; } = 8;
        [field: SerializeField] public float acceleration { get; private set; } = 6;
        [field: SerializeField] public float currentMoveLerpSpeed { get; private set; } = 100;


        #endregion
    
        #region Jump Properties

        [field: Header("Jump Physics Properties")] 
        
        [field: SerializeField] public float jumpForce { get; private set; } = 16;
        [field: SerializeField] public float fallMult { get; private set; } = 4;
        [field: SerializeField] public float jumpVelFallOff { get; private set; } = 8;

        [field: Header("Jump General Properties")] 
        [field: SerializeField] public float jumpBufferTime { get; private set; } = 0.1f;
        [field: SerializeField] public float coyoteTime { get; private set; } = 0.1f;
        [field: SerializeField] public float jumpMaxTime { get; private set; } = 0.35f;

        #endregion
        
        #region Animation Parameters
        [field: Header("Animation Parameters")]
        [field: AnimatorParam("animator")] [field: Foldout("Animation Hashes")]
        [field: SerializeField] public int groundedHash { get; private set; }
        [field: AnimatorParam("animator")] [field: Foldout("Animation Hashes")]
        [field: SerializeField] public int isIdleHash { get; private set; }
        [field: AnimatorParam("animator")] [field: Foldout("Animation Hashes")]
        [field: SerializeField] public int xSpeedHash { get; private set; }
        [field: AnimatorParam("animator")] [field: Foldout("Animation Hashes")]
        [field: SerializeField] public int ySpeedHash { get; private set; }

        #endregion

        #region Animation Events
        [field: Header("Animation Event")]
        
        [field: Foldout("Animation Events")]
        [field: SerializeField] public string JumpStartEvent { get; private set; }
        #endregion

        #region OtherComponents
        
        [field: Header("Components")]
        [field: SerializeField] public Animator animator { get; private set; }
        [field: SerializeField] public AnimationEventReceiver animationEventReceiver { get; private set; }
        [field: SerializeField] public MovementCollisionDetector collisionDetector { get; private set; }
        [field: SerializeField] public PlayerEquipment playerEquipment { get; private set; }
        [field: SerializeField] public InteractDetector interactDetector { get; private set; }
        
        #endregion
        
        public Rigidbody rb { get; private set; }
        
        public Transform cameraTransform { get; private set; }
        
        public ToolArea toolArea { get; private set; }
    
        private PlayerInputStateMachine StateMachine;
        
        public bool IsGrounded => collisionDetector.isGrounded;
        
        [Header("Components To Disable On Freeze")]
        [SerializeField] private MonoBehaviour[] scriptsToDisable;
        
        public static readonly Evt<bool> OnPlayerCanMove = new Evt<bool>();

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            OnPlayerCanMove.AddListener(CanMove);
            cameraTransform = gameObject.scene.GetFirstMainCameraInScene().transform;
            toolArea = ToolArea.Instance;
            StateMachine = new PlayerInputStateMachine(this);

        }

        private void OnDestroy()
        {
            OnPlayerCanMove.RemoveListener(CanMove);
        }

        private void OnEnable()
        {
            StateMachine.Initialize();
        }

        private void OnDisable()
        {
            rb.velocity = Vector3.zero;
        }

        private void Update()
        {
            StateMachine.StateUpdate();
        }

        private void FixedUpdate()
        {
            StateMachine.StateFixedUpdate();
        }
        
        public void CanMove(bool canMove_)
        {
            foreach (var _component in scriptsToDisable)
            {
                _component.enabled = canMove_;
            }
        }
    }
}