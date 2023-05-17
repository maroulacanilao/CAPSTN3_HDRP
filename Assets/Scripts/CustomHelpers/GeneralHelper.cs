using System.Collections;
using UnityEngine;

namespace CustomHelpers
{
    public static class GeneralHelper
    {
        public static bool IsApproximatelyTo(this float a, float b)
        {
            return Mathf.Approximately(a, b);
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
    }
}