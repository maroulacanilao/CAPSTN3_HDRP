using System;
using BaseCore;
using UnityEngine;

namespace Interactable
{
    public class TorchInteractable : InteractableObject
    {
        [SerializeField] private GameObject lightObject, fireObject;
        [SerializeField] private bool isLit = false;

        private void Start()
        {
            lightObject.SetActive(isLit);
            fireObject.SetActive(isLit);
            interactText = isLit ? "Extinguish" : "Light Torch";
        }
        
        protected override void Interact()
        {
            isLit = !isLit;
            lightObject.SetActive(isLit);
            fireObject.SetActive(isLit);
            
            interactText = isLit ? "Extinguish" : "Light Torch";
        }
        
        protected override void Enter()
        {
            
        }
        protected override void Exit()
        {
            
        }
    }
}
