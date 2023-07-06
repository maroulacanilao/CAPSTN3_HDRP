using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        public string interactText
        {
            get
            {
                if(nearestInteractable == null) return string.Empty;
                return !nearestInteractable.canInteract ? string.Empty : nearestInteractable.interactText;
            }
        }
        public Vector3 boxPos => transform.position + offset;
        public Vector3 Size => size;

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
            Collider[] _colliders = new Collider[3];
            
            int _count = Physics.OverlapBoxNonAlloc(boxPos, size, 
                _colliders, Quaternion.identity, interactableLayer);

            if (_count <= 0)
            {
                OnNull();
                return;
            }
            
            // var _validObjects = _colliders.Where(x =>
            // {
            //     if(x.IsEmptyOrDestroyed()) return false;
            //     return x.TryGetComponent(out InteractableObject _interactable) && _interactable.canInteract;
            // }).ToArray();
            //
            // if (_validObjects.Length <= 0)
            // {
            //     OnNull();
            //     return;
            // }
            
            var _obj = _colliders.GetNearestGameObject(transform.position);

            if (_obj == null)
            {
                OnNull();
                return;
            }

            if(nearestInteractable != null && nearestInteractable.gameObject == _obj) return;

            if (nearestInteractable.IsValid())
            {
                InteractableObject.OnExit.Invoke(nearestInteractable);
            }
            
            nearestInteractable = _obj.GetComponent<InteractableObject>();
            InteractableObject.OnEnter.Invoke(nearestInteractable);
        }

        private void OnNull()
        {
            if (nearestInteractable.IsValid())
            {
                InteractableObject.OnExit.Invoke(nearestInteractable);
            }
            nearestInteractable = null;
        }
        
        public void Interact()
        {
            if (!CanInteract()) return;
            InteractableObject.OnInteract.Invoke(nearestInteractable);
        }
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position + offset, size);
        }

        public bool CanInteract()
        {
            return nearestInteractable != null && nearestInteractable.canInteract;
        }
        
    }
}