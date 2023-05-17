using UnityEngine;

namespace CustomHelpers
{
    public static class RigidBodyHelper
    {
        #region RigidBody2D

        public static void SetVelocity(this Rigidbody2D rigidbody2D, Vector2 velocity)
        {
            rigidbody2D.velocity = velocity;
        }

        public static void SetVelocity(this Rigidbody2D rigidbody2D, float xVelocity, float yVelocity)
        {
            rigidbody2D.velocity = new Vector2(xVelocity, yVelocity);
        }

        public static void SetVelocityX(this Rigidbody2D rigidbody2D, float xVelocity)
        {
            rigidbody2D.velocity = rigidbody2D.velocity.SetX(xVelocity);
        }

        public static void SetVelocityY(this Rigidbody2D rigidbody2D, float yVelocity)
        {
            rigidbody2D.velocity = rigidbody2D.velocity.SetY(yVelocity);
        }


        public static void ResetVelocity(this Rigidbody2D rigidbody2D)
        {
            rigidbody2D.velocity = Vector2.zero;
        }

        #endregion

        #region RigidBody3D

        public static void SetVelocity(this Rigidbody rigidbody, Vector3 velocity)
        {
            rigidbody.velocity = velocity;
        }

        public static void SetVelocity(this Rigidbody rigidbody, float xVelocity, float zVelocity)
        {
            rigidbody.velocity = new Vector3(xVelocity, rigidbody.velocity.y, zVelocity);
        }

        public static void SetVelocity(this Rigidbody rigidbody, float xVelocity, float yVelocity, float zVelocity)
        {
            rigidbody.velocity = new Vector3(xVelocity, yVelocity, zVelocity);
        }

        public static void SetVelocityX(this Rigidbody rigidbody, float xVelocity)
        {
            rigidbody.velocity = new Vector3(xVelocity, rigidbody.velocity.y, rigidbody.velocity.z);
        }

        public static void SetVelocityY(this Rigidbody rigidbody, float yVelocity)
        {
            rigidbody.velocity = new Vector3(rigidbody.velocity.x, yVelocity, rigidbody.velocity.z);
        }

        public static void SetVelocityZ(this Rigidbody rigidbody, float zVelocity)
        {
            rigidbody.velocity = new Vector3(rigidbody.velocity.x, rigidbody.velocity.y, zVelocity);
        }

        public static void ResetVelocity(this Rigidbody rigidbody)
        {
            rigidbody.velocity = Vector3.zero;
        }

        #endregion
    }
}