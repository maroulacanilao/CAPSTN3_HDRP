using System;
using Dungeon;
using NaughtyAttributes;
using UnityEngine;

namespace EnemyController.Inheritors
{
    [Serializable]
    public struct TikbalangDisguise
    {
        public RuntimeAnimatorController disguiseAnimator;
        public Vector3 disguiseScale;
        public Vector3 disguiseOffset;
    }
    public class TikbalangController : EnemyAIController
    {
        [field: SerializeField] public TikbalangDisguise[] disguises { get; private set; }
        [field: SerializeField] public TikbalangDisguise defaultDisguise { get; private set; }

        public override void Initialize(EnemyStation station_)
        {
            station = station_;
            stateMachine = new TikbalangStateMachine(this);
            
            stateMachine.Initialize();
        }
        
        public void ChangeDisguise(TikbalangDisguise disguise_)
        {
            if(disguise_.disguiseAnimator == null) return;
            animator.runtimeAnimatorController = disguise_.disguiseAnimator;
            
            animator.ResetTrigger(GroundedHash);
            animator.ResetTrigger(HitHash);
            animator.ResetTrigger(AttackHash);

            var _transform = animator.transform;

            _transform.localScale = disguise_.disguiseScale;
            _transform.localPosition = disguise_.disguiseOffset;
        }
    }
}