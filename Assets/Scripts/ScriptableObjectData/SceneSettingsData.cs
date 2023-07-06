using NaughtyAttributes;
using UnityEngine;

namespace ScriptableObjectData
{
    [CreateAssetMenu(menuName = "ScriptableObjects/SceneSettingsData", fileName = "New SceneSettingsData")]
    public class SceneSettingsData : ScriptableObject
    {
        [field: Scene] [field: SerializeField] public string sceneName {get; private set; }
    
        [field: BoxGroup("Time Settings")] [field: SerializeField] 
        public bool willChangeTime {get; private set; }= true;
        
        [field: BoxGroup("Time Settings")] [field: SerializeField] [field: ShowIf("willChangeTime")]
        public bool canStartTime { get; private set; } = false;
    
        [field: BoxGroup("Time Settings")] [field: SerializeField] [field: ShowIf("willChangeTime")] 
        public bool willPause {get; private set; }

        [field: BoxGroup("Time Settings")] [field: SerializeField] [field: ShowIf("willPause")]
        public bool willAffectTimeScale { get; private set; } = false;
        

        [field: BoxGroup("Player Settings")] [field: SerializeField] 
        public bool willEnablePlayer {get; private set; } = true;
    
        [field: BoxGroup("Player Settings")] [field: SerializeField] [field: ShowIf("willEnablePlayer")] 
        public bool canUseTool {get; private set; }
    
        [field: BoxGroup("Player Settings")] [field: SerializeField] [field: ShowIf("willEnablePlayer")] 
        public bool willEnableLantern {get; private set; }

        [field: BoxGroup("Controller Settings")] [field: SerializeField] 
        public bool willEnableController {get; private set; } = true;
        
        [field: BoxGroup("Controller Settings")] [field: SerializeField] 
        public bool willEnableCursor {get; private set; } = true;
        
        [field: BoxGroup("UI Settings")] [field: SerializeField] 
        public bool willEnablePlayerHUD {get; private set; } = true;
    }
}
