using System;
using UI.Farming;
using UnityEngine;

namespace Others
{
    public class MenuDisabler : MonoBehaviour
    {
        private GameObject menu;
        private void OnEnable()
        {
            menu = PlayerMenuManager.Instance.gameObject;
            if (menu != null) menu.SetActive(false);
        }
        
        private void OnDisable()
        {
            if (menu != null) menu.SetActive(true);
        }

        private void Update()
        {
            if(menu != null && menu.activeInHierarchy) menu.SetActive(false);
        }
    }
}