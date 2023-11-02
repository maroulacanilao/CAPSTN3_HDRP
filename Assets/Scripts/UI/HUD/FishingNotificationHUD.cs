using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FishingNotificationHUD : MonoBehaviour
{
    [SerializeField] private GameObject fishingNotification;
    [SerializeField] private GameObject fishingStatus;

    [SerializeField] private Sprite fishingImage;
    [SerializeField] private Sprite fishingImageFailed;

    private void OnEnable()
    {
        FishingManager.OnHookedFish.AddListener(DisplayText);
        FishingManager.OnFishingStarted.AddListener(DisplayStatus);
        FishingManager.OnCaughtFish.AddListener(DisplaySuccessImage);
    }

    private void OnDisable()
    {
        FishingManager.OnHookedFish.RemoveListener(DisplayText);
        FishingManager.OnFishingStarted.RemoveListener(DisplayStatus);
        FishingManager.OnCaughtFish.RemoveListener(DisplaySuccessImage);
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

    private void DisplayStatus(FishingManager manager)
    {
        if(FishingManager.Instance.hasFishingStarted == true)
        {
            fishingStatus.GetComponent<Image>().sprite = fishingImage;
            fishingStatus.gameObject.SetActive(true);
        }
    }

    private void DisplaySuccessImage(bool caughtFish)
    {
        fishingStatus.gameObject.SetActive(false);
        //if (caughtFish == false)
        //{
        //    fishingStatus.GetComponent<Image>().sprite = fishingImageFailed;
        //}
    }
}
