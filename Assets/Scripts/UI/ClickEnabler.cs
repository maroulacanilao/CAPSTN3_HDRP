using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ClickEnabler : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private GameObject[] objectsToEnable;
        [SerializeField] private GameObject[] objectsToDisable;

        protected void Awake()
        {
            if(button == null) button = GetComponent<Button>();
            button.onClick.AddListener(OnClick);
        }
    
        private void OnClick()
        {
            foreach (var obj in objectsToEnable)
            {
                obj.SetActive(true);
            }
            
            foreach (var obj in objectsToDisable)
            {
                obj.SetActive(false);
            }
        }
    }
}
