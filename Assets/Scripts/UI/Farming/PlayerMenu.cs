using System;
using CustomHelpers;
using Fungus;
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
            CloseDialog();
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

        protected virtual void CloseDialog()
        {
            var _dialog = FindObjectOfType<SayDialog>();
            if (_dialog != null)
            {
                _dialog.Stop();
            }
        }
    }
}
