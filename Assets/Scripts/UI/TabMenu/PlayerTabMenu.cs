using UI.Farming;
using UnityEngine;

namespace UI.TabMenu
{
    public class PlayerTabMenu : PlayerMenu
    {
        [SerializeField] TabGroup tabGroup;
        
        public void OpenInventory()
        {
            Debug.Log("OpenInventory");
            tabGroup.SelectTab(0);
            tabGroup.gameObject.SetActive(true);
        }
        
        public void OpenCharacter()
        {
            tabGroup.SelectTab(1);
            tabGroup.gameObject.SetActive(true);
        }
        
        public void OpenCodex()
        {
            tabGroup.SelectTab(2);
            tabGroup.gameObject.SetActive(true);
        }
        
        public void OpenSettings()
        {
            tabGroup.SelectTab(3);
            tabGroup.gameObject.SetActive(true);
        }

        public void CloseAll()
        {
            PlayerMenuManager.OnCloseAllUI.Invoke();
        }
    }
}
