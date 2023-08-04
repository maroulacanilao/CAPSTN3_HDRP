using System.Collections;
using System.Collections.Generic;
using CustomHelpers;
using NaughtyAttributes;
using UnityEngine;

namespace FungusWrapper
{
    [DefaultExecutionOrder(5)]
    public class OnEnabledMessenger : MonoBehaviour
    {
        public bool DestroyOnSend;
        [BoxGroup("Messages")]public List<string> keys = new List<string>();
        [BoxGroup("Delay")]public bool DelayedSend = false;
        [BoxGroup("Delay")] public float seconds = 0f;
        [BoxGroup("Delay")] public int frames = 1;

        private void OnEnable()
        {
            if (DelayedSend)
            {
                SendDelayed().StartCoroutine();
            }
            else
            {
                SendMessage();
            }
        }

        public void SendMessage()
        {
            foreach (var k in keys)
            {
                Fungus.Flowchart.BroadcastFungusMessage(k);
            }
            
            if(DestroyOnSend) Destroy(this,0.1f);
        }
        
        IEnumerator SendDelayed()
        {
            yield return new WaitForSeconds(seconds);
            SendMessage();
        }
    }
}
