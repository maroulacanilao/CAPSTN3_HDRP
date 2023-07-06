using System;
using UI.Farming;
using UnityEngine;

namespace UI.TabMenu
{
    public class TabGroup : MonoBehaviour
    {
        [SerializeField] private TabButton[] tabButtons;
        [SerializeField] private GameObject[] menus;
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
            for (int i = 0; i < menus.Length; i++)
            {
                if (i == index_)
                {
                    selectedTab = tabButtons[i];
                    selectedTab.Select();
                    menus[i].SetActive(true);
                }
                else
                {
                    menus[i].SetActive(false);
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
