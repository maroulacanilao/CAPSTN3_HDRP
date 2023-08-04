using System;
using Dungeon;
using NaughtyAttributes;
using UnityEngine;

namespace EnemyController.Inheritors
{
    public class TiyanakController : EnemyAIController
    {
        [field: BoxGroup("Animation Parameter")] [field: AnimatorParam("animator")] [field: SerializeField]
        public int idleDisguiseHash { get; private set; }


        public override void Initialize(EnemyStation station_)
        {
            base.Initialize(station_);
            var _disguiseState = new TiyanakDisguiseState(this, stateMachine);
            stateMachine.ChangeState(_disguiseState);
        }
        
        protected override void OnEnable()
        {
            animator.ResetTrigger(idleDisguiseHash);
            animator.ResetTrigger(GroundedHash);
            if (stateMachine == null || station == null) return;
            
            var _disguiseState = new TiyanakDisguiseState(this, stateMachine);
            stateMachine?.ChangeState(_disguiseState);
        }

        private void OnDisable()
        {
            animator.ResetTrigger(idleDisguiseHash);
            animator.ResetTrigger(GroundedHash);
        }
    }
}
