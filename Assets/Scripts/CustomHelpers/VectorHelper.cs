using UnityEngine;

namespace CustomHelpers
{
    public static class VectorExtensions
    {
        public static Vector3 AddX(this Vector3 position, float value)
        {
            return new Vector3(position.x + value, position.y, position.z);
        }

        public static Vector3 AddY(this Vector3 position, float value)
        {
            return new Vector3(position.x, position.y + value, position.z);
        }

        public static Vector3 AddZ(this Vector3 position, float value)
        {
            return new Vector3(position.x, position.y, position.z + value);
        }

        public static Vector2 AddX(this Vector2 position, float value)
        {
            return new Vector2(position.x + value, position.y);
        }

        public static Vector2 AddY(this Vector2 position, float value)
        {
            return new Vector2(position.x, position.y + value);
        }

        public static Vector3 SetX(this Vector3 position, float value)
        {
            return new Vector3(value, position.y, position.z);
        }

        public static Vector3 SetY(this Vector3 position, float value)
        {
            return new Vector3(position.x, value, position.z);
        }

        public static Vector2 SetX(this Vector2 position, float value)
        {
            return new Vector2(value, position.y);
        }

        public static Vector2 SetY(this Vector2 position, float value)
        {
            return new Vector2(position.x, value);
        }

        public static Vector3 SetZ(this Vector3 position, float value)
        {
            return new Vector3(position.x, position.y, value);
        }

        public static Vector3 Add(this Vector3 position, float x_ToAdd, float y_ToAdd, float z_ToAdd = 0)
        {
            return new Vector3(position.x + x_ToAdd, position.y + y_ToAdd, position.z + z_ToAdd);
        }

        public static Vector2 Add(this Vector2 position, float x_ToAdd, float y_ToAdd)
        {
            return new Vector2(position.x + x_ToAdd, position.y + y_ToAdd);
        }

        public static Vector2 AddToAll(this Vector2 position, float toAddAll)
        {
            return new Vector2(position.x + toAddAll, position.y + toAddAll);
        }

        public static Vector3 AddToAll(this Vector3 position, float toAddAll)
        {
            return new Vector3(position.x + toAddAll, position.y + toAddAll, position.z + toAddAll);
        }

        public static Vector3 Add(this Vector3 position, Vector3 toAdd)
        {
            return position + toAdd;
        }
        
        public static Vector3 Multiply(this Vector3 position, float x, float y, float z)
        {
            return new Vector3(position.x * x, position.y * y, position.z * z);
        }

        public static Vector3 Multiply(this Vector3 position, Vector3 other)
        {
            return Multiply(position, other.x, other.y, other.z);
        }

        public static Vector3 Clamp(this Vector3 postion, Vector3 min, Vector3 max)
        {
            postion.x = Mathf.Clamp(postion.x, min.x, max.x);
            postion.y = Mathf.Clamp(postion.y, min.y, max.y);
            postion.z = Mathf.Clamp(postion.z, min.z, max.z);

            return postion;
        }

        public static float Remap(this float f, float fromMin, float fromMax, float toMin, float toMax)
        {
            float t = (f - fromMin) / (fromMax - fromMin);
            return Mathf.LerpUnclamped(toMin, toMax, t);
        }
    }
}