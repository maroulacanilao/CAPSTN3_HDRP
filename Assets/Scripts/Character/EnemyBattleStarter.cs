using System;
using CustomHelpers;
using Managers;
using UnityEngine;

namespace Character
{
    public class EnemyBattleStarter : MonoBehaviour
    {
        [SerializeField] private EnemyCharacter enemyCharacter;
        private Collider col;

        private void Awake()
        {
            if(enemyCharacter.IsUnityNull()) enemyCharacter = GetComponentInParent<EnemyCharacter>();
            col = GetComponent<Collider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log(other.gameObject.name);
            if (!other.gameObject.CompareTag("Player")) return;
            if(!other.gameObject.TryGetComponent(out PlayerCharacter playerCharacter)) return;
            
            col.enabled = false;
            GameManager.OnEnterBattle.Invoke(enemyCharacter);
        }
    }
}
