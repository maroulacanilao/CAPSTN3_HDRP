using System;
using System.Collections.Generic;
using Managers.SceneLoader;
using ScriptableObjectData;
using TMPro;
using UI.Farming;
using UnityEngine;
using UnityEngine.UI;

namespace UI.DungeonEntranceUI
{
    public class DungeonEntranceMenu : PlayerMenu
    {
        [SerializeField] private ProgressionData progressionData;
        [SerializeField] private SessionData sessionData;
        [SerializeField] private TMP_Dropdown dropdown;
        [SerializeField] private Button enterBtn, exitBtn;
        [SerializeField] [NaughtyAttributes.Scene] private string dungeonSceneName;
        
        private void Awake()
        {
            enterBtn.onClick.AddListener(OnEnterBtnClicked);
            exitBtn.onClick.AddListener(OnExitBtnClicked);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            CreateDropdownOptions();
            Time.timeScale = 0;
            Cursor.visible = true;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            Time.timeScale = 1;
        }

        private void OnEnterBtnClicked()
        {
            sessionData.dungeonLevel = dropdown.value + 1;
            gameObject.SetActive(false);

            var _sceneParameter = new LoadSceneParameters(LoadSceneType.LoadAdditive, dungeonSceneName, dungeonSceneName);
            SceneLoader.OnLoadScene.Invoke(_sceneParameter);
        }
        
        private void OnExitBtnClicked()
        {
            gameObject.SetActive(false);
        }
        
        private void CreateDropdownOptions()
        {
            dropdown.ClearOptions();
            var _options = new List<TMP_Dropdown.OptionData>();
            var _count = Mathf.Clamp(progressionData.highestDungeonLevel, 1 , 99);
            Debug.Log(_count);
            for (var i = 0; i < _count ; i++)
            {
                var _option = new TMP_Dropdown.OptionData($"Floor {i + 1}");
                _options.Add(_option);
            }
            dropdown.options = _options;
            dropdown.value = dropdown.options.Count - 1;
        }
    }
}