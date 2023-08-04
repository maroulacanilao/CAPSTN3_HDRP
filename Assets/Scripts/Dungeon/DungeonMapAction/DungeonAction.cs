using System;
using System.Collections;
using System.Threading.Tasks;
using DG.Tweening;
using Managers;
using Managers.SceneLoader.SceneTransition;
using UnityEngine;
using UnityEngine.UI;

namespace Dungeon.DungeonMapAction
{
    public abstract class DungeonAction : MonoBehaviour
    {
        [SerializeField] private FadeTransition_Base fadeTransition;
        [SerializeField] private string sfxName;

        private void OnEnable()
        {
            fadeTransition.gameObject.SetActive(false);
        }

        public async void RemoveBlocker()
        {
            fadeTransition.gameObject.SetActive(true);
            fadeTransition.Initialize();
            InputManager.DisableInput();
            TimeManager.PauseTime();
            
            await fadeTransition.StartTransition(false);
            
            RemoveBlockerHandler();
            PlaySfx();
            
            InputManager.DisableInput();
            TimeManager.PauseTime();

            await Task.Delay(500);
            await fadeTransition.StartTransition(true);
            fadeTransition.gameObject.SetActive(false);
            
            InputManager.EnableInput();
            TimeManager.ResumeTime();
        }

        protected abstract void RemoveBlockerHandler();

        protected virtual void PlaySfx()
        {
            if(string.IsNullOrEmpty(sfxName)) return;
            
            AudioManager.PlaySfx(sfxName);
        }
    }
}
