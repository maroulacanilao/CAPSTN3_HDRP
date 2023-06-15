using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderEditor : EditorWindow
{
    private string[] scenePaths;

    [MenuItem("Window/Scene Loader")]
    public static void ShowWindow()
    {
        var window = GetWindow<SceneLoaderEditor>("Scene Loader");
        window.minSize = new Vector2(250, 80);
    }

    private void OnEnable()
    {
        UpdateScenePaths();
    }

    private void OnGUI()
    {
        GUILayout.Label("Scene Path", EditorStyles.boldLabel);

        if (scenePaths == null || scenePaths.Length == 0)
        {
            GUILayout.Label("No scenes in build.", EditorStyles.miniLabel);
            return;
        }

        foreach (var scenePath in scenePaths)
        {
            if (GUILayout.Button(scenePath))
            {
                LoadScene(scenePath);
            }
        }
    }

    private void UpdateScenePaths()
    {
        int sceneCount = SceneManager.sceneCountInBuildSettings;
        scenePaths = new string[sceneCount];

        for (int i = 0; i < sceneCount; i++)
        {
            scenePaths[i] = SceneUtility.GetScenePathByBuildIndex(i);
        }
    }

    private void LoadScene(string scenePath)
    {
        if (!string.IsNullOrEmpty(scenePath))
        {
            if (EditorApplication.isPlaying)
            {
                // Load the scene in play mode
                UnityEngine.SceneManagement.SceneManager.LoadScene(scenePath);
            }
            else
            {
                // Load the scene in edit mode
                EditorSceneManager.OpenScene(scenePath);
            }
        }
    }
}