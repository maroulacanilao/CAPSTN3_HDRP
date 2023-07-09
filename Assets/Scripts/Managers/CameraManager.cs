using System.Collections.Generic;
using System.Linq;
using BaseCore;
using Cinemachine;
using UnityEngine;

namespace Managers
{
    public class CameraManager : Singleton<CameraManager>
    {
        private readonly Dictionary<string, PlayerCamera> playerCameraDict = new Dictionary<string, PlayerCamera>();
        
        public static void RegisterPlayerCamera(string key_, PlayerCamera cam_)
        {
            if(IsInstanceNull()) return;
            
            if (Instance.playerCameraDict.ContainsKey(key_))
            {
                Debug.LogError($"Camera with key {key_} already exists!");
                return;
            }

            Instance.playerCameraDict.Add(key_, cam_);
        }
        
        public static void UnRegisterPlayerCamera(string key_)
        {
            if(IsInstanceNull()) return;
            
            if (!Instance.playerCameraDict.ContainsKey(key_))
            {
                Debug.LogError($"Camera with key {key_} does not exist!");
                return;
            }
            
            Instance.playerCameraDict.Remove(key_);
        }
        
        public static void SetPlayerCameraActive(string key_, bool active_)
        {
            if(IsInstanceNull()) return;
            
            if (!Instance.playerCameraDict.ContainsKey(key_))
            {
                Debug.LogError($"Camera with key {key_} does not exist!");
                return;
            }
            
            Instance.playerCameraDict[key_].gameObject.SetActive(active_);
        }
        
        public static void ActivePlayerCamera(string key_)
        {
            if(IsInstanceNull()) return;
            
            if (!Instance.playerCameraDict.ContainsKey(key_))
            {
                Debug.LogError($"Camera with key {key_} does not exist!");
                return;
            }
            
            foreach (var _cam in Instance.playerCameraDict)
            {
                _cam.Value.gameObject.SetActive(_cam.Key == key_);
            }
        }
        
        public static void ActivePlayerCameraOnActiveScene()
        {
            if(IsInstanceNull()) return;
            
            var _activeScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            if (!Instance.playerCameraDict.ContainsKey(_activeScene))
            {
                Debug.LogError($"Camera with key {_activeScene} does not exist!");
                return;
            }
            
            ActivePlayerCamera(_activeScene);
        }
        
        public static PlayerCamera GetActivePlayerCamera()
        {
            if(IsInstanceNull()) return null;
            
            return (from _cam in Instance.playerCameraDict 
                where _cam.Value.gameObject.activeInHierarchy 
                select _cam.Value)
                .FirstOrDefault();
        }
        
        public static PlayerCamera GetPlayerCamera(string key_)
        {
            if(IsInstanceNull()) return null;
            
            if (Instance.playerCameraDict.TryGetValue(key_, out var _camera)) return _camera;
            Debug.LogError($"Camera with key {key_} does not exist!");
            return null;
        }
        
        public static PlayerCamera GetPlayerCameraOnActiveScene()
        {
            if(IsInstanceNull()) return null;
            
            var _activeScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            if (Instance.playerCameraDict.TryGetValue(_activeScene, out var _camera)) return _camera;
            Debug.LogError($"Camera with key {_activeScene} does not exist!");
            return null;
        }
    }
}
