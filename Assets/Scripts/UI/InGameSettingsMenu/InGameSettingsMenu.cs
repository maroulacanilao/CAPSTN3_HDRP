using System;
using Managers;
using Managers.SceneLoader;
using NaughtyAttributes;
using ScriptableObjectData;
using UI.Farming;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using LoadSceneParameters = Managers.SceneLoader.LoadSceneParameters;

namespace UI.InGameSettingsMenu
{
    public class InGameSettingsMenu : MonoBehaviour
    {
        [SerializeField] private GameDataBase gameDataBase;
        [SerializeField] private Button settingsBtn, exitDungeonBtn, saveBtn, exitToMainMenu, exitGameBtn;

        [BoxGroup("Settings Panel")]
        [SerializeField] private GameObject audioPanel, cheatMenu;

        public void Awake()
        {
            SceneManager.activeSceneChanged += OnSceneChanged;
            settingsBtn.onClick.AddListener(SettingsClick);
            saveBtn.onClick.AddListener(SaveGameClick);
            exitDungeonBtn.onClick.AddListener(ExitDungeonClick);
            exitToMainMenu.onClick.AddListener(ExitToMainMenuClick);
            exitGameBtn.onClick.AddListener(ExitGameClick);
            
            OnSceneChanged(SceneManager.GetActiveScene(), SceneManager.GetActiveScene());
        }

        public void OnDestroy()
        {
            SceneManager.activeSceneChanged -= OnSceneChanged;
        }
        
        private void OnSceneChanged(Scene current, Scene next)
        {
            exitDungeonBtn.gameObject.SetActive(next.name == gameDataBase.DungeonSceneName);
            saveBtn.gameObject.SetActive(next.name == gameDataBase.FarmSceneName);
        }

        private void OnEnable()
        {
            audioPanel.SetActive(false);
            cheatMenu.SetActive(false);
            EventSystem.current.SetSelectedGameObject(saveBtn.gameObject);
        }

        #region Buttons
        
        private void SettingsClick()
        {
            cheatMenu.SetActive(false);
            audioPanel.SetActive(true);
        }

        public void CheatsClick()
        {
            audioPanel.SetActive(false);
            cheatMenu.SetActive(true);
        }

        private void SaveGameClick()
        {
            gameDataBase.progressionData.SaveProgression();
        }
        
        private void ExitGameClick()
        {
            Application.Quit();
        }
        
        private void ExitToMainMenuClick()
        {
            var _sceneParam = new LoadSceneParameters(LoadSceneType.LoadSingle, gameDataBase.MainMenuSceneName, gameDataBase.MainMenuSceneName);
            SceneLoader.OnLoadScene.Invoke(_sceneParam);
        }
        
        private void ExitDungeonClick()
        {
            gameDataBase.sessionData.farmLoadType = FarmLoadType.DungeonEntrance;
            var _sceneParam = new LoadSceneParameters(LoadSceneType.UnloadAllExcept, gameDataBase.FarmSceneName, gameDataBase.FarmSceneName);
            SceneLoader.OnLoadScene.Invoke(_sceneParam);
        }
        
        #endregion
    }
}