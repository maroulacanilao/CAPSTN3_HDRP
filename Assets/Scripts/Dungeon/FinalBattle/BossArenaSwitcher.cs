using System;
using BattleSystem;
using ScriptableObjectData.CharacterData;
using UnityEngine;

namespace Dungeon.FinalBattle
{
    public class BossArenaSwitcher : MonoBehaviour
    {
        [SerializeField] private EnemyData bossData;
        [SerializeField] private BattleData battleData;
        [SerializeField] private GameObject bossArena;
        [SerializeField] private GameObject regularArena;

        private void Start()
        {
            if(battleData.currentEnemyData == bossData) SwitchToBossArena();
            else SwitchToRegularArena();
        }
        
        private void SwitchToRegularArena()
        {
            regularArena.SetActive(true);
            bossArena.SetActive(false);
        }
        
        private void SwitchToBossArena()
        {
            regularArena.SetActive(false);
            bossArena.SetActive(true);
        }
    }
}
