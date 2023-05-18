using System;
using System.Collections;
using CustomEvent;
using DG.Tweening;
using Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CustomHelpers
{
    public class WaitForEvt : CustomYieldInstruction
    {
        private Evt _evt;
        private bool _isWaiting = true;
        private bool _isDisposed = false;

        public WaitForEvt(Evt evt)
        {
            _evt = evt;
            _evt.AddListener(OnEvt);
        }

        public override bool keepWaiting
        {
            get
            {
                if (_isDisposed)
                {
                    return false;
                }
                else
                {
                    return _isWaiting;
                }
            }
        }

        private void OnEvt()
        {
            if (!_isDisposed)
            {
                _evt.RemoveListener(OnEvt);
                _isWaiting = false;
            }
        }

        public void Dispose()
        {
            if (!_isDisposed)
            {
                _isDisposed = true;
                _evt.RemoveListener(OnEvt);
            }
        }
    }

    public class WaitForAction : CustomYieldInstruction
    {
        private readonly Action m_EventAction;
        private bool m_EventOccurred;

        public WaitForAction(Action eventAction)
        {
            m_EventAction = eventAction;
            m_EventOccurred = false;
        }

        public override bool keepWaiting
        {
            get { return !m_EventOccurred; }
        }

        public void EventOccurred()
        {
            m_EventOccurred = true;
        }

        public void StartWaiting()
        {
            m_EventOccurred = false;
            m_EventAction.Invoke();
        }
    }

    public class WaitForUIElement : CustomYieldInstruction
    {
        private GameObject uiElement;
        private bool waitForEnabled;

        public WaitForUIElement(GameObject uiElement, bool waitForEnabled = true)
        {
            this.uiElement = uiElement;
            this.waitForEnabled = waitForEnabled;
        }

        public override bool keepWaiting
        {
            get { return uiElement.activeSelf != waitForEnabled; }
        }
    }

    public class WaitForAudioClip : CustomYieldInstruction
    {
        private AudioSource audioSource;

        public WaitForAudioClip(AudioSource audioSource)
        {
            this.audioSource = audioSource;
        }

        public override bool keepWaiting
        {
            get { return audioSource.isPlaying; }
        }
    }

    public class WaitForDelegate : CustomYieldInstruction
    {
        private Func<bool> predicate;

        public WaitForDelegate(Func<bool> predicate)
        {
            this.predicate = predicate;
        }

        public override bool keepWaiting
        {
            get { return !predicate(); }
        }
    }

    public class WaitForSecondsRandom : CustomYieldInstruction
    {
        private float waitTime;

        public WaitForSecondsRandom(float minTime, float maxTime)
        {
            waitTime = Random.Range(minTime, maxTime);
        }

        public override bool keepWaiting
        {
            get { return Time.time < waitTime; }
        }
    }

    public class WaitForInputDown : CustomYieldInstruction
    {
        public override bool keepWaiting
        {
            get { return !Input.anyKeyDown; }
        }
    }

    public class WaitForTween : CustomYieldInstruction
    {
        private readonly Tween _tween;

        public WaitForTween(Tween tween)
        {
            _tween = tween;
        }

        public override bool keepWaiting => _tween != null && _tween.IsActive();
    }

    public class WaitForCondition : CustomYieldInstruction
    {
        private readonly Func<bool> _condition;

        public WaitForCondition(Func<bool> condition)
        {
            _condition = condition;
        }

        public override bool keepWaiting => !_condition();
    }
    
    public class WaitForFrames : CustomYieldInstruction
    {
        private int _framesToWait;

        public WaitForFrames(int framesToWait)
        {
            _framesToWait = framesToWait;
        }

        public override bool keepWaiting => _framesToWait-- > 0;
    }
    
    public class WaitForAnyInstruction : CustomYieldInstruction
    {
        private readonly CustomYieldInstruction[] instructions;

        public WaitForAnyInstruction(params CustomYieldInstruction[] instructions)
        {
            this.instructions = instructions;
        }

        public override bool keepWaiting
        {
            get
            {
                foreach (var _instruction in instructions)
                {
                    if (!_instruction.keepWaiting)
                    {
                        return false;
                    }
                }
                return true;
            }
        }
    }
    
    public class WaitForAnyInstructionWithCoroutine : CustomYieldInstruction
    {
        private readonly CustomYieldInstruction customInstruction;
        private readonly IEnumerator coroutine;

        public WaitForAnyInstructionWithCoroutine(CustomYieldInstruction customInstruction, IEnumerator coroutine)
        {
            this.customInstruction = customInstruction;
            this.coroutine = coroutine;
        }

        public override bool keepWaiting
        {
            get { return customInstruction.keepWaiting || (coroutine != null && coroutine.MoveNext()); }
        }
    }
    
    public class WaitForBothInstruction : CustomYieldInstruction
    {
        private readonly CustomYieldInstruction customInstruction;
        private readonly IEnumerator coroutine;
        private bool customInstructionFinished;
        private bool coroutineFinished;

        public WaitForBothInstruction(CustomYieldInstruction customInstruction, IEnumerator coroutine, MonoBehaviour mono)
        {
            this.customInstruction = customInstruction;
            this.coroutine = coroutine;
            customInstructionFinished = false;
            coroutineFinished = false;

            // Start a coroutine to check if the custom instruction finishes
            mono.StartCoroutine(CheckCustomInstructionFinished());
        }

        public override bool keepWaiting
        {
            get { return !customInstructionFinished || !coroutineFinished; }
        }

        private IEnumerator CheckCustomInstructionFinished()
        {
            yield return customInstruction;
            customInstructionFinished = true;
        }

        public IEnumerator CoroutineExample()
        {
            yield return new WaitForSeconds(3f);
            coroutineFinished = true;
        }
    }
}
