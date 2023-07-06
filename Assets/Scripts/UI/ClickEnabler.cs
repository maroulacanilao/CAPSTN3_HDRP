using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ClickEnabler : SelectableMenuButton
    {
        [SerializeField] private GameObject[] objectsToEnable;
        [SerializeField] private GameObject[] objectsToDisable;

        private void Awake()
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
