using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;

public class SunController : MonoBehaviour
{
    [SerializeField] private Light sun;
    [SerializeField] private Light moon;
    [SerializeField] private float yRotation = -45f;
    private void Awake()
    {
        TimeManager.OnMinuteTick.AddListener(UpdateRotation);
        TimeManager.OnHourTick.AddListener(CheckNight);
        moon.gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        TimeManager.OnMinuteTick.AddListener(UpdateRotation);
        TimeManager.OnHourTick.AddListener(CheckNight);
    }

    private void UpdateRotation()
    {
        float _alpha = TimeManager.GameTime / 24f;
        float _sunRotation = Mathf.Lerp(-90, 270, _alpha);
        float _moonRotation = _sunRotation - 180;
        
        sun.transform.localRotation = Quaternion.Euler(_sunRotation, yRotation, 0);
        moon.transform.localRotation = Quaternion.Euler(_moonRotation, yRotation, 0);
    }
    
    private void CheckNight()
    {
        switch (TimeManager.CurrentHour)
        {
            case 6:
                StartDay();
                break;
            case 18:
                StartNight();
                break;
            default: break;
        }
    }

    private void StartDay()
    {
        sun.shadows = LightShadows.Soft;
        moon.shadows = LightShadows.None;
    }
    
    private void StartNight()
    {
        sun.shadows = LightShadows.None;
        moon.shadows = LightShadows.Soft;
    }
}
