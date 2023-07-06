using System;
using BaseCore;
using CustomHelpers;
using ScriptableObjectData;
using Settings;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    [DefaultExecutionOrder(-1)]
    public class AudioManager : SingletonPersistent<AudioManager>
    {
        [SerializeField] private AudioSource musicSource, sfxSource;

        protected override void Awake()
        {
            base.Awake();

            SceneManager.activeSceneChanged += OnSceneChanged;
            SettingsUtil.OnValuesChanged.AddListener(OnSettingsChanged);
            
            OnSceneChanged(SceneManager.GetActiveScene(), SceneManager.GetActiveScene());
            OnSettingsChanged();
        }

        private void OnDestroy()
        {
            SceneManager.activeSceneChanged -= OnSceneChanged;
            SettingsUtil.OnValuesChanged.RemoveListener(OnSettingsChanged);
        }
        
        private void OnSceneChanged(Scene current_, Scene next_)
        {
            PlaySceneMusic(next_.name);
        }
        
        public static void PlaySceneMusic(string sceneName_)
        {
            if(Instance.IsEmptyOrDestroyed()) return;

            var _clip = AssetHelper.GetMusicForScene(sceneName_);
            
            if (_clip == null)
            {
                Instance.musicSource.Stop();
                Instance.musicSource.clip = null;
                return;
            }
            Instance.musicSource.clip = _clip;
            Instance.musicSource.Play();
        }
        
        public static void StopSceneMusic()
        {
            if(Instance.IsEmptyOrDestroyed()) return;
            Instance.musicSource.Stop();
        }
        
        private void OnSettingsChanged()
        {
            musicSource.volume = SettingsUtil.GetMusicVolume();
            sfxSource.volume = SettingsUtil.GetSfxVolume();
        }
        
        public static void PlaySfx(string audioID_)
        {
            if(Instance.IsEmptyOrDestroyed()) return;
            
            var _clip = AssetHelper.GetSfx(audioID_);
            if(_clip == null) return;
            Instance.sfxSource.PlayOneShot(_clip);
        }
        
        public static void StopSfx()
        {
            if(Instance.IsEmptyOrDestroyed()) return;
            Instance.sfxSource.Stop();
        }
    }
}