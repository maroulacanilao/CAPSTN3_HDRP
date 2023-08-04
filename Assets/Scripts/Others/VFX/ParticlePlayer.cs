using System;
using System.Collections;
using CustomHelpers;
using UnityEngine;

namespace Others.VFX
{
    public class ParticlePlayer : EffectsPlayer
    {
        [SerializeField] protected ParticleSystem particleSystem;

        private void Reset()
        {
            poolableObject = gameObject.GetOrAddComponent<ObjectPool.Poolable>();
            particleSystem = GetComponent<ParticleSystem>();
        }
        protected void Awake()
        {
            if(particleSystem == null) particleSystem = GetComponent<ParticleSystem>();
            if(particleSystem == null) particleSystem = GetComponentInChildren<ParticleSystem>();
        }
        
        public override void Play()
        {
            if (particleSystem.IsEmptyOrDestroyed()) return;
            
            PlayAudio();
            particleSystem.Stop();
            particleSystem.Play();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            if(particleSystem.IsValid()) particleSystem.Stop();
        }

        public override IEnumerator Co_Play(float duration_ = 0)
        {
            Debug.Log("ParticlePlayer: Co_Play || Dur: " + duration_);
            if(particleSystem.IsEmptyOrDestroyed()) yield break;
            particleSystem.Stop();
            if(duration_.IsApproximately0()) duration_ = particleSystem.main.duration;
            
            particleSystem.Play();
            PlayAudio();
            yield return new WaitForSeconds(duration_);
            if(particleSystem.IsValid()) particleSystem.Stop();
            StartCoroutine(Co_Stop());
        }

        public IEnumerator Co_Stop()
        {
            yield return new WaitForSecondsRealtime(0.1f);
            poolableObject.Release();
        }
    }
}