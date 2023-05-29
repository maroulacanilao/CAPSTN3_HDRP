using UnityEngine;
using UnityEngine;
using UnityEngine.EventSystems;
using Fungus;
using UnityEngine.InputSystem.UI;

namespace UI.FungusWrapper
{
    // Copy of DialogInput from FUngus with the new InputSystem instead of StandaloneInputModule
    
    /// <summary>
    /// Supported modes for clicking through a Say Dialog.
    /// </summary>
    public enum ClickMode
    {
        /// <summary> Clicking disabled. </summary>
        Disabled,
        /// <summary> Click anywhere on screen to advance. </summary>
        ClickAnywhere,
        /// <summary> Click anywhere on Say Dialog to advance. </summary>
        ClickOnDialog,
        /// <summary> Click on continue button to advance. </summary>
        ClickOnButton
    }

    /// <summary>
    /// Input handler for say dialogs.
    /// </summary>
    public class DialogInputSystem : MonoBehaviour
    {
        [Tooltip("Click to advance story")]
        [SerializeField] protected ClickMode clickMode;

        [Tooltip("Delay between consecutive clicks. Useful to prevent accidentally clicking through story.")]
        [SerializeField] protected float nextClickDelay = 0f;

        [Tooltip("Allow holding Cancel to fast forward text")]
        [SerializeField] protected bool cancelEnabled = true;

        [Tooltip("Ignore input if a Menu dialog is currently active")]
        [SerializeField] protected bool ignoreMenuClicks = true;

        protected bool dialogClickedFlag;

        protected bool nextLineInputFlag;

        protected float ignoreClickTimer;

        protected InputSystemUIInputModule uiInput;

        protected Writer writer;

        protected virtual void Awake()
        {
            writer = GetComponent<Writer>();

            CheckEventSystem();
        }

        // There must be an Event System in the scene for Say and Menu input to work.
        // This method will automatically instantiate one if none exists.
        protected virtual void CheckEventSystem()
        {
            EventSystem eventSystem = GameObject.FindObjectOfType<EventSystem>();
            if (eventSystem == null)
            {
                // Auto spawn an Event System from the prefab
                GameObject prefab = Resources.Load<GameObject>("Prefabs/EventSystem");
                if (prefab != null)
                {
                    GameObject go = Instantiate(prefab) as GameObject;
                    go.name = "EventSystem";
                }
            }
        }
            
        protected virtual void Update()
        {
            if (EventSystem.current == null)
            {
                return;
            }

            if (uiInput == null)
            {
                uiInput = EventSystem.current.GetComponent<InputSystemUIInputModule>();
            }

            if (writer != null)
            {
                if (uiInput.submit.action.triggered ||
                    (cancelEnabled && uiInput.submit.action.triggered))
                {
                    SetNextLineFlag();
                }
            }

            switch (clickMode)
            {
            case ClickMode.Disabled:
                break;
            case ClickMode.ClickAnywhere:
                if (Input.GetMouseButtonDown(0))
                {
                    SetClickAnywhereClickedFlag();
                }
                break;
            case ClickMode.ClickOnDialog:
                if (dialogClickedFlag)
                {
                    SetNextLineFlag();
                    dialogClickedFlag = false;
                }
                break;
            }

            if (ignoreClickTimer > 0f)
            {
                ignoreClickTimer = Mathf.Max (ignoreClickTimer - Time.deltaTime, 0f);
            }

            if (ignoreMenuClicks)
            {
                // Ignore input events if a Menu is being displayed
                if (MenuDialog.ActiveMenuDialog != null && 
                    MenuDialog.ActiveMenuDialog.IsActive() &&
                    MenuDialog.ActiveMenuDialog.DisplayedOptionsCount > 0)
                {
                    dialogClickedFlag = false;
                    nextLineInputFlag = false;
                }
            }

            // Tell any listeners to move to the next line
            if (nextLineInputFlag)
            {
                var inputListeners = gameObject.GetComponentsInChildren<IDialogInputListener>();
                for (int i = 0; i < inputListeners.Length; i++)
                {
                    var inputListener = inputListeners[i];
                    inputListener.OnNextLineEvent();
                }
                nextLineInputFlag = false;
            }
        }

        #region Public members

        /// <summary>
        /// Trigger next line input event from script.
        /// </summary>
        public virtual void SetNextLineFlag()
        {
            if(writer.IsWaitingForInput || writer.IsWriting)
            {
                nextLineInputFlag = true;
            }
        }
        /// <summary>
        /// Set the ClickAnywhere click flag.
        /// </summary>
        public virtual void SetClickAnywhereClickedFlag()
        {
            if (ignoreClickTimer > 0f)
            {
                return;
            }
            ignoreClickTimer = nextClickDelay;

            // Only applies if ClickedAnywhere is selected
            if (clickMode == ClickMode.ClickAnywhere)
            {
                SetNextLineFlag();
            }
        }
        /// <summary>
        /// Set the dialog clicked flag (usually from an Event Trigger component in the dialog UI).
        /// </summary>
        public virtual void SetDialogClickedFlag()
        {
            // Ignore repeat clicks for a short time to prevent accidentally clicking through the character dialogue
            if (ignoreClickTimer > 0f)
            {
                return;
            }
            ignoreClickTimer = nextClickDelay;

            // Only applies in Click On Dialog mode
            if (clickMode == ClickMode.ClickOnDialog)
            {
                dialogClickedFlag = true;
            }
        }

        /// <summary>
        /// Sets the button clicked flag.
        /// </summary>
        public virtual void SetButtonClickedFlag()
        {
            // Only applies if clicking is not disabled
            if (clickMode != ClickMode.Disabled)
            {
                SetNextLineFlag();
            }
        }

        #endregion
    }
}