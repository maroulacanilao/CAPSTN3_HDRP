using System.Collections.Generic;
using System.Linq;
using BaseCore;
using Cinemachine;
using UnityEngine;

namespace Managers
{
    public class CameraManager : Singleton<CameraManager>
    {
        private readonly Dictionary<string, ICinemachineCamera> playerCameraDict = new Dictionary<string, ICinemachineCamera>();
        
        public void RegisterPlayerCamera(string key_, ICinemachineCamera cam_)
        {
            if (playerCameraDict.ContainsKey(key_))
            {
                Debug.LogError($"Camera with key {key_} already exists!");
                return;
            }
            
            playerCameraDict.Add(key_, cam_);
        }
        
        public void UnRegisterPlayerCamera(string key_)
        {
            if (!playerCameraDict.ContainsKey(key_))
            {
                Debug.LogError($"Camera with key {key_} does not exist!");
                return;
            }
            
            playerCameraDict.Remove(key_);
        }
        
        public void SetPlayerCameraActive(string key_, bool active_)
        {
            if (!playerCameraDict.ContainsKey(key_))
            {
                Debug.LogError($"Camera with key {key_} does not exist!");
                return;
            }
            
            playerCameraDict[key_].VirtualCameraGameObject.SetActive(active_);
        }
        
        public void ActivePlayerCamera(string key_)
        {
            if (!playerCameraDict.ContainsKey(key_))
            {
                Debug.LogError($"Camera with key {key_} does not exist!");
                return;
            }
            
            foreach (var _cam in playerCameraDict)
            {
                _cam.Value.VirtualCameraGameObject.SetActive(_cam.Key == key_);
            }
        }
        
        public void ActivePlayerCameraOnActiveScene()
        {
            var _activeScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            if (!playerCameraDict.ContainsKey(_activeScene))
            {
                Debug.LogError($"Camera with key {_activeScene} does not exist!");
                return;
            }
            
            ActivePlayerCamera(_activeScene);
        }
        
        public ICinemachineCamera GetActivePlayerCamera()
        {
            return (from _cam in playerCameraDict 
                where _cam.Value.VirtualCameraGameObject.activeInHierarchy 
                select _cam.Value)
                .FirstOrDefault();
        }
        
        public ICinemachineCamera GetPlayerCamera(string key_)
        {
            if (playerCameraDict.TryGetValue(key_, out var _camera)) return _camera;
            Debug.LogError($"Camera with key {key_} does not exist!");
            return null;
        }
        
        public ICinemachineCamera GetPlayerCameraOnActiveScene()
        {
            var _activeScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            if (playerCameraDict.TryGetValue(_activeScene, out var _camera)) return _camera;
            Debug.LogError($"Camera with key {_activeScene} does not exist!");
            return null;
        }
    }
}
