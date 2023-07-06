using System;
using System.Collections;
using System.Globalization;
using BaseCore;
using Items.Inventory;
using NaughtyAttributes;
using ScriptableObjectData.CharacterData;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using Farming;
using Managers;
using SaveSystem;

namespace ScriptableObjectData
{
    [CreateAssetMenu(fileName = "ProgressionData", menuName = "ScriptableObjects/ProgressionData", order = 0)]
    public class ProgressionData : ScriptableObject
    {
        [SerializeField] private GameDataBase gameDataBase;
        
        [BoxGroup("Save Properties")]
        [SerializeField] private string saveFileName = "SaveData.sav";


        private PlayerData playerData => gameDataBase.playerData;
        private PlayerInventory inventory => playerData.inventory;
        private PlayerLevel levelData => playerData.LevelData;
        
        public int dayCounter { get; set; } = 0;
        public int highestDungeonLevel { get; set; } = 1;
        public bool hasFinishedTutorial { get; set; }
        
        public SaveData saveData { get; private set; }
        
        public string savePath { get; private set; }
        
        public bool isLoadOperationDone { get; private set; }

        public void Initialize()
        {
            isLoadOperationDone = false;
            savePath = Path.Combine(Application.persistentDataPath, saveFileName);
            ResetProgress();
            LoadData();
        }
        
        public void DeInitialize()
        {
            saveData = null;
        }


        [Button("Save")]
        public void SaveProgression()
        {
            SaveData saveData = SaveHelper.SaveProgress(gameDataBase);
            
            BinaryFormatter formatter = new BinaryFormatter();

            FileStream fileStream = File.Create(savePath);
            
            formatter.Serialize(fileStream, saveData);
            
            fileStream.Close();

            Debug.Log($"Game saved @ {savePath}");
        }

        [Button("Load")]
        public void LoadData()
        {
            if (!File.Exists(savePath))
            {
                Debug.Log($"No save file found @ {savePath}");
            }
            
            try
            {
                // Create a BinaryFormatter to deserialize the data
                BinaryFormatter formatter = new BinaryFormatter();

                // Open the save file
                FileStream fileStream = File.Open(savePath, FileMode.Open);

                // Deserialize the data from the file
                saveData = (SaveData) formatter.Deserialize(fileStream);

                // Close the file stream
                fileStream.Close();
            }
            catch (Exception e)
            {
                saveData = null;
                Debug.LogError($"Failed to load save file: {e.Message}");
            }
        }
        
        public IEnumerator LoadProgression()
        {
            isLoadOperationDone = false;
            if (saveData == null)
            {
                Debug.Log("No save data found");
                isLoadOperationDone = true;
                yield break;
            }

            gameDataBase.sessionData.farmLoadType = FarmLoadType.LoadGame;
            var _sceneName = gameDataBase.FarmSceneName;
            gameDataBase.eventQueueData.AddEvent(_sceneName, InGameLoad);

            try
            {
                LoadHelper.LoadData(saveData, gameDataBase);
                isLoadOperationDone = true;
            }
            catch (Exception e)
            {
                Debug.Log(e);
                isLoadOperationDone = true;
            }

            yield return new WaitForSeconds(1);
        }

        private void InGameLoad()
        {
            try
            {
                LoadHelper.LoadFarmTiles(saveData.farmTileSaveData, gameDataBase);
                
                if (DateTime.TryParseExact(saveData.timeOfDay, "yyyyMMddHHmmss", 
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out var _parsedDateTime))
                {
                    TimeManager.StartTime(_parsedDateTime);
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to load In Game: {e.Message}");
            }
        }

        [Button("ResetProgress")]
        private void ResetProgress()
        {
            dayCounter = 0;
            highestDungeonLevel = 1;
            hasFinishedTutorial = false;
            playerData.ResetData();
        }
    }
}