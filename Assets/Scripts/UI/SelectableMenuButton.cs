using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Button), typeof(Outline))]
    public abstract class SelectableMenuButton: MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        [SerializeField] protected Button button;
        [SerializeField] protected Outline outline;
        [SerializeField] protected Color outlineColor;
        [SerializeField] protected Vector2 outlineSize = new Vector2(3, 3);

        private void Reset()
        {
            button = GetComponent<Button>();
            outline = GetComponent<Outline>();
        }

        public virtual void OnSelect(BaseEventData eventData)
        {
            outline.effectColor = outlineColor;
            outline.effectDistance = outlineSize;
        }
        
        public virtual void OnDeselect(BaseEventData eventData)
        {
            outline.effectColor = Color.clear;
        }

        protected virtual void OnDisable()
        {
            outline.effectColor = Color.clear;
        }
    }
}