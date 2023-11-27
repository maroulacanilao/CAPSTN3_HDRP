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
            RevisedPlayerMenuManager.OnCloseAllUIRevised.Invoke();
        }

        #region NewTabMenus
        public void OpenProfileTab()
        {
            tabGroup.SelectTab(0);
            tabGroup.gameObject.SetActive(true);
        }

        public void OpenPartyTab()
        {
            tabGroup.SelectTab(1);
            tabGroup.gameObject.SetActive(true);
        }

        public void OpenCropsTab()
        {
            tabGroup.SelectTab(2);
            tabGroup.gameObject.SetActive(true);
        }

        public void OpenFishesTab()
        {
            tabGroup.SelectTab(3);
            tabGroup.gameObject.SetActive(true);
        }

        public void OpenMonstersTab()
        {
            tabGroup.SelectTab(4);
            tabGroup.gameObject.SetActive(true);
        }

        public void OpenNewSettingsTab()
        {
            tabGroup.SelectTab(5);
            tabGroup.gameObject.SetActive(true);
        }
        #endregion

    }
}
