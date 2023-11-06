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

    public void SetVisibility(bool isActive)
    {
        playerPortrait.SetActive(isActive);
        characterNameBG.SetActive(isActive);
    }
}
