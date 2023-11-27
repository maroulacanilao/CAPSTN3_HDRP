using BaseCore;
using CustomEvent;
using CustomHelpers;
using FungusWrapper;
using Player;
using Player.ControllerState;
using UI.TabMenu;
using UI.TabMenu.InventoryMenu;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Farming
{
    public class RevisedPlayerMenuManager : Singleton<RevisedPlayerMenuManager>
    {
        [SerializeField] private PlayerMenu[] Menus;
        [SerializeField] private PlayerTabMenu tabGroup;
        [SerializeField] private InventoryMenu inventoryMenu;

        public static readonly Evt OnCloseAllUIRevised = new Evt();
        public static readonly Evt OnOpenMenu = new Evt();

        bool canOpenMenu = true;

        private PlayerInputController mPlayerController;
        private PlayerInputController controller
        {
            get => mPlayerController != null ? mPlayerController : mPlayerController = FindObjectOfType<PlayerInputController>();
        }

        protected void Start()
        {
            foreach (var _ui in Menus)
            {
                _ui.Initialize();
            }
            SceneManager.activeSceneChanged += OnSceneChanged;

            InputUIManager.OnMenu.AddListener(OpenMenu);
            InputUIManager.OnInventoryMenu.AddListener(Inventory);

            InputUIManager.OnCharacterInfo.AddListener(ProfileInfo);
            InputUIManager.OnPartyMenu.AddListener(Party);
            InputUIManager.OnCropsMenu.AddListener(Crops);
            InputUIManager.OnFishesMenu.AddListener(Fishes);
            InputUIManager.OnMonstersMenu.AddListener(Monsters);
            InputUIManager.OnCancel.AddListener(Settings);
        }

        private void OnDestroy()
        {
            SceneManager.activeSceneChanged -= OnSceneChanged;

            InputUIManager.OnMenu.RemoveListener(OpenMenu);
            InputUIManager.OnInventoryMenu.RemoveListener(Inventory);

            InputUIManager.OnCharacterInfo.RemoveListener(ProfileInfo);
            InputUIManager.OnPartyMenu.RemoveListener(Party);
            InputUIManager.OnCropsMenu.RemoveListener(Crops);
            InputUIManager.OnFishesMenu.RemoveListener(Fishes);
            InputUIManager.OnMonstersMenu.RemoveListener(Monsters);
            InputUIManager.OnCancel.RemoveListener(Settings);
        }

        private void OnSceneChanged(Scene arg0_, Scene next_)
        {
            if (this.IsEmptyOrDestroyed())
            {
                SceneManager.activeSceneChanged -= OnSceneChanged;
            }

            OnCloseAllUIRevised.Invoke();
            tabGroup.gameObject.SetActive(false);

            inventoryMenu.gameObject.SetActive(false);

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
                OnCloseAllUIRevised.Invoke();
                return;
            }

            if (controller.playerState != PlayerSate.Grounded) return;

            if (!CanOpenMenu()) return;

            tabGroup.gameObject.SetActive(true);

            Cursor.visible = true;
        }

        bool IsMenuOpen()
        {
            if (this.IsEmptyOrDestroyed()) return false;
            return PlayerMenu.OpenedMenu.IsValid() && PlayerMenu.OpenedMenu.gameObject.activeInHierarchy;
        }

        bool CanOpenMenu()
        {
            return this.IsValid() &&
                   canOpenMenu &&
                   !IsMenuOpen() &&
                   controller.playerState == PlayerSate.Grounded &&
                   !FungusFlowchartSetter.IsExecuting() &&
                   !FungusSetter.IsOpen();
        }

        private void Inventory()
        {
            if (IsMenuOpen())
            {
                OnCloseAllUIRevised.Invoke();
                return;
            }

            if (controller.playerState != PlayerSate.Grounded) return;

            if (!CanOpenMenu()) return;

            inventoryMenu.gameObject.SetActive(true);

            Cursor.visible = true;
        }

        private void ProfileInfo()
        {
            if (!CanOpenMenu()) return;
            tabGroup.OpenProfileTab();
        }

        private void Party()
        {
            if (!CanOpenMenu()) return;
            tabGroup.OpenPartyTab();
        }

        private void Crops()
        {
            if (!CanOpenMenu()) return;
            tabGroup.OpenCropsTab();
        }

        private void Fishes()
        {
            if (!CanOpenMenu()) return;
            tabGroup.OpenFishesTab();
        }

        private void Monsters()
        {
            if (!CanOpenMenu()) return;
            tabGroup.OpenMonstersTab();
        }

        private void Settings()
        {
            if (IsMenuOpen())
            {
                OnCloseAllUIRevised.Invoke();
                return;
            }

            if (!CanOpenMenu()) return;
            tabGroup.OpenNewSettingsTab();
        }
    }
}