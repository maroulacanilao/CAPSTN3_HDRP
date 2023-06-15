using System;
using UI.Farming;
using UnityEngine;

namespace UI.TabMenu
{
    public class TabGroup : FarmUI
    {
        [SerializeField] private TabButton[] tabButtons;
        [SerializeField] private FarmUI[] farmUIs;
        [SerializeField] private bool canControlWithKeys = true;

        public TabButton selectedTab { get; private set; }
        public int currentIndex => selectedTab.index;

        private void Awake()
        {
            selectedTab = tabButtons[0];

            for (var _index = 0; _index < tabButtons.Length; _index++)
            {
                var _button = tabButtons[_index];
                _button.Initialize(this, _index);
            }
        }

        public void SelectTab(int index_)
        {
            for (int i = 0; i < farmUIs.Length; i++)
            {
                if (i == index_)
                {
                    farmUIs[i].gameObject.SetActive(true);
                    farmUIs[i].OpenMenu();
                    tabButtons[i].Select();
                }
                else
                {
                    farmUIs[i].gameObject.SetActive(false);
                    tabButtons[i].Deselect();
                }
            }
        }

        private void OnEnable()
        {
            if(canControlWithKeys)
            {
                InputUIManager.OnNext.AddListener(Next);
                InputUIManager.OnPrev.AddListener(Previous);
            }
            
            if (selectedTab == null)
            {
                selectedTab = tabButtons[0];
            }
            Debug.Log(selectedTab.gameObject);
            OnSelectNewTab(selectedTab);
        }

        private void OnDisable()
        {
            if(canControlWithKeys)
            {
                InputUIManager.OnNext.RemoveListener(Next);
                InputUIManager.OnPrev.RemoveListener(Previous);
            }
        }
        
        public override void Initialize()
        {
            selectedTab = tabButtons[0];

            for (var _index = 0; _index < tabButtons.Length; _index++)
            {
                var _button = tabButtons[_index];
                _button.Initialize(this, _index);
            }
        }
        public override void OpenMenu()
        {
            
        }

        public void Next()
        {
            var _index = Mathf.Clamp(currentIndex + 1, 0, tabButtons.Length - 1);
            OnSelectNewTab(tabButtons[_index]);
        }
        
        public void Previous()
        {
            var _index = Mathf.Clamp(currentIndex - 1, 0, tabButtons.Length - 1);
            OnSelectNewTab(tabButtons[_index]);
        }
        
        public void OnSelectNewTab(TabButton tabButton_)
        {
            if(tabButton_ == null) return;
            selectedTab = tabButton_;
            selectedTab.OnSelect(null);
            selectedTab.Select();
        }
    }
}
