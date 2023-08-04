using System;
using System.Threading.Tasks;
using Dungeon;
using FungusWrapper;
using Managers;
using Managers.SceneLoader;
using Managers.SceneLoader.SceneTransition;
using UnityEngine;

namespace Farming.Ending
{
    public class EndingCutscene : MonoBehaviour
    {
        [SerializeField]
        private FadeTransition_Base fadeTransition;

        [SerializeField]
        private GameObject camera;

        [SerializeField]
        private GameObject endScreen;
        
        [SerializeField] private string sendMsg, receiveMsg;
        
        [SerializeField] private LoadSceneParameters loadSceneParameters;

        private void Awake()
        {
            FungusReceiver.OnReceiveMessage.AddListener(OnReceiveMessage);
        }

        private void OnDestroy()
        {
            FungusReceiver.OnReceiveMessage.RemoveListener(OnReceiveMessage);
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player")) StartCutScene();
        }

        private async void StartCutScene()
        {
            fadeTransition.gameObject.SetActive(true);
            fadeTransition.Initialize();
            InputManager.DisableInput();
            TimeManager.PauseTime();

            await fadeTransition.StartTransition(false);
            camera.SetActive(true);
            InputManager.DisableInput();
            TimeManager.PauseTime();
            

            await Task.Delay(500);
            await fadeTransition.StartTransition(true);
            fadeTransition.gameObject.SetActive(false);

            await Task.Delay(500);
             Time.timeScale = 1;
            Fungus.Flowchart.BroadcastFungusMessage(sendMsg);
        }
        
        private void OnReceiveMessage(string obj_)
        {
            if(obj_ == receiveMsg)
            {
                endScreen.SetActive(true);
            }
        }
        
        public void GoBackToTitle()
        {
            SceneLoader.OnLoadScene.Invoke(loadSceneParameters);
        }
    }
}
