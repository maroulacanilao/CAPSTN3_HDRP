using System.Collections.Generic;
using CustomHelpers;
using Fungus;
using UnityEngine;

namespace FungusWrapper
{
    public class FungusSetter : MonoBehaviour
    {
        public static List<FungusSetter> fungusComponents = new List<FungusSetter>();

        private void Awake()
        {
            fungusComponents.Add(this);
            var _sayDialog = GetComponentInChildren<SayDialog>();
        }

        private void OnDestroy()
        {
            fungusComponents.Remove(this);
        }

        private static void Purge()
        {
            for (int i = fungusComponents.Count - 1; i >= 0; i--)
            {
                if(fungusComponents[i].IsEmptyOrDestroyed()) fungusComponents.RemoveAt(i);
            }
        }

        public static bool IsOpen()
        {
            Purge();
            if(fungusComponents.Count == 0) return false;
            foreach (var component in fungusComponents)
            {
                if(component.IsEmptyOrDestroyed()) continue;
                if (component.gameObject.activeInHierarchy)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
