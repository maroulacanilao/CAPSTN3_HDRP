using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace ScriptableObjectData
{
    [CreateAssetMenu(fileName = "ObjectiveData", menuName = "ScriptableObjects/ObjectiveData", order = 0)]
    public class ObjectiveData : ScriptableObject
    {
        [SerializeField] private SerializedDictionary<string,string> objectiveText = new SerializedDictionary<string, string>();
        
        
        public bool TryGetObjectiveText(string key_, out string text_)
        {
            return objectiveText.TryGetValue(key_, out text_);
        }
    }
}