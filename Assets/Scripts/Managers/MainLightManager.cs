using System;
using BaseCore;
using Managers;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

// Source: https://dotnetfiddle.net/7jfc0O

public class MainLightManager : Singleton<MainLightManager>
{
    [Header("Light")]
    [SerializeField] private Light mainLight;
    [SerializeField] private HDAdditionalLightData lightData;
    
    [Header("Day")]
    [SerializeField] private Color dayLightColor;
    [SerializeField] private float dayTemperature;
    [SerializeField] private float dayIntensity;
    
    [Header("Day")]
    [SerializeField] private Color nightLightColor;
    [SerializeField] private float nightTemperature;
    [SerializeField] private float nightIntensity;
    
    [Header("Curves")]
    [SerializeField] private AnimationCurve intensityCurve;
    [SerializeField] private AnimationCurve colorCurve;
    [SerializeField] private AnimationCurve temperatureCurve;
    
    
    private int DayTime=> TimeManager.StartingHour; 
    private int NightTime => TimeManager.NightHour;
    private int DayDuration => NightTime - DayTime;


    protected override void Awake()
    {
        base.Awake();
        TimeManager.OnMinuteTick.AddListener(UpdateLight);
    }

    private void UpdateLight(TimeManager timeManager_)
    {
        bool isDay = TimeManager.GameTime >= DayTime && TimeManager.GameTime < NightTime;
        var _scaledTime = isDay ? ScaledTime() : 1;
        var _intensityScale = intensityCurve.Evaluate(_scaledTime);
        var _colorScale = colorCurve.Evaluate(_scaledTime);
        var _temperatureScale = temperatureCurve.Evaluate(_scaledTime);

        lightData.intensity = Mathf.Lerp(dayIntensity, nightIntensity, _intensityScale);
        lightData.color = Color.Lerp(dayLightColor, nightLightColor, _colorScale);
        mainLight.color = Color.Lerp(dayLightColor, nightLightColor, _colorScale);
        mainLight.colorTemperature = Mathf.Lerp(dayTemperature, nightTemperature, _temperatureScale);
    }

    private float ScaledTime()
    {
        var _scaledTime = TimeManager.GameTime - DayTime;
        _scaledTime /= DayDuration;
        return _scaledTime;
    }
}