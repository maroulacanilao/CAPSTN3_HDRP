using System;
using System.Collections;
using CustomHelpers;
using NaughtyAttributes;
using ObjectPool;
using UnityEngine;

namespace Others.VFX
{
    public abstract class EffectsPlayer : MonoBehaviour
    {
        [SerializeField] protected ObjectPool.Poolable poolableObject;
        [SerializeField] protected AudioSource audioSource;

        protected virtual void Start()
        {
            poolableObject = gameObject.GetOrAddComponent<Poolable>();
        }
        
        public abstract void Play();

        public abstract IEnumerator Co_Play(float duration_ = 0);

        protected virtual void OnDisable()
        {
            poolableObject.Release();
        }
        
        protected void PlayAudio()
        {
            if (audioSource == null) return;
            // audioSource.volume = Settings.SettingsUtil.GetSfxVolume();
            audioSource.Play();
        }
    }
}
