using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseCore;
using CustomEvent;
using CustomHelpers;
using UnityEngine;
using UnityEngine.SceneManagement;
using Managers;
using Managers.SceneManager.SceneTransition;

namespace Managers.SceneManager
{
    
    public enum LoadSceneType
    {
        LoadAdditive = 0,
        LoadSingle = 1,
        Unload = 3,
        ReloadSingle = 4,
        ReloadAdditive = 5,
    }
    
    public class SceneLoader : SingletonPersistent<SceneLoader>
    {
       
        // transition 
        [SerializeField] private FadeTransition_Base[] _transitionsList;
        [SerializeField] [NaughtyAttributes.Scene] private string fallbackScene;
        private FadeTransition_Base currentFade;
        
        // sceneEnablers
        private List<SceneEnabler> sceneEnablerList = new List<SceneEnabler>();
        private SceneEnabler currentSceneEnabler;

        public static readonly Evt<string, LoadSceneType> OnLoadScene = new Evt<string, LoadSceneType>();

        protected override void Awake()
        {
            OnLoadScene.AddListener(ManageScene);
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnOpeningTransition;
            UnityEngine.SceneManagement.SceneManager.sceneUnloaded += OnClosingTransition;
            
            foreach (var transition in _transitionsList)
            {
                transition.Initialize();
            }
            
            SceneEnabler sceneEnabler = GetSceneEnabler(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
            
            sceneEnablerList.Add(sceneEnabler);

            currentFade = GetRandomFade();
        }

        private void OnDestroy()
        {
            OnLoadScene.RemoveListener(ManageScene);
            UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnOpeningTransition;
            UnityEngine.SceneManagement.SceneManager.sceneUnloaded -= OnClosingTransition;
        }

        private SceneEnabler GetSceneEnabler(string sceneName)
        {
            return UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneName).FindFirstComponentInScene<SceneEnabler>(true);
        }

        private void ManageScene(string sceneName, LoadSceneType type)
        {
            switch (type)
            {
                case LoadSceneType.LoadAdditive:
                {
                    LoadScene(sceneName, true);
                    break;
                }
                case LoadSceneType.LoadSingle:
                {
                    LoadScene(sceneName, false);
                    break;
                }
                case LoadSceneType.Unload:
                {
                    UnloadScene(sceneName);
                    break;
                }
                case LoadSceneType.ReloadAdditive:
                {
                    ReloadScene(sceneName, true);
                    break;
                }
                case LoadSceneType.ReloadSingle:
                {
                    ReloadScene(sceneName, false);
                    break;
                }
                
            }
        }

        private async void LoadScene(string sceneToLoad, bool isAdditive)
        {
            Time.timeScale = 0;

            LoadSceneMode mode = isAdditive ? LoadSceneMode.Additive : LoadSceneMode.Single;

            AsyncOperation load = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneToLoad, mode);
            
            load.allowSceneActivation = false;

            currentFade = GetRandomFade();
            
            currentFade.gameObject.SetActive(true);

            await currentFade.StartTransition(false);

            load.allowSceneActivation = true;
        }

        private async void UnloadScene(string sceneToUnload)
        {
            Time.timeScale = 0;

            SceneEnabler enablerToRemove = sceneEnablerList.FirstOrDefault(s => s.scene.name == sceneToUnload);

            sceneEnablerList.Remove(enablerToRemove);
            
            if (sceneEnablerList.Count == 0)
            {
                // if no other scene is loaded, but this shouldn't happen
                LoadScene(fallbackScene, false);
                return;
            }
            

            currentFade = GetRandomFade();
            
            currentFade.gameObject.SetActive(true);
            
            string prevScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

            await currentFade.StartTransition(false);

            AsyncOperation unload = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(
                sceneToUnload,
                UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);

            foreach (SceneEnabler sceneEnabler in sceneEnablerList)
            {
                UnityEngine.SceneManagement.SceneManager.SetActiveScene(sceneEnabler.scene);
            }
        }

        private async void ReloadScene(string sceneToReload, bool isAdditive)
        {
            Time.timeScale = 0;
            
            var enablerToRemove = sceneEnablerList.FirstOrDefault(s => s.scene.name == sceneToReload);
            sceneEnablerList.Remove(enablerToRemove);

            try
            {
                AsyncOperation unload = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(
                    sceneToReload,
                    UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);

                while (!unload.isDone)
                {
                    await Task.Delay(50);
                }
                LoadScene(sceneToReload, isAdditive);
            }
            catch (Exception e)
            {
                print(e);
                LoadScene(sceneToReload, false);
            }

            // LoadScene(sceneToReload, isAdditive);
        }

        private async void OnOpeningTransition(Scene scene, LoadSceneMode mode)
        {
            Time.timeScale = 0;

            UnityEngine.SceneManagement.SceneManager.SetActiveScene(scene);
            
            SceneEnabler sceneEnabler = GetSceneEnabler(scene.name);
            if (sceneEnabler == null)
            {
                GameObject newObject = new GameObject("Scene Enabler");
                UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(newObject, scene);
                sceneEnabler = newObject.AddComponent<SceneEnabler>();
            }

            if (mode == LoadSceneMode.Additive)
            {
                if (!sceneEnablerList.Contains(sceneEnabler) && sceneEnabler != null)
                {
                    sceneEnablerList.Add(sceneEnabler);
                    currentSceneEnabler = sceneEnabler;
                }
            }
            else
            {
                if (sceneEnabler != null)
                {
                    sceneEnablerList = new List<SceneEnabler>();
                    sceneEnablerList.Add(sceneEnabler);
                    currentSceneEnabler = sceneEnabler;
                }
            }

            await currentFade.StartTransition(true);
            

            
            currentFade.gameObject.SetActive(false);
            
            Time.timeScale = 1;
        }
        
        private async void OnClosingTransition(Scene scene)
        {
            Time.timeScale = 0;

            await currentFade.StartTransition(true);
            
            currentSceneEnabler = sceneEnablerList.FirstOrDefault();

            currentFade.gameObject.SetActive(false);
            
            Time.timeScale = 1;
        }

        private FadeTransition_Base GetRandomFade()
        {
            return _transitionsList.GetRandomItem();
        }
    }
}
