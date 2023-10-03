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
        
        public void InitalizeMembers(GameDataBase gameDataBase_)
        {
            foreach (var a in allyDataDictionary.Values)
            {
                a.Initialize(gameDataBase_);
            }
        }
        
        // public void TransferKeyValuePair(string key)
        // {
        //     // Check if the key exists in the source dictionary
        //     if (allyDataDictionary.ContainsKey(key))
        //     {
        //         // Get the value associated with the key
        //         var value = allyDataDictionary[key];
        //
        //         // Add the key-value pair to the destination dictionary
        //         playableAllyDataDictionary.Add(key, value);
        //
        //         // Remove the key-value pair from the source dictionary if needed
        //         allyDataDictionary.Remove(key);
        //     }
        //     else
        //     {
        //         // Handle the case where the key does not exist in the source dictionary
        //         Debug.LogWarning("Key " + key + " does not exist in dictionary1.");
        //     }
        // }
    }

}
