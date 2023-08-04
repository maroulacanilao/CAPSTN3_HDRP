#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

public class UnusedAssetsRemover : EditorWindow
{
    private string targetFolderName = "";

    [MenuItem("Custom/Delete Unused Assets in Folder")]
    private static void Init()
    {
        UnusedAssetsRemover window = (UnusedAssetsRemover)EditorWindow.GetWindow(typeof(UnusedAssetsRemover));
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Label("Unused Assets Remover", EditorStyles.boldLabel);
        
        targetFolderName = EditorGUILayout.TextField("Target Folder Name:", targetFolderName);

        if (GUILayout.Button("Delete Unused Assets"))
        {
            DeleteUnusedAssetsInFolder();
        }
    }

    private void DeleteUnusedAssetsInFolder()
    {
        string targetFolderPath = "Assets/" + targetFolderName;
        string[] allAssetPaths = AssetDatabase.GetAllAssetPaths();
        int deletedCount = 0;

        foreach (string assetPath in allAssetPaths)
        {
            if (assetPath.StartsWith(targetFolderPath) && !AssetDatabase.IsValidFolder(assetPath) && !IsAssetUsed(assetPath))
            {
                string assetName = System.IO.Path.GetFileName(assetPath);
                AssetDatabase.DeleteAsset(assetPath);
                deletedCount++;
                Debug.Log("Deleted unused asset: " + assetName + " (" + assetPath + ")");
            }
        }

        Debug.Log("Deleted " + deletedCount + " unused assets in folder: " + targetFolderPath);
    }

    private static bool IsAssetUsed(string assetPath)
    {
        string[] dependencies = AssetDatabase.GetDependencies(assetPath, false);
        return dependencies.Length > 1; // Check if the asset has dependencies other than itself
    }
}



#endif