using FungusWrapper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDialogueTest : MonoBehaviour
{
    [SerializeField] private GameObject[] borders;

    private void Awake()
    {
        FungusReceiver.OnPlayerChoicesDisplayed.AddListener(SetVisibility);
    }

    public void SetVisibility(bool isActive)
    {
        borders[0].SetActive(isActive);
        borders[1].SetActive(isActive);
    }

}
