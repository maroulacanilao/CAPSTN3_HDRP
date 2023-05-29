using System;
using System.Collections;
using System.Collections.Generic;
using BaseCore;
using CustomHelpers;
using Managers;
using UnityEngine;

namespace Player
{
    public class InteractDetector : MonoBehaviour
    {
        [SerializeField] private Transform player;
        [SerializeField] private Vector3 offset, size;
        [SerializeField] private LayerMask interactableLayer;
        [SerializeField] private float tickRate = 0.25f;
        
        public InteractableObject nearestInteractable { get; private set; }

        private void OnEnable()
        {
            nearestInteractable = null;
            StartCoroutine(Tick());
        }
        
        private void OnDisable()
        {
            nearestInteractable = null;
            StopCoroutine(Tick());
        }

        private IEnumerator Tick()
        {
            while (enabled)
            {
                GetNearestInteractable();
                yield return new WaitForSeconds(tickRate);
            }
        }

        private void GetNearestInteractable()
        {
            var _pos = player.position;
            Collider[] _colliders = new Collider[3];
            
            int _count = Physics.OverlapBoxNonAlloc(_pos + offset, size, 
                _colliders, Quaternion.identity, interactableLayer);

            if (_count <= 0)
            {
                OnNull();
                return;
            }
            
            var _obj =_colliders.GetNearestGameObject(_pos);

            if (_obj == null)
            {
                OnNull();
                return;
            }

            if(nearestInteractable != null && nearestInteractable.gameObject == _obj) return;

            if (!nearestInteractable.IsUnityValid())
            {
                InteractableObject.OnExit.Invoke(nearestInteractable);
            }
            
            nearestInteractable = _obj.GetComponent<InteractableObject>();
            InteractableObject.OnEnter.Invoke(nearestInteractable);
        }

        private void OnNull()
        {
            if (!nearestInteractable.IsUnityNull())
            {
                InteractableObject.OnExit.Invoke(nearestInteractable);
            }
            nearestInteractable = null;
        }
        
        public void Interact()
        {
            if (nearestInteractable == null) return;
            InteractableObject.OnInteract.Invoke(nearestInteractable);
        }
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position + offset, size);
        }
    }
}