using UnityEngine;

namespace CustomHelpers
{
    public static class TransformExtensions
    {
        /// <summary>
        /// Resets position, scale, and rotation of transform 
        /// </summary>
        /// <param name="transform"></param>
        /// <returns></returns>
        public static Transform ResetTransformation(this Transform transform)
        {
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;
            transform.localScale = Vector3.one;
            return transform;
        }

        public static Transform ResetLocalTransform(this Transform transform_)
        {
            transform_.localPosition = Vector3.zero;
            transform_.localRotation = Quaternion.identity;
            transform_.localScale = Vector3.one;
            return transform_;
        }
        
        public static Transform ResetRotation(this Transform transform)
        {
            transform.localRotation = Quaternion.identity;
            return transform;
        }

        /// <summary>
        /// Add current position to given Position
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="Z"></param>
        public static Transform AddPosition(this Transform transform, float X, float Y, float Z = 0)
        {
            transform.position = transform.position.Add(X, Y, Z);
            return transform;
        }

        /// <summary>
        /// Add current X position to a given value
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="X"></param>
        /// <returns></returns>
        public static Transform AddPositionX(this Transform transform, float X)
        {
            transform.position = transform.position.AddX(X);
            return transform;
        }

        /// <summary>
        /// Add current Y position to a given value
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="Y"></param>
        /// <returns></returns>
        public static Transform AddPositionY(this Transform transform, float Y)
        {
            transform.position = transform.position.AddY(Y);
            return transform;
        }

        /// <summary>
        /// Add current Z position to a given value
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="Z"></param>
        /// <returns></returns>
        public static Transform AddPositionZ(this Transform transform, float Z)
        {
            transform.position = transform.position.AddZ(Z);
            return transform;
        }

        /// <summary>
        /// Teleports transform to a given Position
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="Z"></param>
        public static Transform SetPosition(this Transform transform, float X, float Y, float Z = 0)
        {
            transform.position = new Vector3(X, Y, Z);
            return transform;
        }

        /// <summary>
        /// Teleports transform to a given X position while preserving Y and Z
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="X"></param>
        /// <returns></returns>
        public static Transform SetX(this Transform transform, float X)
        {
            transform.position = transform.position.SetX(X);
            return transform;
        }

        /// <summary>
        /// Teleports transform to a given Y position while preserving X and Z
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="Y"></param>
        /// <returns></returns>
        public static Transform SetY(this Transform transform, float Y)
        {
            transform.position = transform.position.SetY(Y);
            return transform;
        }

        /// <summary>
        /// Teleports transform to a given Z position while preserving X and Y
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="Z"></param>
        /// <returns></returns>
        public static Transform SetZ(this Transform transform, float Z)
        {
            transform.position = transform.position.SetZ(Z);
            return transform;
        }
    }
}