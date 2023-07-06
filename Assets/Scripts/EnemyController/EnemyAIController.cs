using System;
using System.Collections;
using Character;
using Dungeon;
using EnemyController.EnemyStates;
using Farming;
using NaughtyAttributes;
using Pathfinding;
using UnityEngine;
using UnityEngine.AI;

namespace EnemyController
{
    public class EnemyAIController : MonoBehaviour
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

        [SerializeReference] [ReadOnly] private EnemyStateMachine stateMachine;
        
        public EnemyStation station { get; private set; }
        
        public void Initialize(EnemyStation station_)
        {
            station = station_;
            stateMachine = new EnemyStateMachine(this);
            
            stateMachine.Initialize();
        }

        private void OnEnable()
        {
            stateMachine?.Enable();
        }

        private void FixedUpdate()
        {
            stateMachine.FixedUpdate();
        }
    }
}
