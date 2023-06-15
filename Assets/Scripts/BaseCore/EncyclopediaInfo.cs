using UnityEngine;

[System.Serializable]
public class EncyclopediaInfo
{
     [NaughtyAttributes.ResizableTextArea] public string description;
     [NaughtyAttributes.ShowAssetPreview()] public Sprite sprite;
}
