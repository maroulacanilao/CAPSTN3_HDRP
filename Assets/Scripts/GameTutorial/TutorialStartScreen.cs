using System;
using System.Threading.Tasks;
using Managers;
using Managers.SceneLoader.SceneTransition;
using UnityEngine;

namespace GameTutorial
{
    public class TutorialStartScreen : MonoBehaviour
    {
        public FadeTransition_Base fadeTransition;

        public async void CloseTransition()
        {
            fadeTransition.Initialize();
            TimeManager.PauseTime(true);
            await Task.Delay(500);
            fadeTransition.gameObject.SetActive(true);
            await fadeTransition.StartTransition(true);
            await Task.Delay(500);
            TimeManager.ResumeTime();
            gameObject.SetActive(false);
        }
    }
}
