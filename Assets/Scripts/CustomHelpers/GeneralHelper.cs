using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CustomHelpers
{
    public static class GeneralHelper
    {
        public static bool IsApproximatelyTo(this float a, float b)
        {
            return Mathf.Approximately(a, b);
        }
        
        public static bool IsApproximately0(this float a)
        {
            return Mathf.Approximately(a, 0);
        }

        /// <summary>
        /// 50% chance of returning true
        /// </summary>
        /// <returns></returns>
        public static bool RandomBool()
        {
            return Random.Range(0, 1f) < 0.5f;
        }

        public static int GetRandomInRange(this Vector2Int source)
        {
            return Random.Range(source.x, source.y + 1);
        }

        public static int GetRandomInRange(this Vector2Int source, float modifier)
        {
            float min = source.x * modifier;
            float max = source.y * modifier;
            var _val = Random.Range(min, max);
            return Mathf.RoundToInt(_val);
        }

        public static int EvaluateScaledCurve(this AnimationCurve curve_, int xToEvaluate_, int maxX_, int maxY_)
        {
            if (curve_ == null) return 0;
            
            var _scaledX = (float) xToEvaluate_ / maxX_;
            var _scaledY = curve_.Evaluate(_scaledX);
            return (int) (_scaledY * maxY_);
        }
        
        public static float EvaluateScaledCurve(this AnimationCurve curve_, float xToEvaluate_, float maxX_, float maxY_)
        {
            var _scaledX = xToEvaluate_ / maxX_;
            var _scaledY = curve_.Evaluate(_scaledX);
            return _scaledY * maxY_;
        }
        
        public static Color SetAlpha(this Color source, float alpha_)
        {
            return new Color(source.r, source.g, source.b, alpha_);
        }
        
        public static DateTime RoundToFive(this DateTime source)
        {
            var _clampedMinutes = (int)Math.Round((double)source.Minute / 5) * 5;
            return new DateTime(source.Year, 
                source.Month, 
                source.Day, 
                source.Hour, 
                _clampedMinutes, 
                0);
        }
        
        public static TimeSpan RoundToFive(this TimeSpan source)
        {
            int clampedMinutes = (int)Math.Round((double)source.Minutes / 5) * 5;
            
            return new TimeSpan(
                source.Days,
                source.Hours,
                clampedMinutes,
                0,
                0
            );
        }
        
        public static Color AddGrayishTint(this Color color, float strength)
        {
            // Convert color to HSV
            Color.RGBToHSV(color, out float h, out float s, out float v);

            // Reduce saturation and adjust brightness
            s *= (1f - strength);
            v *= (1f + strength * 0.5f);

            // Convert back to RGB
            Color grayishColor = Color.HSVToRGB(h, s, v);
            return grayishColor;
        }
    }
}