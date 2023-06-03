using System;
using System.Collections;
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

        #endregion

        #region Attack And Movement properties

        [field: BoxGroup("Movement")] [field: SerializeField] 
        public float movementSpeed { get; private set; }
        
        [field: BoxGroup("Movement")] [field: SerializeField] 
        public float chaseSpeed { get; private set; }
        
        [field: BoxGroup("Attack Properties")] [field: SerializeField]
        public float attackRange { get; private set; }
        
        [field: BoxGroup("Attack Properties")] [field: SerializeField]
        public float attackCooldown { get; private set; }

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

        #endregion
        
        #region Animation Events
        
        [field: BoxGroup("Animation Event")] [field: SerializeField]
        public string AttackHitEvent { get; private set; }
        
        [field: BoxGroup("Animation Event")] [field: SerializeField]
        public string AnimationEndEvent { get; private set; }

        #endregion

        [SerializeReference] [ReadOnly] private EnemyStateMachine stateMachine;

        private void Awake()
        {
            stateMachine = new EnemyStateMachine(this);
        }
        
        private void Start()
        {
            
        }

        private void FixedUpdate()
        {
            animator.SetFloat(xSpeedHash, aiPath.velocity.magnitude);
        }

        public IEnumerator RefreshDestination(Transform target_, float refreshRate_ = 1f)
        {
            var _waiter = new WaitForSeconds(refreshRate_);
            while (gameObject.activeSelf)
            {
                if(target_ == null) yield break;
                aiPath.destination = target_.position;
                aiPath.OnTargetReached();
                yield return _waiter;
            }
        }

        public IEnumerator WaitUntilReachDestination(float refreshRate_ = 1f)
        {
            yield return null;
            var _waiter = new WaitForSeconds(refreshRate_);
            
            while (gameObject.activeSelf)
            {
                yield return _waiter;
                if(aiPath.reachedDestination) yield break;
            }
        }

        public void SetTileTarget(FarmTile tile_)
        {
            if (stateMachine == null)
            {
                stateMachine = new EnemyStateMachine(this);
            }
            StopAllCoroutines();
            stateMachine.ChangeState(new EnemyGoToTileState(this, stateMachine, tile_));
        }
    }
}
