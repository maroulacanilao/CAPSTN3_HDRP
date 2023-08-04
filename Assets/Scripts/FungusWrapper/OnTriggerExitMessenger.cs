using NaughtyAttributes;
using UnityEngine;

namespace FungusWrapper
{
    public class OnTriggerExitMessenger : MonoBehaviour
    {
        [NaughtyAttributes.Tag] public string Tag;
        public string key;
        public bool destroyAfterTrigger = true;
        [ShowIf("destroyAfterTrigger")] public bool destroyScriptOnly = true;
        
        
        private void OnTriggerExit(Collider other)
        {
            if(!other.CompareTag(Tag)) return;
            Fungus.Flowchart.BroadcastFungusMessage(key);
            
            if (!destroyAfterTrigger) return;
            
            if(destroyScriptOnly) Destroy(this);
            else Destroy(gameObject);
        }
    }
}