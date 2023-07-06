using System;
using CustomHelpers;
using UI.Farming;
using UnityEngine;

namespace UI.ShrineUI
{
    public class ShrineMenuManager : PlayerMenu
    {
        [SerializeField] private ShrineMenu[] menuList;

        private void Awake()
        {
            foreach (var menu in menuList)
            {
                menu.Initialize();
            }
        }
        
        protected override void OnEnable()
        {
            base.OnEnable();
            CloseAllMenu();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
        }

        protected void CloseAllMenu()
        {
            foreach (var menu in menuList)
            {
                menu.gameObject.SetActive(false);
            }
        }

        public void Close()
        {
            CloseAllMenu();
            CloseMenu();
        }
    }
}
