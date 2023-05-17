using System;
using BaseCore;
using Managers;
using NaughtyAttributes;
using UnityEngine;

// Source: https://dotnetfiddle.net/7jfc0O

public class MainLightManager : Singleton<MainLightManager>
{
    [Header("Day")]
    [SerializeField] private Light sunLight;
    [SerializeField] private Color dayLightColor;
    [SerializeField] private float maxSunLightIntensity;
    [SerializeField] private float sunriseHour;
    [SerializeField] private float sunsetHour;

    [Header("Night")]
    [SerializeField] private Light moonLight;
    [SerializeField] private Color nightLightColor;
    [SerializeField] private float maxMoonLightIntensity;
    
    [Header("Light Curve")]
    [SerializeField] [CurveRange(0,0,1,1,EColor.Yellow)]
    private AnimationCurve lightChangeCurve;
    
    private TimeSpan sunriseTime;

    private TimeSpan sunsetTime;

    protected void Start()
    {
        sunriseTime = TimeSpan.FromHours(sunriseHour);
        sunsetTime = TimeSpan.FromHours(sunsetHour);
        TimeManager.OnMinuteTick.AddListener(RotateSun);
        RotateSun(TimeManager.Instance);
    }

    private void RotateSun(TimeManager timeManager_)
    {
        float _sunLightRotation;
        
        var _timeOfDay = TimeManager.DateTime.TimeOfDay;
        
        if (_timeOfDay > sunriseTime && _timeOfDay < sunsetTime)
        {
            var _sunriseToSunsetDuration = CalculateTimeDifference(sunriseTime, sunsetTime);
            var _timeSinceSunrise = CalculateTimeDifference(sunriseTime, _timeOfDay);

            var _percentage = _timeSinceSunrise.TotalMinutes / _sunriseToSunsetDuration.TotalMinutes;

            _sunLightRotation = Mathf.Lerp(0, 180, (float) _percentage);
        }
        else
        {
            var _sunsetToSunriseDuration = CalculateTimeDifference(sunsetTime, sunriseTime);
            var _timeSinceSunset = CalculateTimeDifference(sunsetTime, _timeOfDay);

            var _percentage = _timeSinceSunset.TotalMinutes / _sunsetToSunriseDuration.TotalMinutes;

            _sunLightRotation = Mathf.Lerp(180, 360, (float) _percentage);
        }

        sunLight.transform.rotation = Quaternion.AngleAxis(_sunLightRotation, Vector3.right);
        
        UpdateLightSettings();
    }

    private void UpdateLightSettings()
    {
        float _dotProduct = Vector3.Dot(sunLight.transform.forward, Vector3.down);
        sunLight.intensity = Mathf.Lerp(0, maxSunLightIntensity, lightChangeCurve.Evaluate(_dotProduct));
        moonLight.intensity = Mathf.Lerp(maxMoonLightIntensity, 0, lightChangeCurve.Evaluate(_dotProduct));
        RenderSettings.ambientLight = Color.Lerp(nightLightColor, dayLightColor, lightChangeCurve.Evaluate(_dotProduct));
    }

    private TimeSpan CalculateTimeDifference(TimeSpan fromTime, TimeSpan toTime)
    {
        TimeSpan _difference = toTime - fromTime;

        if (_difference.TotalSeconds < 0)
        {
            _difference += TimeSpan.FromHours(24);
        }

        return _difference;
    }
}