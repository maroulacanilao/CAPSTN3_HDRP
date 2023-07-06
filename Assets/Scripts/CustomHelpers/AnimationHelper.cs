using UnityEngine;
using System.Collections;

namespace CustomHelpers
{
    public class WaitForAnimationEvent : CustomYieldInstruction
    {
        private Animator animator;
        private string eventName;
        private bool eventTriggered;
        private float timeout;
        private float elapsedTime;

        public WaitForAnimationEvent(Animator animator, string eventName, float timeout = 5f)
        {
            this.animator = animator;
            this.eventName = eventName;
            this.eventTriggered = false;
            this.timeout = timeout;
            this.elapsedTime = 0f;

            AnimationEventReceiver eventReceiver = animator.GetComponent<AnimationEventReceiver>();
            if (eventReceiver != null)
            {
                eventReceiver.AddListener(eventName, OnAnimationEvent);
            }
            else
            {
                // Debug.LogError("AnimationEventReceiver component not found on animator " + animator.name);
            }
        }

        public override bool keepWaiting
        {
            get 
            {
                elapsedTime += Time.deltaTime;
                if (!eventTriggered && elapsedTime >= timeout)
                {
                    // Debug.LogWarningFormat("Animation event '{0}' not triggered within {1} seconds on {2}", eventName, timeout, animator.name);
                    eventTriggered = true;
                }
                return !eventTriggered;
            }
        }

        private void OnAnimationEvent()
        {
            eventTriggered = true;
            AnimationEventReceiver eventReceiver = animator.GetComponent<AnimationEventReceiver>();
            if (eventReceiver != null)
            {
                eventReceiver.RemoveListener(eventName, OnAnimationEvent);
            }
        }
    }
    
    public class WaitForAnimationEnd : CustomYieldInstruction
    {
        private readonly Animator animator;
        private readonly string animationName;
        private readonly float fallbackTime;
        private readonly float startTime;

        public override bool keepWaiting
        {
            get
            {
                if (animator == null || string.IsNullOrEmpty(animationName))
                {
                    Debug.LogError("Animator or animation name not set.");
                    return false;
                }

                if (!animator.GetCurrentAnimatorStateInfo(0).IsName(animationName))
                {
                    Debug.Log("Animation not playing.");
                    return false;
                }

                if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    Debug.Log("This Should Happen");
                    return true;
                }

                if (fallbackTime > 0f && Time.time < startTime + fallbackTime)
                {
                    Debug.Log("Fall back");
                    return true;
                }

                return false;
            }
        }

        public WaitForAnimationEnd(Animator animator_, string animationName_, float fallbackTime_ = 5f)
        {
            animator = animator_;
            animationName = animationName_;
            fallbackTime = fallbackTime_;
            startTime = Time.time;
        }
    }
}