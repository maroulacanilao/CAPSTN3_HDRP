using UnityEngine;
using Managers;

namespace ScriptableObjectData
{
    [CreateAssetMenu(fileName = "SessionData", menuName = "ScriptableObjects/SessionData", order = 0)]
    public class SessionData : ScriptableObject
    {
        public bool hasStartedTime { get; set; }
        public bool hasInitialized { get; set; }
        public FarmLoadType farmLoadType { get; set; }

        public int dungeonLevel { get; set; } = 1;

        public void InitializeSession()
        {
            if(hasInitialized) return;
            hasStartedTime = false;
            farmLoadType = FarmLoadType.NewGame;
            hasInitialized = true;
        }
        
        public void DeInitialize()
        {
            hasInitialized = false;
        }
    }
}