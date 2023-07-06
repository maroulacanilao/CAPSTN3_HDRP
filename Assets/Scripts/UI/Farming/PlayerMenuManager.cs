using System.Linq;
using BaseCore;
using CustomEvent;
using CustomHelpers;
using Managers;
using Player;
using Player.ControllerState;
using UI.TabMenu;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Farming
{
    public class PlayerMenuManager : Singleton<PlayerMenuManager>
    {
        [SerializeField] private PlayerMenu[] Menus;
        [SerializeField] private PlayerTabMenu tabGroup;
        
        public static readonly Evt OnCloseAllUI = new Evt();
        public static readonly Evt OnOpenMenu = new Evt();
        
        bool canOpenMenu = true;

        private PlayerInputController mPlayerController;
        private PlayerInputController controller
        {
            get => mPlayerController != null ? mPlayerController : mPlayerController = FindObjectOfType<PlayerInputController>();
        }

        protected override void Awake()
        {
            base.Awake();
            foreach (var _ui in Menus)
            {
                _ui.Initialize();
            }
            SceneManager.activeSceneChanged += OnSceneChanged;
            
            InputUIManager.OnMenu.AddListener(OpenMenu);
            InputUIManager.OnCharacterInfo.AddListener(CharacterInfo);
            InputUIManager.OnInventoryMenu.AddListener(Inventory);
            InputUIManager.OnCodexMenu.AddListener(Codex);
            InputUIManager.OnCancel.AddListener(Settings);
        }

        private void OnDestroy()
        {
            SceneManager.activeSceneChanged -= OnSceneChanged;
            
            InputUIManager.OnMenu.RemoveListener(OpenMenu);
            InputUIManager.OnCharacterInfo.RemoveListener(CharacterInfo);
            InputUIManager.OnInventoryMenu.RemoveListener(Inventory);
            InputUIManager.OnCodexMenu.RemoveListener(Codex);
            InputUIManager.OnCancel.RemoveListener(Settings);
        }
        
        private void OnSceneChanged(Scene arg0_, Scene next_)
        {
            if (this.IsEmptyOrDestroyed())
            {
                SceneManager.activeSceneChanged -= OnSceneChanged;
            }
            
            OnCloseAllUI.Invoke();
            tabGroup.gameObject.SetActive(false);
            
            var _settings = SettingsEnabler.Instance
                .sceneSettingsDictionary
                .TryGetValue(next_.name, out var _settingsData) ? 
                _settingsData : null;

            canOpenMenu = _settings != null && _settings.willEnableController;
        }

        public void OpenMenu()
        {
            if (this.IsEmptyOrDestroyed())
            {
                OnOpenMenu.RemoveListener(this.OpenMenu);
                return;
            }
            
            if (IsMenuOpen())
            {
                OnCloseAllUI.Invoke();
                return;
            }
            
            if(controller.playerState != PlayerSate.Grounded) return;
            
            if(!canOpenMenu) return;

            tabGroup.gameObject.SetActive(true);
            Cursor.visible = true;
        }
        
        bool IsMenuOpen()
        {
            if(this.IsEmptyOrDestroyed()) return false;
            return PlayerMenu.OpenedMenu.IsValid() && PlayerMenu.OpenedMenu.gameObject.activeInHierarchy;
        }

        bool CanOpenMenu()
        {
            return this.IsValid() && canOpenMenu && !IsMenuOpen() && controller.playerState == PlayerSate.Grounded;
        }
        
        private void CharacterInfo()
        {
            if (!CanOpenMenu()) return;
            tabGroup.OpenCharacter();
        }
        
        private void Inventory()
        {
            if (!CanOpenMenu()) return;
            tabGroup.OpenInventory();
        }
        
        private void Codex()
        {
            if (!CanOpenMenu()) return;
            tabGroup.OpenCodex();
        }
        
        private void Settings()
        {
            if (!CanOpenMenu()) return;
            tabGroup.OpenSettings();
        }
    }
}
