using System;
using NaughtyAttributes;
using UnityEngine;

namespace EnemyController.Inheritors
{
    public class SidapaController : EnemyAIController
    {
        [field: BoxGroup("Animation Parameter")] [field: AnimatorParam("animator")] [field: SerializeField]
        public int DialogHash { get; private set; }

        [SerializeField] private int SidapaLevel; 

        private void Start()
        {
            enemyCharacter.SetLevel(SidapaLevel);
        }

        public void DialogAnim()
        {
            animator.SetTrigger(DialogHash);
        }
    }
}
