using System;
using System.Collections;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.VFX;

namespace Others.VFX
{
    public class VfxPlayer : EffectsPlayer
    {
        [SerializeField] protected VisualEffect vfx;
        [SerializeField] protected float duration;

        protected void Awake()
        {
            if(vfx == null) vfx = GetComponent<VisualEffect>();
            if(vfx == null) vfx = GetComponentInChildren<VisualEffect>();
        }
        
        public override void Play()
        { 
            PlayAudio();
            vfx.Reinit();
            vfx.Play();
        }
        
        public override IEnumerator Co_Play(float duration_ = 0)
        {
            Debug.Log("VfxPlayer: Co_Play || Dur: " + duration_);
            if (duration_ <= 0f) duration_ = 1f;

            vfx.Reinit();
            PlayAudio();
            vfx.Play();
            yield return new WaitForSeconds(duration_);
            vfx.Stop();
            poolableObject.Release();
        }
        
        [Button("Play")]
        private void PlayButton()
        {
            Play();
        }
    }
}