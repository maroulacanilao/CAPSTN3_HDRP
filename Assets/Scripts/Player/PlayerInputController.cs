using System;
using BaseCore;
using CustomEvent;
using CustomHelpers;
using Farming;
using Managers;
using NaughtyAttributes;
using Player.ControllerState;
using UI;
using UI.Farming;
using UnityEngine;

namespace Player
{
    [DefaultExecutionOrder(-1)]
    public class PlayerInputController : MonoBehaviour
    {
        [field: SerializeField] public bool CanUseFarmTools { get; private set; } = true;
        
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
        
        [field: AnimatorParam("animator")] [field: Foldout("Animation Hashes")]
        [field: SerializeField] public int hoeHash { get; private set; }
        
        [field: AnimatorParam("animator")] [field: Foldout("Animation Hashes")]
        [field: SerializeField] public int wateringHash { get; private set; }
        
        [field: AnimatorParam("animator")] [field: Foldout("Animation Hashes")]
        [field: SerializeField] public int jumpHash { get; private set; }
        
        [field: AnimatorParam("animator")] [field: Foldout("Animation Hashes")]
        [field: SerializeField] public int attackHash { get; private set; }
        [field: AnimatorParam("animator")] [field: Foldout("Animation Hashes")]
        [field: SerializeField] public int attackSpeedHash { get; private set; }
        #endregion

        #region Animation Events
        [field: Header("Animation Event")]
        
        [field: Foldout("Animation Events")]
        [field: SerializeField] public string JumpStartEvent { get; private set; }
        
        [field: Foldout("Animation Events")]
        [field: SerializeField] public string hoeEnd { get; private set; }
        
        [field: Foldout("Animation Events")]
        [field: SerializeField] public string waterEnd { get; private set; }
        
        [field: Foldout("Animation Events")]
        [field: SerializeField] public string attackHitEvent { get; private set; }
        
        [field: Foldout("Animation Events")]
        [field: SerializeField] public string attackEndEvent { get; private set; }
        #endregion
        
        #region OtherComponents
        
        [field: Header("Components")]
        [field: SerializeField] public Animator animator { get; private set; }
        [field: SerializeField] public AnimationEventReceiver animationEventReceiver { get; private set; }
        [field: SerializeField] public MovementCollisionDetector collisionDetector { get; private set; }
        [field: SerializeField] public PlayerEquipment playerEquipment { get; private set; }
        [field: SerializeField] public InteractDetector interactDetector { get; private set; }
        
        [field: SerializeField] public Light lanternLight { get; private set; }

        #endregion

        #region Attack Properties
        
        [field: Header("Attack Properties")]
        [field: SerializeField] public float attackCooldown { get; private set; } = 0.5f;
        [field: SerializeField] public float attackSpeed{ get; private set; } = 3f;
        [field: SerializeField] public Vector3 attackOffset { get; private set; }
        [field: SerializeField] public Vector3 attackSize { get; private set; }
        [field: SerializeField] public LayerMask enemyLayer { get; private set; }
        [field: SerializeField] [field:Tag] public string enemyTag { get; private set; }

        #endregion

        public Rigidbody rb
        {
            get
            {
                if(mRb == null) mRb = GetComponent<Rigidbody>();
                return mRb;
            }
        }

        public ToolArea toolArea
        {
            get
            {
                if(mToolArea == null) mToolArea = ToolArea.Instance;
                return mToolArea;
            }
        }

        public Vector3 moveDirection { get; set; }
    
        private PlayerInputStateMachine StateMachine;
        
        public bool IsGrounded => collisionDetector.isGrounded;
        private Rigidbody mRb;
        private ToolArea mToolArea;

        public PlayerSate playerState
        {
            get
            {
                if (StateMachine.CurrentState is PlayerInputState _state) return _state.playerState;
                return PlayerSate.Grounded;
            }
        }

        private static PlayerInputController mInstance;
        public static PlayerInputController Instance
        {
            get
            {
                if (mInstance.IsValid()) return mInstance;
                if (!ReferenceEquals(mInstance, null) && mInstance == null) return null;

                // get instances
                var objs = FindObjectsOfType<PlayerInputController>();


                if (objs.Length == 0)
                {
                    // create instance if there is none
                    var _prefab = Resources.LoadAll<PlayerInputController>("Prefabs");
                    mInstance = Instantiate(_prefab[0], Vector3.zero, Quaternion.identity);
                }

                else if (objs.Length == 1)
                {
                    mInstance = objs[0];
                }

                else if (objs.Length > 1)
                {
                    mInstance = objs[0];
                    for (var i = objs.Length - 1; i > -1; --i)
                    {
                        if (mInstance == objs[i]) continue;
                        Destroy(objs[i].gameObject);
                    }
                }

                return mInstance;
            }

            protected set => mInstance = value;
        }

        private void Awake()
        {
            if(Instance == null) Instance = this;
            else if(Instance != this && this.IsValid() && gameObject.IsValid()) Destroy(gameObject);
            
            DontDestroyOnLoad(this);

            StateMachine = new PlayerInputStateMachine(this);
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

        public void SetCanUseFarmTools(bool value_)
        {
            CanUseFarmTools = value_;

            playerEquipment.enabled = CanUseFarmTools;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position + attackOffset, attackSize);
        }
    }
}