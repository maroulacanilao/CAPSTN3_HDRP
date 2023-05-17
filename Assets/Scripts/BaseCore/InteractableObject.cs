using CustomEvent;
using UnityEngine;

namespace BaseCore
{
    public abstract class InteractableObject : MonoBehaviour
    {
        [field: SerializeField] public bool showIcon { get; protected set; } = true;
        public static readonly Evt<InteractableObject> OnInteract = new Evt<InteractableObject>();
        public static readonly Evt<InteractableObject> OnEnter = new Evt<InteractableObject>();
        public static readonly Evt<InteractableObject> OnExit = new Evt<InteractableObject>();

        private void OnEnable()
        {
            OnInteract.AddListener(InteractWrapper);
            OnEnter.AddListener(EnterWrapper);
            OnExit.AddListener(ExitWrapper);
        }

        private void OnDisable()
        {
            OnInteract.RemoveListener(InteractWrapper);
            OnEnter.RemoveListener(EnterWrapper);
            OnExit.RemoveListener(ExitWrapper);
        }
    
        private void InteractWrapper(InteractableObject obj_)
        {
            if(obj_ != this) return;
            Interact();
        }
    
        private void EnterWrapper(InteractableObject obj_)
        {
            if(obj_ != this) return;
            Enter();
        }

        private void ExitWrapper(InteractableObject obj_)
        {
            if(obj_ != this) return;
            Exit();
        }

        protected abstract void Interact();
        protected abstract void Enter();
        protected abstract void Exit();
    }
}
