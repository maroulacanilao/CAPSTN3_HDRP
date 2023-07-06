using System;
using CustomHelpers;
using UnityEngine;

namespace UI.Farming
{
    public abstract class PlayerMenu : MonoBehaviour
    {
        public static PlayerMenu OpenedMenu { get; protected set; }
        public virtual void Initialize() {}

        protected virtual void OnEnable()
        {
            OpenedMenu = this;
            PlayerMenuManager.OnCloseAllUI.AddListener(CloseMenu);
        }
        
        protected virtual void OnDisable()
        {
            PlayerMenuManager.OnCloseAllUI.RemoveListener(CloseMenu);
        }
        
        protected virtual void CloseMenu()
        {
            if(this.IsEmptyOrDestroyed()) return;
            gameObject.SetActive(false);
        }
    }
}
