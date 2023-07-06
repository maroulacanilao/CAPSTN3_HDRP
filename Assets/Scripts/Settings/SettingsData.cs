using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using CustomEvent;
using NaughtyAttributes;
using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "SettingsData", menuName = "ScriptableObjects/SettingsData", order = 0)]
    public class SettingsData : ScriptableObject
    {
        [BoxGroup("Save Properties")]
        [SerializeField] private string saveFileName = "Settings.sav";

        private string savePath;
        
        [field: BoxGroup("Default Settings")]
        [field: SerializeField] public SettingsValues defaultSettingsValues {get ; private set; }

        private SettingsValues mSettings;

        public SettingsValues settings
        {
            get
            {
                if(mSettings == null)
                {
                    mSettings = defaultSettingsValues;
                }
                return mSettings;
            }
            set => mSettings = value;
        }

        public void Initialize()
        {
            savePath = Path.Combine(Application.persistentDataPath, saveFileName);
            Load();
        }
        
        public void ResetSettings()
        {
            settings = defaultSettingsValues;
        }

        [Button("Save")]
        public void Save()
        {
            BinaryFormatter formatter = new BinaryFormatter();

            FileStream fileStream = File.Create(savePath);
            
            formatter.Serialize(fileStream, settings);
            
            fileStream.Close();

            Debug.Log($"Game saved @ {savePath}");
        }
        
        public void Load()
        {
            if(!File.Exists(savePath))
            {
                Debug.Log("No save file found @ "+savePath);
                return;
            }

            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream fileStream = File.Open(savePath, FileMode.Open);
                SettingsValues saveData = (SettingsValues) formatter.Deserialize(fileStream);
                fileStream.Close();
                
                mSettings = saveData;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to load save file @ {savePath}");
                mSettings = defaultSettingsValues;
            }

        }
    }

    [Serializable] 
    public class SettingsValues
    {
        public float masterVolume = 1;
        public float musicVolume = 1;
        public float sfxVolume = 1;
    }
}