using UnityEngine;

[System.Serializable]
public class EncyclopediaInfo
{
     [TextArea(10,15)] public string description;
     [NaughtyAttributes.ShowAssetPreview()] public Sprite sprite;
}
