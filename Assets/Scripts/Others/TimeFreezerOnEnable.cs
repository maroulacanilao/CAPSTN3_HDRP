using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;

public class TimeFreezerOnEnable : MonoBehaviour
{
    private void OnEnable()
    {
        TimeManager.PauseTime();
    }

    private void OnDisable()
    {
        TimeManager.ResumeTime();
    }
}
