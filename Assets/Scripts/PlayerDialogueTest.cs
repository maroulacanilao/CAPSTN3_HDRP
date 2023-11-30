using FungusWrapper;
using System.Collections;
using System.Collections.Generic;
using Managers;
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
        if (borders != null)
        {
            foreach (GameObject o in borders)
            {
                if (o != null)
                {
                    o.SetActive(isActive);
                }
            }
        }
    }

    private void OnDestroy()
    {
        FungusReceiver.OnPlayerChoicesDisplayed.RemoveListener(SetVisibility);
    }
}
