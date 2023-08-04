using System;
using System.Collections;
using Fungus;
using FungusWrapper;
using Managers.SceneLoader;
using ScriptableObjectData;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using SceneLoader = Managers.SceneLoader.SceneLoader;

namespace GameOverScreen
{
    public class GameOverScreen : BaseCore.Singleton<GameOverScreen>
    {
        public GameDataBase gameDataBase;
        public GameObject panel;
        public Button goBack_BTN;
        public string startingDialogue = "GameOverMessage";
        public string receiveMessage = "GameOverScreen";
        public LoadSceneParameters loadSceneParameters;

        protected override void Awake()
        {
            base.Awake();
            FungusManager.Instance.useUnscaledTime = true;
            goBack_BTN.onClick.AddListener(GoBack);
            // FungusReceiver.OnReceiveMessage.AddListener(OnReceiveMessage);
            Time.timeScale = 0;
        }

        private void OnDestroy()
        {
            // FungusReceiver.OnReceiveMessage.AddListener(OnReceiveMessage);
        }
        
        private void OnReceiveMessage(string obj_)
        {
            if (obj_ == receiveMessage)
            {
                Debug.Log("GameOverScreen: " + obj_);
                panel.SetActive(true);
            }
        }
        
        private IEnumerator Start()
        {
            panel.SetActive(false);
            yield return new WaitForSecondsRealtime(.1f);
            // Fungus.Flowchart.BroadcastFungusMessage(startingDialogue);
        }
        
        private void GoBack()
        {
            SceneLoader.OnLoadScene.Invoke(loadSceneParameters);
            gameDataBase.progressionData.ResetProgress();
        }
        
        public void ShowPanel()
        {
            panel.SetActive(true);
        } 
    }
}
