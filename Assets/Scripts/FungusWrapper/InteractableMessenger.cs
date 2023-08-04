using BaseCore;
using NaughtyAttributes;
using UnityEngine;

namespace FungusWrapper
{
    public class InteractableMessenger : InteractableObject
    {
        [SerializeField] private string message;
        [SerializeField] private bool destroyAfterTrigger = false;
        [SerializeField] [ShowIf("destroyAfterTrigger")] private bool destroyScriptOnly = true;
        
        protected override void Interact()
        {
            Fungus.Flowchart.BroadcastFungusMessage(message);
        }
        
        protected override void Enter()
        {

        }
        
        protected override void Exit()
        {

        }
    }
}
