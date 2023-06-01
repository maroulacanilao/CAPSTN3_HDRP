using System.Linq;
using BaseCore;
using Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI
{
    public class FarmUIManager : Singleton<FarmUIManager>
    {
        [SerializeField] private FarmUI[] farmUIs;
        [SerializeField] private FarmUI[] NonMenuUI;
        
        public FarmUI lastOpenMenu { get; set; }

        protected override void Awake()
        {
            base.Awake();
            
            foreach (var _ui in farmUIs)
            {
                _ui.Initialize();
            }
        }

        public void CloseAllUI()
        {
            if(!IsMenuOpen()) return;
            foreach (var _ui in farmUIs)
            {
                _ui.gameObject.SetActive(false);
            }
            Cursor.visible = false;
            Time.timeScale = 1;
        }

        public void OpenMenu()
        {
            if (IsMenuOpen())
            {
                CloseAllUI();
                return;
            }

            var _menu = lastOpenMenu != null ? lastOpenMenu : farmUIs[0]; 
            _menu.gameObject.SetActive(true);
            _menu.OpenMenu();
            Cursor.visible = true;
            Time.timeScale = 0;
        }
        
        bool IsMenuOpen()
        {
            return farmUIs.Any(ui => ui.gameObject.activeSelf);
        }
    }
}
