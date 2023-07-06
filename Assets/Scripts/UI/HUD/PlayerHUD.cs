using System;
using ScriptableObjectData;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.HUD
{
    public class PlayerHUD : MonoBehaviour
    {
        [SerializeField] private EquipmentActionUI equipmentActionUI;
        [SerializeField] private Toolbar.Toolbar toolbar;
        
        private void Awake()
        {
            SceneManager.activeSceneChanged += OnSceneChanged;
            ApplySettings(SettingsEnabler.currSettings);
        }

        private void OnDestroy()
        {
            SceneManager.activeSceneChanged -= OnSceneChanged;
        }
        
        private void OnSceneChanged(Scene current_, Scene next_)
        {
            SettingsEnabler.Instance.sceneSettingsDictionary.TryGetValue(next_.name, out var _settings);
            if (_settings == null) return;
            
            ApplySettings(_settings);
        }
        
        private void ApplySettings(SceneSettingsData settings_)
        {
            if (!settings_.willEnablePlayerHUD)
            {
                gameObject.SetActive(false);
                return;
            }
            
            gameObject.SetActive(true);
            
            var _canUseTool = settings_.willEnablePlayer && settings_.canUseTool && settings_.willEnablePlayerHUD;
            
            toolbar.gameObject.SetActive(_canUseTool);
            equipmentActionUI.SetCanUseTool(_canUseTool);
        }
    }
}
