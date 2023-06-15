using UnityEngine;

namespace BaseCore
{
    public static class Bootstrapper
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Execute()
        {
            // Instantiate Game Manager Before First Scene Is Loaded
            Object.DontDestroyOnLoad(Object.Instantiate(Resources.Load("Prefabs/GameManager")));
        }
    }
}