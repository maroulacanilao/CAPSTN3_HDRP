using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CustomHelpers
{
    public static class SceneHelper
    {
        /// <summary>
        /// returns all given component in a specific scene
        /// Very expensive method. only use for caching
        /// </summary>
        /// <param name="includeInactive"></param>
        /// <param name="scene"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> FindComponentsInScene<T>(this Scene scene, bool includeInactive)
        {
            var results = new List<T>();

            foreach (var root in scene.GetRootGameObjects())
            {
                results.AddRange(root.GetComponents<T>());
                results.AddRange(root.GetComponentsInChildren<T>(includeInactive));
            }

            return results;
        }

        /// <summary>
        /// returns first found component in a specific scene
        /// Very expensive method. only use for caching
        /// </summary>
        /// <param name="includeInactive"></param>
        /// <param name="scene"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T FindFirstComponentInScene<T>(this Scene scene, bool includeInactive)
        {
            return scene.FindComponentsInScene<T>(includeInactive).FirstOrDefault();
        }

        /// <summary>
        /// Get All main cameras in a given scene
        /// Expensive method. only use for caching
        /// </summary>
        /// <param name="includeInActive"></param>
        /// <returns></returns>
        public static List<Camera> GetAllMainCamerasInScene(this Scene scene, bool includeInActive)
        {
            return includeInActive ?
                Camera.allCameras.Where(c => c.gameObject.scene == scene && c.gameObject.CompareTag("MainCamera")).ToList() :
                Camera.allCameras.Where(c =>
                {
                    GameObject gameObject;
                    return (gameObject = c.gameObject).scene == scene && gameObject.activeSelf && c.gameObject.CompareTag("MainCamera");
                }).ToList();
        }

        public static List<Camera> GetAllMainCamerasInScene(string sceneName, bool includeInActive)
        {
            return SceneManager.GetSceneByName(sceneName).GetAllMainCamerasInScene(includeInActive);
        }


        /// <summary>
        /// Get first found main camera in a given scene
        /// Expensive method. only use for caching
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="includeInActive"></param>
        /// <returns></returns>
        public static Camera GetFirstMainCameraInScene(this Scene scene, bool includeInActive = true)
        {
            return scene.GetAllMainCamerasInScene(includeInActive).FirstOrDefault();
        }

        public static Camera GetFirstMainCameraInScene(string sceneName, bool includeInActive = true)
        {
            return SceneManager.GetSceneByName(sceneName).GetFirstMainCameraInScene(includeInActive);
        }
    }
}