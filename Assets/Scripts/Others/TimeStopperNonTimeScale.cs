using System;
using System.Collections;
using System.Collections.Generic;
using CustomHelpers;
using Managers;
using UnityEngine;

public class TimeStopperNonTimeScale : MonoBehaviour
{
    private void OnEnable()
    {
        if(TimeManager.Instance.IsEmptyOrDestroyed()) return;
        TimeManager.PauseTime(false);
    }
    
    private void OnDisable()
    {
        if(TimeManager.Instance.IsEmptyOrDestroyed()) return; 
        TimeManager.ResumeTime();
    }
}
