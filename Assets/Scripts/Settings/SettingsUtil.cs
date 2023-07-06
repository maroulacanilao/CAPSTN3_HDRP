using CustomEvent;
using ScriptableObjectData;

namespace Settings
{
    public static class SettingsUtil
    {
        private static SettingsData SettingsData;
        
        private static SettingsValues Settings => SettingsData.settings;
        
        public static readonly Evt OnValuesChanged = new Evt();
        
        public static void Initialize(SettingsData settingsData_)
        {
            SettingsData = settingsData_;
        }

        public static float GetMasterVolume()
        {
            return Settings.masterVolume;
        }
        
        public static float GetMusicVolume()
        {
            return Settings.masterVolume * Settings.musicVolume;
        }
        
        public static float GetSfxVolume()
        {
            return Settings.masterVolume * Settings.sfxVolume;
        }
        
        public static void SetMasterVolume(float value_)
        {
            Settings.masterVolume = value_;
            OnValuesChanged.Invoke();
        }
        
        public static void SetMusicVolume(float value_)
        {
            Settings.musicVolume = value_;
            OnValuesChanged.Invoke();
        }
        
        public static void SetSfxVolume(float value_)
        {
            Settings.sfxVolume = value_;
            OnValuesChanged.Invoke();
        }
        
        public static void SetVolumes(float master_, float music_, float sfx_)
        {
            Settings.masterVolume = master_;
            Settings.musicVolume = music_;
            Settings.sfxVolume = sfx_;
            
            OnValuesChanged.Invoke();
        }
        
        public static void ResetSettings()
        {
            ChangeSettings(SettingsData.defaultSettingsValues);
        }
        
        public static void ChangeSettings(SettingsValues values_)
        {
            Settings.masterVolume = values_.masterVolume;
            Settings.musicVolume = values_.musicVolume;
            Settings.sfxVolume = values_.sfxVolume;
            
            OnValuesChanged.Invoke();
        }

        public static void SaveSettings()
        {
            SettingsData.Save();
        }
    }
}