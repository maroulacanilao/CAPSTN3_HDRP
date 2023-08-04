using System;
using System.Collections;
using AYellowpaper.SerializedCollections;
using BaseCore;
using CustomHelpers;
using DG.Tweening;
using ScriptableObjectData;
using Settings;
using UnityEngine;
using UnityEngine.SceneManagement;
using LoadSceneParameters = Managers.SceneLoader.LoadSceneParameters;

namespace Managers
{
    [DefaultExecutionOrder(-1)]
    public class AudioManager : SingletonPersistent<AudioManager>
    {
        [SerializeField] private AudioSource musicSource, sfxSource;
        private AudioClip hitSfx, plowSfx, wateringSfx, harvestingSfx, jumpSfx, plantSfx, missSfx;
        
        SerializedDictionary<AudioClip,float> musicDuration = new SerializedDictionary<AudioClip, float>();

        protected override void Awake()
        {
            base.Awake();
            
            hitSfx = AssetHelper.GetSfx("Hit2");
            plowSfx = AssetHelper.GetSfx("Plowing");
            wateringSfx = AssetHelper.GetSfx("Watering");
            harvestingSfx = AssetHelper.GetSfx("Harvesting");
            jumpSfx = AssetHelper.GetSfx("Jump");
            plantSfx = AssetHelper.GetSfx("Planting");
            missSfx = AssetHelper.GetSfx("Miss");

            SceneManager.activeSceneChanged += OnSceneChanged;
            SettingsUtil.OnValuesChanged.AddListener(OnSettingsChanged);
            SceneLoader.SceneLoader.OnLoadScene.AddListener(OnSceneLoaded);
            
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
            var _lastMusic = AssetHelper.GetMusicForScene(current_.name);
            var _nextMusic = AssetHelper.GetMusicForScene(next_.name);
            if(_lastMusic == null && _nextMusic == null) return;
            if(_lastMusic == _nextMusic) return;
            
            SaveMusicDuration(_lastMusic);

            PlaySceneMusic(next_.name); 
            SceneLoader.SceneLoader.OnLoadScene.RemoveListener(OnSceneLoaded);
        }
        
        private void OnSceneLoaded(LoadSceneParameters obj_)
        {
            Instance.musicSource.DOFade(0, 1f);
            Instance.musicSource.clip = null;
            Instance.musicSource.Stop();
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
            Instance.musicSource.volume = 0;
            Instance.musicSource.clip = _clip;
            Instance.musicSource.Play();
            Instance.LoadMusicDuration(_clip);
            Instance.musicSource.DOFade(SettingsUtil.GetMusicVolume(), 1f);
        }
        
        private void SaveMusicDuration(AudioClip clip_)
        {
            if(clip_ == null) return;
            if (musicDuration.ContainsKey(clip_))
            {
                var _duration = musicSource.time;
                musicDuration[clip_] = Mathf.Clamp(_duration, 0, clip_.length);
            }
            else
            {
                musicDuration.Add(clip_, musicSource.time);
            }
        }
        
        private void LoadMusicDuration(AudioClip clip_)
        {
            if(clip_ == null) return;
            if (musicDuration.TryGetValue(clip_, out var _duration))
            {
                musicSource.time = Mathf.Clamp(_duration, 0, clip_.length);
            }
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
        
        public static void PlaySfx(AudioClip audioClip_)
        {
            if(Instance.IsEmptyOrDestroyed()) return;
            Instance.sfxSource.PlayOneShot(audioClip_);
        }
        
        public static void PlayMissSfx()
        {
            PlaySfx(Instance.missSfx);
        }
        
        public static void StopSfx()
        {
            if(Instance.IsEmptyOrDestroyed()) return;
            Instance.sfxSource.Stop();
        }

        #region PlayCommonSFX

        public static void PlayHit()
        {
            PlaySfx(Instance.hitSfx);
        }
        
        public static void PlayPlow()
        {
            PlaySfx(Instance.plowSfx);
        }

        public static void PlayWatering()
        {
            PlaySfx(Instance.wateringSfx);
        }

        public static void PlayHarvesting()
        {
            PlaySfx(Instance.harvestingSfx);
        }

        public static void PlayJump()
        {
            PlaySfx(Instance.jumpSfx);
        }
        
        public static void PlayPlanting()
        {
            PlaySfx(Instance.plantSfx);
        }

        #endregion
    }
}