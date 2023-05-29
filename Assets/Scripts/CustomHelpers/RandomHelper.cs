using UnityEngine;

namespace CustomHelpers
{
    public static class RandomHelper
    {
        public static Vector3 RandomPointInBounds(this Bounds bounds)
        {
            float x = UnityEngine.Random.Range(bounds.min.x, bounds.max.x);
            float y = UnityEngine.Random.Range(bounds.min.y, bounds.max.y);
            float z = UnityEngine.Random.Range(bounds.min.z, bounds.max.z);

            return new Vector3(x, y, z);
        }
        
        /// <summary>
        /// probability range should be between 0 and 1
        /// </summary>
        /// <param name="probability_"></param>
        /// <returns></returns>
        public static bool RandomBool(float probability_)
        {
            return Random.value < probability_;
        }
    }
}