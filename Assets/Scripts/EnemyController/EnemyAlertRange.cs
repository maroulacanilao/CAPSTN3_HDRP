using System;
using System.Collections;
using CustomEvent;
using NaughtyAttributes;
using UnityEngine;

namespace EnemyController
{
    public class EnemyAlertRange : MonoBehaviour
    {
        [SerializeField] [Tag] private string targetTag = "Player";
        [SerializeField] private float checkInterval = 0.2f;
        
        public readonly Evt<Transform> OnPlayerNearby = new Evt<Transform>();

        private Transform player;

        private void OnEnable()
        {
            player = null;
            // StartCoroutine(OnPlayerNearbyRoutine());
        }

        private void OnDisable()
        {
            // StopAllCoroutines();
        }

        private void FixedUpdate()
        {
            if (!IsPlayerNearby()) return;
            OnPlayerNearby.Invoke(player);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(targetTag)) return;
            player = other.transform;
            OnPlayerNearby.Invoke(player);
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag(targetTag)) return;
            player = null;
        }
        
        private IEnumerator OnPlayerNearbyRoutine()
        {
            var _wait = new WaitForSeconds(checkInterval);
            
            while (gameObject.activeInHierarchy)
            {
                if(IsPlayerNearby()) OnPlayerNearby.Invoke(player);
                yield return _wait;
            }
        }
        
        public bool IsPlayerNearby()
        {
            if(player == null) return false;
            
            var _verticalDistance = Mathf.Abs(transform.parent.position.y - player.position.y);
            return _verticalDistance < 1.5f;
        }
    }
}
