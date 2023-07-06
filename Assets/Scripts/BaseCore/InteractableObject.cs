using CustomEvent;
using CustomHelpers;
using UnityEngine;

namespace BaseCore
{
    public abstract class InteractableObject : MonoBehaviour
    {
        [field: SerializeField] public bool canInteract { get; protected set; } = true;
        [field: SerializeField] public string interactText { get; protected set; } = "Interact";
        public static readonly Evt<InteractableObject> OnInteract = new Evt<InteractableObject>();
        public static readonly Evt<InteractableObject> OnEnter = new Evt<InteractableObject>();
        public static readonly Evt<InteractableObject> OnExit = new Evt<InteractableObject>();

        protected bool isEfectActive;

        protected virtual void OnEnable()
        {
            OnInteract.AddListener(InteractWrapper);
            OnEnter.AddListener(EnterWrapper);
            OnExit.AddListener(ExitWrapper);
        }

        protected virtual void OnDisable()
        {
            OnInteract.RemoveListener(InteractWrapper);
            OnEnter.RemoveListener(EnterWrapper);
            OnExit.RemoveListener(ExitWrapper);
        }
    
        private void InteractWrapper(InteractableObject obj_)
        {
            if(IsNull()) return;
            if(obj_ != this) return;
            Interact();
        }
    
        private void EnterWrapper(InteractableObject obj_)
        {
            if(IsNull()) return;
            if (obj_ != this)
            {
                if(isEfectActive) Exit();
                return;
            }
            
            isEfectActive = true;
            Enter();
        }

        private void ExitWrapper(InteractableObject obj_)
        {
            if(IsNull()) return;
            if(obj_ != this) return;
            isEfectActive = false;
            Exit();
        }

        protected abstract void Interact();
        protected abstract void Enter();
        protected abstract void Exit();

        protected bool IsNull()
        {
            if (gameObject.IsValid()) return false;
            
            OnInteract.RemoveListener(InteractWrapper);
            OnEnter.RemoveListener(EnterWrapper);
            OnExit.RemoveListener(ExitWrapper);
            return true;
        }
    }
}
