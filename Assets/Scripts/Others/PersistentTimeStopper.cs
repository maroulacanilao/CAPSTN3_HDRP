using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;

public class PersistentTimeStopper : TimeStopper
{
    private void Update()
    {
        if (!TimeManager.IsTimePaused)
        {
            TimeManager.PauseTime(pauseTimeManagerScale);
        }
    }
}
