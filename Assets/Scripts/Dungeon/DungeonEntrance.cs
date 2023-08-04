using System;
using BaseCore;
using Managers;
using Managers.SceneLoader;
using ScriptableObjectData;
using UnityEngine;

namespace Dungeon
{
    public class DungeonEntrance : InteractableObject
    {
        [SerializeField] private SessionData sessionData;
        [SerializeField] private GameObject canvas;
        [SerializeField] private LoadSceneParameters loadSceneParameters;
        protected override void OnEnable()
        {
            base.OnEnable();
            canvas.SetActive(false);
        }
        
        protected override void Interact()
        {
            canvas.SetActive(true);
        }
        protected override void Enter()
        {
            
        }
        
        protected override void Exit()
        {
            
        }

        public void GoBackToFarm()
        {
            if(sessionData == null) sessionData = GameManager.Instance.GameDataBase.sessionData;
            sessionData.farmLoadType = FarmLoadType.DungeonEntrance;
            SceneLoader.OnLoadScene.Invoke(loadSceneParameters);
        }

        public void GoBackToLastDungeon()
        {
            DungeonManager.Instance.GoToPrevDungeon();
        }
    }
}
