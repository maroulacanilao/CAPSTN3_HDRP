using System.Collections;
using System.Threading.Tasks;
using BaseCore;
using DG.Tweening;
using Managers;
using Managers.SceneLoader.SceneTransition;
using UnityEngine;
using UnityEngine.UI;

namespace Dungeon
{
    public class DungeonMapExit : InteractableObject
    {
        [SerializeField] private FadeTransition_Base fadeTransition;
        protected override void Interact()
        {
            NextLevel();
        }
        
        protected override void Enter()
        {

        }
        protected override void Exit()
        {
        
        }
        
        protected override void OnEnable()
        {
            base.OnEnable();
            // if(OverlayImage != null) OverlayImage.color = new Color(OverlayImage.color.r, OverlayImage.color.g, OverlayImage.color.b, 0f);
            // if(Overlay != null) Overlay.SetActive(false);
            fadeTransition.gameObject.SetActive(false);
        }
        
        public async void NextLevel()
        {
            fadeTransition.gameObject.SetActive(true);
            fadeTransition.Initialize();
            InputManager.DisableInput();
            TimeManager.PauseTime();
            
            await fadeTransition.StartTransition(false);
            InputManager.DisableInput();
            TimeManager.PauseTime();
            
            DungeonManager.Instance.GoToNextLevel();
            await Task.Delay(500);
            await fadeTransition.StartTransition(true);
            
            InputManager.EnableInput();
            TimeManager.ResumeTime();
            
            fadeTransition.gameObject.SetActive(false);
        }

        public IEnumerator GoToNextLevel()
        {
            // if (Overlay == null || OverlayImage == null)
            // {
            //     DungeonManager.Instance.GoToNextLevel();
            //     yield break;
            // }
            //     
            // Overlay.SetActive(true);
            // OverlayImage.color = new Color(OverlayImage.color.r, OverlayImage.color.g, OverlayImage.color.b, 0f);
            // OverlayImage.gameObject.SetActive(true);
            // InputManager.DisableInput();
            // TimeManager.PauseTime();
            //
            // yield return OverlayImage.DOFade(1f, 0.5f).SetUpdate(true).WaitForCompletion();
            //
            // Overlay.SetActive(false);
            // OverlayImage.gameObject.SetActive(true);
            // InputManager.EnableInput();
            // TimeManager.ResumeTime();
            // OverlayImage.color = new Color(OverlayImage.color.r, OverlayImage.color.g, OverlayImage.color.b, 0f);
            //
            // DungeonManager.Instance.GoToNextLevel();
            yield break;
        }
    }
}
