using UnityEngine;

namespace FungusWrapper
{
    public class OnDestroyTutorial : MonoBehaviour
    {
        public string tutorialKey;
        
        private void OnDestroy()
        {
            Fungus.Flowchart.BroadcastFungusMessage(tutorialKey);
        }
    }
}
