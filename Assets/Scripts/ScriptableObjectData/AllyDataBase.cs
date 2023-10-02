using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using CustomHelpers;
using NaughtyAttributes;
using ScriptableObjectData.CharacterData;
using UnityEngine;

namespace ScriptableObjectData
{
    [CreateAssetMenu(fileName = "AllyDataBase", menuName = "ScriptableObjects/AllyDataBase", order = 99)]
    public class AllyDataBase : ScriptableObject
    {
        [field: SerializedDictionary("ID", "allyData")]
        [field: SerializeField] public SerializedDictionary<string, AllyData> allyDataDictionary { get; private set; }
        
        [field: SerializedDictionary("ID", "playableAllyData")]
        
        [field: SerializeField] public SerializedDictionary<string, AllyData> playableAllyDataDictionary;
        public void InitalizeMembers(GameDataBase gameDataBase_)
        {
            foreach (var a in allyDataDictionary.Values)
            {
                a.Initialize(gameDataBase_);
            }
        }
        
        public void TransferKeyValuePair(string key)
        {
            Dictionary<string, AllyData> source = allyDataDictionary;
            Dictionary<string, AllyData> destination = playableAllyDataDictionary;
            
            // Check if the key exists in the source dictionary
            if (source.ContainsKey(key))
            {
                // Get the value associated with the key
                var value = source[key];

                // Add the key-value pair to the destination dictionary
                destination.Add(key, value);

                // Remove the key-value pair from the source dictionary if needed
                source.Remove(key);
            }
            else
            {
                // Handle the case where the key does not exist in the source dictionary
                Debug.LogWarning("Key " + key + " does not exist in dictionary1.");
            }
        }
    }

}
