using System;
using AYellowpaper.SerializedCollections;
using CustomHelpers;
using UnityEngine;

namespace ScriptableObjectData
{
    [CreateAssetMenu(menuName = "ScriptableObjects/AudioDataBase", fileName = "AudioDataBase", order = 0)]
    public class AudioDataBase : ScriptableObject
    {
        [SerializedDictionary("Scene Name","Background Music")]
        [SerializeField] private SerializedDictionary<string, AudioClip> backgroundMusics = new SerializedDictionary<string, AudioClip>();
    
        [SerializedDictionary("Audio ID","SFX")]
        [SerializeField] private SerializedDictionary<string, AudioClip> soundEffectsDictionary;

        public AudioClip GetSceneMusic(string sceneName)
        {
            if(string.IsNullOrEmpty(sceneName)) return null;
            return backgroundMusics.TryGetValue(sceneName, out var music) ? music : null;
        }

        public AudioClip GetSfx(string audioID)
        {
            if(string.IsNullOrEmpty(audioID)) return null;
            return soundEffectsDictionary.TryGetValue(audioID, out var music) ? music : null;
        }
    }
}
