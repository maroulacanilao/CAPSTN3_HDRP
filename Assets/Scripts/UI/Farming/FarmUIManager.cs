using System.Linq;
using BaseCore;
using Player;
using UI.TabMenu;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Farming
{
    public class FarmUIManager : Singleton<FarmUIManager>
    {
        [SerializeField] private FarmUI[] farmUIs;
        [SerializeField] private FarmUI[] NonMenuUI;
        [SerializeField] private TabGroup tabGroup;
        [SerializeField] private PlayerInputController playerController;
        public FarmUI lastOpenMenu { get; set; }

        protected override void Awake()
        {
            base.Awake();
            
            foreach (var _ui in farmUIs)
            {
                _ui.Initialize();
            }
            SceneManager.activeSceneChanged += OnSceneChanged;
        }

        private void OnDestroy()
        {
            SceneManager.activeSceneChanged += OnSceneChanged;
        }
        
        private void OnSceneChanged(Scene arg0_, Scene arg1_)
        {
            CloseAllUI();
        }

        public void CloseAllUI()
        {
            if(!IsMenuOpen()) return;
            
            tabGroup.gameObject.SetActive(false);
            
            foreach (var _ui in farmUIs)
            {
                _ui.gameObject.SetActive(false);
            }
            Cursor.visible = false;
            Time.timeScale = 1;
        }
        
        public void OpenMenu()
        {
            Debug.Log("OpenMenu");
            if (IsMenuOpen())
            {
                CloseAllUI();
                return;
            }

            tabGroup.gameObject.SetActive(true);
            Cursor.visible = true;
            Time.timeScale = 0;
        }
        
        bool IsMenuOpen()
        {
            return farmUIs.Any(ui => ui.gameObject.activeSelf);
        }
    }
}
