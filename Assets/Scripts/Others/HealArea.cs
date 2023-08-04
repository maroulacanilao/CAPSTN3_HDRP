using System;
using System.Collections;
using BaseCore;
using CustomHelpers;
using ScriptableObjectData.CharacterData;
using UnityEngine;

namespace Others
{
    public class HealArea : MonoBehaviour
    {
        
        [SerializeField] private PlayerData playerData;
        [SerializeField] private int healPerTick = 1;
        [SerializeField] private float tickDuration = 0.1f;
        [SerializeField] private string vfxName = "HealAura";
        
        private bool IsPlayerInside => playerTransform != null;
        private Transform playerTransform;

        private void OnEnable()
        {
            playerTransform = null;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            playerTransform = other.transform;
            StartCoroutine(CoHeal());
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            playerTransform = null;
            StopAllCoroutines();
        }

        private IEnumerator CoHeal()
        {
            var _wait = new WaitForSeconds(tickDuration);
            var _healInfo = new HealInfo(healPerTick, gameObject);
            var _health = playerData.health;
            var _ticks = 0;

            while (IsPlayerInside)
            {
                if (_ticks == 0)
                {
                    if(playerTransform == null) yield break;
                    AssetHelper.PlayEffectCoroutine(vfxName, playerTransform, Vector3.zero, Quaternion.Euler(15, 0, 0), 1f);
                }
                _health.ReceiveHeal(_healInfo);
                _ticks++;
                yield return _wait;
                if(_ticks <= 10) _ticks = 0;
            }
        }
    }
}
