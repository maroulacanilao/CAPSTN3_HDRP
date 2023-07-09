using System;
using System.Linq;
using CustomHelpers;
using System.Collections.Generic;
using System.Threading.Tasks;
using BaseCore;
using CustomEvent;
using UnityEngine;
using UnityEngine.SceneManagement;
using Managers.SceneLoader.SceneTransition;

namespace Managers.SceneLoader
{
    
    public enum LoadSceneType
    {
        LoadAdditive = 0,
        LoadSingle = 1,
        Unload = 3,
        ReloadSingle = 4,
        ReloadAdditive = 5,
        UnloadAllExcept = 6,
    }

    [Serializable]
    public struct LoadSceneParameters
    {
        public LoadSceneType loadSceneType;
        public string sceneName;
        public string sceneToActivate;
        public bool randomTransition;
        public int transitionIndex;
        
        public LoadSceneParameters(LoadSceneType loadSceneType_, string sceneName_, string sceneToActivate_, bool randomTransition_ = true, int transitionIndex_ = -1)
        {
            loadSceneType = loadSceneType_;
            sceneName = sceneName_;
            sceneToActivate = sceneToActivate_;
            randomTransition = randomTransition_;
            transitionIndex = transitionIndex_;
        }
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
        
        private bool isOperating = false;

        public static readonly Evt<LoadSceneParameters> OnLoadScene = new Evt<LoadSceneParameters>();

        protected override void Awake()
        {
            OnLoadScene.AddListener(ManageScene);
            SceneManager.sceneLoaded += OnOpeningTransition;
            SceneManager.sceneUnloaded += OnClosingTransition;
            
            foreach (var transition in _transitionsList)
            {
                transition.Initialize();
            }
            
            SceneEnabler sceneEnabler = GetSceneEnabler(SceneManager.GetActiveScene().name);
            
            sceneEnablerList.Add(sceneEnabler);

            currentFade = GetRandomFade();
        }

        private void OnDestroy()
        {
            OnLoadScene.RemoveListener(ManageScene);
            SceneManager.sceneLoaded -= OnOpeningTransition;
            SceneManager.sceneUnloaded -= OnClosingTransition;
        }

        private SceneEnabler GetSceneEnabler(string sceneName)
        {
            return SceneManager.GetSceneByName(sceneName).FindFirstComponentInScene<SceneEnabler>(true);
        }

        private void ManageScene(LoadSceneParameters loadSceneParameters_)
        {
            switch (loadSceneParameters_.loadSceneType)
            {
                case LoadSceneType.LoadAdditive:
                {
                    LoadScene(loadSceneParameters_);
                    break;
                }
                case LoadSceneType.LoadSingle:
                {
                    LoadScene(loadSceneParameters_);
                    break;
                }
                case LoadSceneType.Unload:
                {
                    UnloadScene(loadSceneParameters_);
                    break;
                }
                case LoadSceneType.ReloadAdditive:
                {
                    ReloadScene(loadSceneParameters_);
                    break;
                }
                case LoadSceneType.ReloadSingle:
                {
                    ReloadScene(loadSceneParameters_);
                    break;
                }
                case LoadSceneType.UnloadAllExcept:
                {
                    UnloadAllExcept(loadSceneParameters_);
                    break;
                }
            }
        }

        private async void LoadScene(LoadSceneParameters sceneParameters_)
        {
            var _isAdditive = sceneParameters_.loadSceneType == LoadSceneType.LoadAdditive;
            if(_isAdditive && IsSceneAlreadyActive(sceneParameters_.sceneName)) return;
            if(_isAdditive && isOperating) return;
            
            RemoveNullEnablers();
            
            isOperating = true;
            
            Time.timeScale = 0;

            LoadSceneMode mode = _isAdditive ? LoadSceneMode.Additive : LoadSceneMode.Single;

            AsyncOperation load = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneParameters_.sceneName, mode);
            
            load.allowSceneActivation = false;

            if(sceneParameters_.randomTransition) currentFade = GetRandomFade();
            else
            {
                currentFade = sceneParameters_.transitionIndex >= _transitionsList.Length ? 
                    GetRandomFade() :
                    _transitionsList[sceneParameters_.transitionIndex];
            }
            
            currentFade.gameObject.SetActive(true);

            await currentFade.StartTransition(false);

            load.allowSceneActivation = true;
            isOperating = false;
        }

        private async void UnloadScene(LoadSceneParameters sceneParameters_)
        {
            if(isOperating) return;
            
            isOperating = true;
            
            Time.timeScale = 0;

            try
            {
                RemoveNullEnablers();
                SceneEnabler enablerToRemove = sceneEnablerList.FirstOrDefault(s => s.scene.name == sceneParameters_.sceneName);

                sceneEnablerList.Remove(enablerToRemove);
            
                if (sceneEnablerList.Count == 0)
                {
                    // if no other scene is loaded, but this shouldn't happen
                    sceneParameters_.sceneName = fallbackScene;
                    sceneParameters_.loadSceneType = LoadSceneType.LoadSingle;
                    LoadScene(sceneParameters_);
                    return;
                }
            
                if(sceneParameters_.randomTransition) currentFade = GetRandomFade();
                else
                {
                    currentFade = sceneParameters_.transitionIndex >= _transitionsList.Length ? 
                        GetRandomFade() :
                        _transitionsList[sceneParameters_.transitionIndex];
                }
                
            
                currentFade.gameObject.SetActive(true);

                await currentFade.StartTransition(false);


                var _sceneToActivate = !string.IsNullOrEmpty(sceneParameters_.sceneToActivate) ? 
                    SceneManager.GetSceneByName(sceneParameters_.sceneToActivate) : 
                    sceneEnablerList.Last().scene;

                if (_sceneToActivate.IsValid() && _sceneToActivate.isLoaded)
                {
                    SceneManager.SetActiveScene(_sceneToActivate);
                }
                else
                {
                    SceneManager.SetActiveScene(sceneEnablerList.Last().scene);
                }
                
                AsyncOperation unload = SceneManager.UnloadSceneAsync(sceneParameters_.sceneName, UnloadSceneOptions.None);

                isOperating = false;
            }
            catch (Exception e)
            {
                isOperating = false;
                sceneParameters_.sceneName = fallbackScene;
                sceneParameters_.loadSceneType = LoadSceneType.LoadSingle;
                LoadScene(sceneParameters_);
            }
        }

        private async void ReloadScene(LoadSceneParameters sceneParameters_)
        {
            if(isOperating) return;
            
            isOperating = true;
            Time.timeScale = 0;
            RemoveNullEnablers();
            
            var enablerToRemove = sceneEnablerList.FirstOrDefault(s => s.scene.name == sceneParameters_.sceneName);
            sceneEnablerList.Remove(enablerToRemove);

            try
            {
                AsyncOperation unload = SceneManager.UnloadSceneAsync(
                    sceneParameters_.sceneName,
                    UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);

                while (!unload.isDone)
                {
                    await Task.Delay(50);
                }
                LoadScene(sceneParameters_);
            }
            catch (Exception e)
            {
                print(e);
                sceneParameters_.loadSceneType = LoadSceneType.LoadSingle;
                LoadScene(sceneParameters_);
            }

            isOperating = false;
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
        
        public static bool IsSceneAlreadyActive(string sceneNameToLoad_)
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                var _sceneName = SceneManager.GetSceneAt(i).name;
                if (_sceneName == sceneNameToLoad_) return true;
            }
            return false;
        }

        private void RemoveNullEnablers()
        {
            sceneEnablerList.RemoveAll(s => s.IsEmptyOrDestroyed());
        }

        private async void UnloadAllExcept(LoadSceneParameters sceneParameters_)
        {
            var _sceneToActivate = SceneManager.GetSceneByName(sceneParameters_.sceneToActivate);
            if(!_sceneToActivate.IsValid()) _sceneToActivate = SceneManager.GetSceneAt(0);
            
            if (!_sceneToActivate.isLoaded)
            {
                sceneParameters_.loadSceneType = LoadSceneType.LoadSingle;
                sceneParameters_.sceneName = _sceneToActivate.name;
                LoadScene(sceneParameters_);
                return;
            }
            
            Time.timeScale = 0;
            currentFade = GetRandomFade();
            
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                var _sceneToUnload = SceneManager.GetSceneAt(i);
                if(_sceneToUnload.name == _sceneToActivate.name) continue;
                
                
                SceneEnabler enablerToRemove = sceneEnablerList.FirstOrDefault(s => s.scene.name == _sceneToUnload.name);

                sceneEnablerList.Remove(enablerToRemove);
                
                AsyncOperation unload = SceneManager.UnloadSceneAsync(
                    _sceneToUnload,
                    UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);

                unload.allowSceneActivation = true;
            }

            currentFade.gameObject.SetActive(true);

            await currentFade.StartTransition(false);
            
            SceneManager.SetActiveScene(_sceneToActivate);
        }
    }
}
