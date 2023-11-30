using UnityEngine;
using FungusWrapper;
using UnityEngine.UI;
using CustomHelpers;

public class PlayerImageTest : MonoBehaviour
{
    [SerializeField] private GameObject playerPortrait;
    [SerializeField] private GameObject characterNameBG;

    private void Awake()
    {
        FungusReceiver.OnTutorialDialogueEnabled.AddListener(SetVisibility);
    }

    private void OnDestroy()
    {
        FungusReceiver.OnTutorialDialogueEnabled.RemoveListener(SetVisibility);
    }

    public void SetVisibility(bool isActive)
    {
        if (playerPortrait != null && characterNameBG != null)
        {
            playerPortrait.SetActive(isActive);
            characterNameBG.SetActive(isActive);
        }
    }
}
