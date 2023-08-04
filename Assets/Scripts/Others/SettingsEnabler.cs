using System;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using BaseCore;
using CustomHelpers;
using Managers;
using NaughtyAttributes;
using Player;
using ScriptableObjectData;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-9)]
public class SettingsEnabler : Singleton<SettingsEnabler>
{
    [field: SerializeField] public SerializedDictionary<string, SceneSettingsData> sceneSettingsDictionary { get; private set; }
    [SerializeField] private SceneSettingsData defaultSettings;
    [SerializeField] private SessionData sessionData;

    public static SceneSettingsData currSettings
    {
        get
        {
            var _currScene = SceneManager.GetActiveScene();
            return Instance.sceneSettingsDictionary.TryGetValue(_currScene.name, out var _value) ? _value : Instance.defaultSettings;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        SceneManager.activeSceneChanged += OnSceneChanged;
        var _currScene = SceneManager.GetActiveScene();
        OnSceneChanged(_currScene, _currScene);
    }

    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= OnSceneChanged;
    }
    
    private void OnSceneChanged(Scene current_, Scene next_)
    {
        var _settings = sceneSettingsDictionary.TryGetValue(next_.name, out var _value) ? _value : defaultSettings;

        if (TimeManager.IsInstanceValid() && _settings.willChangeTime)
        {
            var _willAffectTimeScale = _settings.willAffectTimeScale;
            if(_settings.willPause) TimeManager.PauseTime(_willAffectTimeScale);
            else TimeManager.ResumeTime(_willAffectTimeScale);

            if (_settings.canStartTime && !sessionData.hasStartedTime)
            {
                TimeManager.StartTime();
                sessionData.hasStartedTime = true;
            }
        }
        
        if(PlayerInputController.Instance.IsValid())
        {
            var _player = PlayerInputController.Instance;
            _player.gameObject.SetActive(_settings.willEnablePlayer);
            _player.SetCanUseFarmTools(_settings.willEnablePlayer && _settings.canUseTool);
            _player.lanternLight.gameObject.SetActive(_settings.willEnablePlayer && _settings.willEnableLantern);
        }
        
        if (InputManager.IsInstanceValid())
        {
            InputManager.Instance.gameObject.SetActive(_settings.willEnableController);
        }
        
        Cursor.visible = _settings.willEnableCursor;
    }
    
    
    [Button("Get All Scene Settings")]
    private void GetAllSceneSettings()
    {
        var _sceneSettings = Resources.LoadAll<SceneSettingsData>("Data/SceneSettings");
        sceneSettingsDictionary.Clear();
        
        foreach (var _setting in _sceneSettings)
        {
            if(defaultSettings == _setting) continue;
            if(sceneSettingsDictionary.ContainsKey(_setting.sceneName)) continue;
            sceneSettingsDictionary.Add(_setting.sceneName, _setting);
        }
    }
}
