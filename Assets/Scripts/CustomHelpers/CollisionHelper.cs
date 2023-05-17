using UnityEngine;

namespace CustomHelpers
{
    public static class CollisionHelper
    {
        /// <summary>
        /// Similar To CompareTag() but uses internal hashing to be more efficient
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="Tag"></param>
        /// <returns></returns>
        public static bool CompareTagHash(this GameObject gameObject, string tag)
        {
            return gameObject.tag.ToHash() == tag.ToHash();
        }

        /// <summary>
        /// Similar To CompareTag() but uses internal hashing to be more efficient
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="Tag"></param>
        /// <returns></returns>
        public static bool CompareTagHash(this GameObject gameObject, int tagHash)
        {
            return gameObject.tag.ToHash() == tagHash;
        }

        /// <summary>
        /// Similar To CompareTag() but uses internal hashing to be more efficient
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="Tag"></param>
        /// <returns></returns>
        public static bool CompareTagHash(this Collider2D collider2D, string tag)
        {
            return collider2D.tag.ToHash() == tag.ToHash();
        }

        /// <summary>
        /// Similar To CompareTag() but uses internal hashing to be more efficient
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="Tag"></param>
        /// <returns></returns>
        public static bool CompareTagHash(this Collider collider, string tag)
        {
            return collider.tag.ToHash() == tag.ToHash();
        }

        /// <summary>
        /// Similar To CompareTag() but uses internal hashing to be more efficient
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="Tag"></param>
        /// <returns></returns>
        public static bool CompareTagHash(this Collider2D collider2D, int tagHash)
        {
            return collider2D.tag.ToHash() == tagHash;
        }

        /// <summary>
        /// Similar To CompareTag() but uses internal hashing to be more efficient
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="Tag"></param>
        /// <returns></returns>
        public static bool CompareTagHash(this Collider collider, int tagHash)
        {
            return collider.tag.ToHash() == tagHash;
        }

        /// <summary>
        /// Similar To CompareTag() but uses internal hashing to be more efficient
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="Tag"></param>
        /// <returns></returns>
        public static bool CompareTagHash(this Collider2D collider2D, string tag, out GameObject colliderGameObject)
        {
            colliderGameObject = collider2D.gameObject;
            return collider2D.tag.ToHash() == tag.ToHash();
        }

        /// <summary>
        /// Similar To CompareTag() but uses internal hashing to be more efficient
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="Tag"></param>
        /// <returns></returns>
        public static bool CompareTagHash(this Collider collider, string tag, out GameObject colliderGameObject)
        {
            colliderGameObject = collider.gameObject;
            return collider.tag.ToHash() == tag.ToHash();
        }

        /// <summary>
        /// Similar To CompareTag() but uses internal hashing to be more efficient
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="Tag"></param>
        /// <returns></returns>
        public static bool CompareTagHash(this Collider2D collider2D, int tagHash, out GameObject colliderGameObject)
        {
            colliderGameObject = collider2D.gameObject;
            return collider2D.tag.ToHash() == tagHash;
        }

        /// <summary>
        /// Similar To CompareTag() but uses internal hashing to be more efficient
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="Tag"></param>
        /// <returns></returns>
        public static bool CompareTagHash(this Collider collider, int tagHash, out GameObject colliderGameObject)
        {
            colliderGameObject = collider.gameObject;
            return collider.tag.ToHash() == tagHash;
        }

        public static bool IsLayerInLayerMask(int layer, LayerMask mask)
        {
            return (1 << layer & mask) != 0;
        }

        public static bool CompareLayer(this GameObject gameObject, LayerMask layerMask)
        {
            return IsLayerInLayerMask(gameObject.layer, layerMask);
        }

        public static bool CompareLayer(this Collider2D collider2D, LayerMask layerMask)
        {
            return IsLayerInLayerMask(collider2D.gameObject.layer, layerMask);
        }

        public static bool CompareLayer(this Collider collider, LayerMask layerMask)
        {
            return IsLayerInLayerMask(collider.gameObject.layer, layerMask);
        }

        public static bool CompareLayer(this Collider2D collider2D, LayerMask layerMask, out GameObject colliderGameObject)
        {
            colliderGameObject = collider2D.gameObject;
            return IsLayerInLayerMask(colliderGameObject.layer, layerMask);
        }

        public static bool CompareLayer(this Collider collider, LayerMask layerMask, out GameObject colliderGameObject)
        {
            colliderGameObject = collider.gameObject;
            return IsLayerInLayerMask(colliderGameObject.layer, layerMask);
        }
    }
}