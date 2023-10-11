using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FishingNotificationHUD : MonoBehaviour
{
    [SerializeField] private GameObject fishingNotification;

    private void OnEnable()
    {
        FishingManager.OnHookedFish.AddListener(DisplayText);
    }

    private void OnDisable()
    {
        FishingManager.OnHookedFish.RemoveListener(DisplayText);
    }
    
    private void DisplayText(FishingManager manager)
    {
        if (FishingManager.Instance.fishOnHook == true)
        {
            fishingNotification.gameObject.SetActive(true);
        }
        else
        {
            fishingNotification.SetActive(false);
        }
    }
}
