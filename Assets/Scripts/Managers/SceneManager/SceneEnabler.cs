using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers.SceneLoader
{
    public class SceneEnabler : MonoBehaviour
    {
        private Transform mInstanceParent;
        
        public Transform InstanceParent
        {
            get
            {
                if (mInstanceParent != null) return mInstanceParent;
                
                mInstanceParent = new GameObject("Instance Parent").transform;
                return mInstanceParent;
            }
        }
        
        public Scene scene { get; private set; }

        private List<GameObject> rootObjects;
        
        private void Awake()
        {
            scene = gameObject.scene;
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += OnActiveSceneChanged;
        }

        private void OnDestroy()
        {
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged -= OnActiveSceneChanged;
        }
        
        private void OnActiveSceneChanged(Scene current, Scene next)
        {
            if(!current.IsValid()) return;
            EnableObjectsOnScene(next == gameObject.scene);
        }

        public void EnableObjectsOnScene(bool willEnable)
        {
            foreach (GameObject o in scene.GetRootGameObjects())
            {
                o.SetActive(willEnable);
            }
        }
        
        public static SceneEnabler FindSceneEnabler(string sceneName)
        {
            var _scene = UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneName);
            
            if(!_scene.IsValid()) return null;
            
            var _enablers = FindObjectsOfType<SceneEnabler>(true);
            
            foreach (var _enabler in _enablers)
            {
                if (_enabler.scene != _scene) continue;
                
                return _enabler;
            }
            
            var _newEnabler = new GameObject($"SceneEnabler").AddComponent<SceneEnabler>();
            UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(_newEnabler.gameObject, _scene);
            return null;
        }
    }
}
