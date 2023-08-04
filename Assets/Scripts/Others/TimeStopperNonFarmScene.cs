using System.Collections;
using System.Collections.Generic;
using CustomHelpers;
using Managers;
using NaughtyAttributes;
using UnityEngine;

public class TimeStopperNonFarmScene : MonoBehaviour
{
    private void OnEnable()
    {
        if(TimeManager.Instance.IsEmptyOrDestroyed()) return;
        var _willStopTimeScale = !GameManager.IsFarmOrTutorialSceneActive();
        TimeManager.PauseTime(_willStopTimeScale);
    }

    private void OnDisable()
    {
        if(TimeManager.Instance.IsEmptyOrDestroyed()) return; 
        TimeManager.ResumeTime();
    }
}
