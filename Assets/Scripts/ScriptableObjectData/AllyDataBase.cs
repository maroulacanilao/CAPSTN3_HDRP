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
    }

}
