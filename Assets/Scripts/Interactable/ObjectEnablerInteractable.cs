using BaseCore;
using UnityEngine;

namespace Interactable
{
    public class ObjectEnablerInteractable : InteractableObject
    {
        [SerializeField] private GameObject objectToEnable;
        
        protected override void Interact()
        {
            objectToEnable.SetActive(true);
        }
    
        protected override void Enter()
        {
        
        }
    
        protected override void Exit()
        {
        
        }
    }
}
