using System;
using BaseCore;
using Managers;
using NaughtyAttributes;
using ScriptableObjectData;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

// Source: https://dotnetfiddle.net/7jfc0O

public class MainLightManager : MonoBehaviour
{
    [SerializeField] LightingData data;
    
    [Header("Light")]
    [SerializeField] private Light mainLight;
    [SerializeField] private HDAdditionalLightData lightData;


    private int DayTime=> TimeManager.StartingHour; 
    private int NightTime => TimeManager.NightHour;
    private int DayDuration => NightTime - DayTime;


    protected void Awake()
    {
        TimeManager.OnMinuteTick.AddListener(UpdateLight);
    }
    
    protected void OnDestroy()
    {
        TimeManager.OnMinuteTick.RemoveListener(UpdateLight);
    }

    private void OnEnable()
    {
        UpdateLight();
    }

    private void UpdateLight()
    {
        var _time = TimeManager.GameTime / 10f;
        var _intensityScale = data.intensityCurve.Evaluate(_time);
        var _colorScale = data.colorCurve.Evaluate(_time);
        var _temperatureScale = data.temperatureCurve.Evaluate(_time);
        var _rotationScale = data.rotationCurve.Evaluate(_time);
        
        lightData.intensity = Mathf.Lerp(data.dayIntensity, data.nightIntensity, _intensityScale);
        lightData.color = Color.Lerp(data.dayLightColor, data.nightLightColor, _colorScale);
        mainLight.color = Color.Lerp(data.dayLightColor, data.nightLightColor, _colorScale);
        mainLight.colorTemperature = Mathf.Lerp(data.dayTemperature, data.nightTemperature, _temperatureScale);
        mainLight.transform.rotation = Quaternion.Euler(160, Mathf.Lerp(data.dayYRotationRange.x, data.nightYRotationRange.x, _rotationScale), 0);
    }

    private float ScaledTime()
    {
        var _scaledTime = TimeManager.GameTime - DayTime;
        _scaledTime /= DayDuration;
        return _scaledTime;
    }
}