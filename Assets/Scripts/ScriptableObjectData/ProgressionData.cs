using System;
using BaseCore;
using Items.Inventory;
using NaughtyAttributes;
using ScriptableObjectData.CharacterData;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using SaveSystem;

namespace ScriptableObjectData
{
    [CreateAssetMenu(fileName = "ProgressionData", menuName = "ScriptableObjects/ProgressionData", order = 0)]
    public class ProgressionData : ScriptableObject
    {
        [SerializeField] private GameDataBase gameDataBase;
        
        [BoxGroup("Save Properties")]
        [SerializeField] private string saveFileName = "SaveData";
        
        [BoxGroup("Save Properties")]
        [SerializeField] private string saveFileExtension = ".sav";
        
        [BoxGroup("Save Properties")]
        [SerializeField] private string saveFolder = "Save/";
        

        private PlayerData playerData => gameDataBase.playerData;
        private PlayerInventory inventory => playerData.inventory;
        private PlayerLevel levelData => playerData.LevelData;
        
        public int dayCounter { get; set; } = 0;
        public int highestDungeonLevel { get; set; } = 1;
        public bool hasFinishedTutorial { get; set; }
        
        public SaveData saveData { get; private set; }
        
        public string savePath { get; private set; }

        public void Initialize()
        {
            savePath = Path.Combine(Application.persistentDataPath, saveFolder+saveFileName+saveFileExtension);
            ResetProgress();
            LoadProgress();
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
        public void LoadProgress()
        {
            if (File.Exists(savePath))
            {
                try
                {
                    // Create a BinaryFormatter to deserialize the data
                    BinaryFormatter formatter = new BinaryFormatter();
                
                    // Open the save file
                    FileStream fileStream = File.Open(savePath, FileMode.Open);
                
                    // Deserialize the data from the file
                    saveData = (SaveData)formatter.Deserialize(fileStream);
                
                    // Close the file stream
                    fileStream.Close();
                }
                catch (Exception e)
                {
                    saveData = null;
                    Debug.LogError($"Failed to load save file: {e.Message}");
                }

            }
            else
            {
                Debug.Log("No save file found.");
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