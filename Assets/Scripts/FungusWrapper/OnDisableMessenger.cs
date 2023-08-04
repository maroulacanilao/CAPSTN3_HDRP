using System.Collections.Generic;
using UnityEngine;

namespace FungusWrapper
{
    [DefaultExecutionOrder(5)]
    public class OnDisableMessenger : MonoBehaviour
    {
        public bool DestroyOnSend;
        public List<string> keys = new List<string>();

        private void OnDisable()
        {
            foreach (var k in keys)
            {
                Fungus.Flowchart.BroadcastFungusMessage(k);
            }

            Debug.Log("SEND MESSAGE");
            if(DestroyOnSend) Destroy(this,0.1f);
        }
    }
}
