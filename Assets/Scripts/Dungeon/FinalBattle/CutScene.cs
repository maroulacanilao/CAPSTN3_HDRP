using EnemyController.Inheritors;
using Managers;
using UnityEngine;

namespace Dungeon.FinalBattle
{
    public class CutScene : MonoBehaviour
    {
        [SerializeField] private SidapaController sidapaController;
        
        public void StartBattle()
        {
            GameManager.OnEnterBattle.Invoke(sidapaController.enemyCharacter,true);
        }
    }
}
