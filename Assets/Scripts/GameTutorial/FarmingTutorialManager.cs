using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using CustomHelpers;
using Farming;
using Fungus;
using FungusWrapper;
using Items.ItemData;
using Managers;
using Managers.SceneLoader;
using NaughtyAttributes;
using ScriptableObjectData;
using Spells.Base;
using UI.HUD;
using UnityEngine;
using SceneLoader = Managers.SceneLoader.SceneLoader;

namespace GameTutorial
{
    [DefaultExecutionOrder(-1)]
    public class FarmingTutorialManager : MonoBehaviour
    {
        public GameDataBase gameDataBase;
        public int maxHour;
        public Flowchart tutorialFlowchart;
        public SpellData healSpell;
        public PersistentTimeStopper timeStopper;
        
        
        [Header("When Done")]
        public string doneWithTutorialVariableName;
        public string OnLearnSpell;
        public CinemachineVirtualCamera tempCam;
        public LoadSceneParameters loadSceneParameters;

        [Header("Messages")]
        public string hoeMessage;
        public string wateringMessage;
        public string plantingMessage;
        public string harvestingMessage;
        public string harvestMoreMessage;
        public string doneHarvestingMessage;
        public string doneWithTutorialMessage;

        List<FarmTile> farmTiles = new List<FarmTile>();

        private bool isMaxTime;
        private void Awake()
        {
            TutorialValues.HavePlanted = false;
            TutorialValues.HaveWatered = false;
            TutorialValues.HaveGrown = false;
            TutorialValues.HasHarvested = false;
            
            TutorialValues.HarvestedCount = 0;
            // if (TutorialValues.DoneWithTutorial)
            // {
            //     OnTutorialDone();
            //     return;
            // }
            FarmTileManager.OnHarvestCrop.AddListener(OnHarvestCrop);
            FarmTileManager.OnAddFarmTile.AddListener(OnAddFarmTile);
            FarmTileManager.OnRemoveTile.AddListener(OnRemoveTile);
            FungusReceiver.OnReceiveMessage.AddListener(OnReceiveMessage);
            TimeManager.OnHourTick.AddListener(OnHourTick);
            gameDataBase.sessionData.farmLoadType = FarmLoadType.NewGame;

        }

        private void Start()
        {
            FarmSceneManager.Instance.SetSceneOnType();
        }

        private void OnDestroy()
        {
            FarmTileManager.OnHarvestCrop.RemoveListener(OnHarvestCrop);
            FarmTileManager.OnAddFarmTile.RemoveListener(OnAddFarmTile);
            FarmTileManager.OnRemoveTile.RemoveListener(OnRemoveTile);
            FungusReceiver.OnReceiveMessage.RemoveListener(OnReceiveMessage);
            TimeManager.OnHourTick.RemoveListener(OnHourTick);

            for (int i = farmTiles.Count - 1; i >= 0; i--)
            {
                OnRemoveTile(farmTiles[i]);
            }
        }

        private void OnReceiveMessage(string obj_)
        {
            if (obj_.ToHash() == doneWithTutorialMessage.ToHash())
            {
                OnTutorialDone();
                return;
            }
            if(obj_.ToHash() == OnLearnSpell.ToHash())
            {
                var _playerData = gameDataBase.playerData;
                _playerData.spells.AddUnique(healSpell);
            }
        }

        private void OnAddFarmTile(FarmTile obj_)
        {
            farmTiles.Add(obj_);
            obj_.OnChangeState.AddListener(OnTileChangeState);
            if (!TutorialValues.HavePlanted)
            {
                TutorialValues.HavePlanted = true;
                Fungus.Flowchart.BroadcastFungusMessage(plantingMessage);
            }
        }
        
        private void OnRemoveTile(FarmTile obj_)
        {
            farmTiles.Remove(obj_);
            obj_.OnChangeState.RemoveListener(OnTileChangeState);
        }
        
        private void OnHourTick()
        {
            if (TimeManager.CurrentHour >= maxHour && !isMaxTime)
            {
                isMaxTime = true;
                timeStopper.enabled = true;
            }
        }
        
        private void OnHarvestCrop(SeedData obj_)
        {
            TutorialValues.HarvestedCount++;
            
            if (TutorialValues.HarvestedCount == 1)
            {
                TutorialValues.HasHarvested = true;
                Fungus.Flowchart.BroadcastFungusMessage(harvestMoreMessage);
            }
            if (TutorialValues.HarvestedCount >= 5 && !TutorialValues.IsFarmingTutorialDone)
            {
                Fungus.Flowchart.BroadcastFungusMessage(doneHarvestingMessage);
                TutorialValues.IsFarmingTutorialDone = true;
            }
            else if(TutorialValues.HarvestedCount < 5 && !TutorialValues.IsFarmingTutorialDone)
            {
                ObjectiveHUD.OnUpdateObjectiveText.Invoke($"Harvest 5 crops {TutorialValues.HarvestedCount}/5");
            }
        }
        
        private void OnTileChangeState(FarmTile tile_, TileState tileState_)
        {
            switch (tileState_)
            {
                case TileState.Empty when !TutorialValues.HavePlanted:
                    TutorialValues.HavePlanted = true;
                    Fungus.Flowchart.BroadcastFungusMessage(plantingMessage);
                    return;
                case TileState.Planted when !TutorialValues.HaveWatered:
                    TutorialValues.HaveWatered = true;
                    Fungus.Flowchart.BroadcastFungusMessage(wateringMessage);
                    return;
                case TileState.Growing when !TutorialValues.HaveGrown:
                    TutorialValues.HavePlanted = true;
                    TutorialValues.HaveWatered = true;
                    TutorialValues.HaveGrown = true;
                    Fungus.Flowchart.BroadcastFungusMessage(harvestingMessage);
                    break;
            }
            
            if(tileState_ == TileState.Growing && isMaxTime)
            {
                tile_.ChangeState(tile_.readyToHarvestTileState);
            }
        }

        private void OnTutorialDone()
        {
            // TutorialValues.IsFarmingTutorialDone = true;
            // TutorialValues.DoneWithTutorial = true;
            // tutorialFlowchart.SetBooleanVariable(doneWithTutorialVariableName, true);
            // foreach (var _flowchart in Flowchart.CachedFlowcharts)
            // {
            //     _flowchart.SetBooleanVariable(doneWithTutorialVariableName, true);
            // }
            // Destroy(gameObject);
            
            TutorialValues.IsFarmingTutorialDone = true;
            TutorialValues.DoneWithTutorial = true;
            StartCoroutine(OnTutorialDoneCo());
        }
        
        private IEnumerator OnTutorialDoneCo()
        {
            var _sceneName = loadSceneParameters.sceneName;
            tempCam.gameObject.SetActive(true);
            tempCam.Priority = 100;
            
            gameDataBase.sessionData.farmLoadType = FarmLoadType.NewDay;
            gameDataBase.eventQueueData.AddEvent(_sceneName, (() =>
            {
                
                TimeManager.EndDay();
            }));
            
            
            yield return new WaitForSecondsRealtime(3.5f);

            SceneLoader.OnLoadScene.Invoke(loadSceneParameters);
            Debug.Log("Done with tutorial");
        }
        
        
    }
}
