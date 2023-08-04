using System;
using System.Collections;
using Character;
using CustomHelpers;
using Dungeon;
using EnemyController.EnemyStates;
using Farming;
using Interface;
using Managers;
using NaughtyAttributes;
using Pathfinding;
using UnityEngine;
using UnityEngine.AI;

namespace EnemyController
{
    public class EnemyAIController : MonoBehaviour, IHittable
    {
        #region Components

        [field: BoxGroup("Components")] [field: SerializeField] 
        public Animator animator { get; private set; }
        
        [field: BoxGroup("Components")] [field: SerializeField] 
        public AnimationEventReceiver animationEvents { get; private set; }
        
        [field: BoxGroup("Components")] [field: SerializeField] 
        public  AIPath  aiPath { get; private set; }
        
        [field: BoxGroup("Components")] [field: SerializeField] 
        public Collider attackRangeCollider { get; private set; }
        
        [field: BoxGroup("Components")] [field: SerializeField]
        public EnemyAlertRange alertRange { get; private set; }
        
        [field: BoxGroup("Components")] [field: SerializeField] 
        public EnemyCharacter enemyCharacter { get; private set; }
        
        

        #endregion

        #region Attack And Movement properties

        [field: BoxGroup("Movement")] [field: SerializeField] 
        public float chaseSpeed { get; private set; }
        
        [field: BoxGroup("Movement")] [field: SerializeField] 
        public float patrolSpeed { get; private set; }
        
        [field: BoxGroup("Movement")] [field: SerializeField]
        public float chaseRange { get; private set; }
        
        [field: BoxGroup("Attack Properties")] [field: SerializeField]
        public float attackRange { get; private set; }
        
        [field: BoxGroup("Attack Properties")] [field: SerializeField]
        public float attackCooldown { get; private set; }
        
        [field: BoxGroup("Attack Properties")] [field: SerializeField]
        public float attackSpeed { get; private set; }
        
        
        #endregion

        #region Animation Parameters
        
        [field: BoxGroup("Animation Parameter")] [field: AnimatorParam("animator")] [field: SerializeField]
        public int GroundedHash { get; private set; }
        
        [field: BoxGroup("Animation Parameter")] [field: AnimatorParam("animator")] [field: SerializeField]
        public int AttackHash { get; private set; }
                
        [field: BoxGroup("Animation Parameter")] [field: AnimatorParam("animator")] [field: SerializeField]
        public int xSpeedHash { get; private set; }
        
        [field: BoxGroup("Animation Parameter")] [field: AnimatorParam("animator")] [field: SerializeField]
        public int HitHash { get; private set; }
        
        [field: BoxGroup("Animation Parameter")] [field: AnimatorParam("animator")] [field: SerializeField]
        public int DeathHash { get; private set; }

        [field: BoxGroup("Animation Parameter")] [field: AnimatorParam("animator")] [field: SerializeField]
        public int IsIdleHash { get; private set; }
        
        [field: BoxGroup("Animation Parameter")] [field: AnimatorParam("animator")] [field: SerializeField]
        public int attackAnimSpeedHash { get; private set; }

        #endregion
        
        #region Animation Events
        
        [field: BoxGroup("Animation Event")] [field: SerializeField]
        public string AttackHitEvent { get; private set; }
        
        [field: BoxGroup("Animation Event")] [field: SerializeField]
        public string AnimationEndEvent { get; private set; }

        #endregion

        [SerializeReference] [ReadOnly] protected EnemyStateMachine stateMachine;
        
        public EnemyStation station { get; protected set; }
        
        public CharacterController movementController { get; private set; }

        private void Awake()
        {
            movementController = GetComponent<CharacterController>();
        }

        public virtual void Initialize(EnemyStation station_)
        {
            station = station_;
            stateMachine = new EnemyStateMachine(this);
            
            stateMachine.Initialize();
        }

        protected virtual void OnEnable()
        {
            stateMachine?.patrolState?.TeleportToRandomWaypoint();
            stateMachine?.ChangeState(stateMachine?.patrolState);
            TimeManager.OnPauseTime.AddListener(OnPause);
        }

        protected void OnDisable()
        {
            TimeManager.OnPauseTime.RemoveListener(OnPause);
        }

        protected virtual void FixedUpdate()
        {
            stateMachine?.FixedUpdate();
        }
        
        public void Hit()
        {
            var _hit = stateMachine.hitState;
            _hit.previousState = stateMachine.CurrentState;
            stateMachine.ChangeState(_hit);
        }

        private void OnPause(bool isPaused_)
        {
            if (this.IsEmptyOrDestroyed())
            {
                TimeManager.OnPauseTime.RemoveListener(OnPause);
                return;
            }
            
            if (isPaused_)
            {
                StopAllCoroutines();
                stateMachine?.CurrentState?.Exit();
                stateMachine?.CurrentState?.StopMovement();
            }
            else
            {
                stateMachine?.CurrentState?.ResumeMovement();
                stateMachine?.CurrentState?.Enter();
            }
        }
        
        public void ResetPosition()
        {
            aiPath.Teleport(station.transform.position);
        }
    }
}
