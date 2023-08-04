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
            menu.SetActive(false);
        }
        
        private void OnDisable()
        {
            menu.SetActive(true);
        }

        private void Update()
        {
            if(menu.activeInHierarchy) menu.SetActive(false);
        }
    }
}