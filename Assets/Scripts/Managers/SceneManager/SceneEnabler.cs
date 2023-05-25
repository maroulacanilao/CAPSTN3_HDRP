using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers.SceneManager
{
    public class SceneEnabler : MonoBehaviour
    {
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
    }
}
