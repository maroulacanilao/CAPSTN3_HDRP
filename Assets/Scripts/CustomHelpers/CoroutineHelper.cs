using System;
using System.Collections;
using System.Collections.Generic;
using CustomEvent;
using UnityEngine;

namespace CustomHelpers
{
    public static class CoroutineHelper
    {
        private static readonly Dictionary<float, WaitForSeconds> WaitDictionary =
            new Dictionary<float, WaitForSeconds>();

        public static WaitForSeconds GetWait(float time)
        {
            if (WaitDictionary.TryGetValue(time, out var wait)) return wait;

            WaitDictionary[time] = new WaitForSeconds(time);
            return WaitDictionary[time];
        }

        public static CustomYieldInstruction WaitForEvt(this Evt evt_)
        {
            return new WaitForEvt(evt_);
        }
        
        public static CustomYieldInstruction WaitForAction(this Action action_)
        {
            return new WaitForAction(action_);
        }
        
        public static CustomYieldInstruction WaitForAnimationEnd(this Animator animator_, string animationName_, float fallbackTime = 0)
        {
            return new WaitForAnimationEnd(animator_, animationName_, fallbackTime);
        }
        
        public static CustomYieldInstruction WaitForAnimationEvent(this Animator animator_, string eventName_, float fallbackTime = 5)
        {
            return new WaitForAnimationEvent(animator_, eventName_, fallbackTime);
        }
    }
}
