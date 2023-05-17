using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

namespace CustomHelpers
{
    // copied from Unity.VisualScripting.UnityObjectUtility
    public static class ObjectHelpers
    {
        public static bool IsDestroyed(this UnityEngine.Object target)
        {
            // Checks whether a Unity object is not actually a null reference,
            // but a rather destroyed native instance.

            return !ReferenceEquals(target, null) && target == null;
        }

        public static bool IsUnityNull(this object obj)
        {
            // Checks whether an object is null or Unity pseudo-null
            // without having to cast to UnityEngine.Object manually

            return obj == null || obj is UnityEngine.Object o && o == null;
        }

        public static bool IsUnityValid(this UnityEngine.Object obj)
        {
            return obj != null;
        }

        public static T AsUnityNull<T>(this T obj) where T : UnityEngine.Object
        {
            // Converts a Unity pseudo-null to a real null, allowing for coalesce operators.
            // e.g.: destroyedObject.AsUnityNull() ?? otherObject
            return obj == null ? null : obj;
        }

        public static bool TrulyEqual(UnityEngine.Object a, UnityEngine.Object b)
        {
            // This method is required when checking two references
            // against one another, where one of them might have been destroyed.
            // It is not required when checking against null.

            // This is because Unity does not compare alive state
            // in the CompareBaseObjects method unless one of the two
            // operators is actually the null literal.

            // From the source:
            /*
              bool lhsIsNull = (object) lhs == null;
              bool rhsIsNull = (object) rhs == null;
              if (rhsIsNull && lhsIsNull)
                return true;
              if (rhsIsNull)
                return !Object.IsNativeObjectAlive(lhs);
              if (lhsIsNull)
                return !Object.IsNativeObjectAlive(rhs);
              return lhs.m_InstanceID == rhs.m_InstanceID;
             */

            // As we can see, Object.IsNativeObjectAlive is not compared
            // across the two objects unless one of the operands is actually null.
            // But it can happen, for example when exiting play mode.
            // If we stored a static reference to a scene object that was destroyed,
            // the reference won't get cleared because assembly reloads don't happen
            // when exiting playmode. But the instance ID of the object will stay
            // the same, because it only gets reserialized. So if we compare our
            // stale reference that was destroyed to a new reference to the object,
            // it will return true, even though one reference is alive and the other isn't.

            if (a != b) return false;

            return a == null == (b == null);
        }

        public static IEnumerable<T> NotUnityNull<T>(this IEnumerable<T> enumerable) where T : UnityEngine.Object
        {
            return enumerable.Where(i => i != null);
        }

        public static IEnumerable<T> FindObjectsOfTypeIncludingInactive<T>()
        {
            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);

                if (!scene.isLoaded) continue;

                foreach (var rootGameObject in scene.GetRootGameObjects())
                foreach (var result in rootGameObject.GetComponentsInChildren<T>(true))
                    yield return result;
            }
        }

        public static T GetOrAddComponent<T>(this UnityEngine.GameObject gameObject_) where T : UnityEngine.Component
        {
            return gameObject_.TryGetComponent(out T _component) ? _component : gameObject_.AddComponent<T>();
        }
        
        public static T GetOrAddComponent<T>(this UnityEngine.Component component_) where T : UnityEngine.Component
        {
            return component_.gameObject.GetOrAddComponent<T>();
        }
    }
}