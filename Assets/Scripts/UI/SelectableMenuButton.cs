using System;
using CustomEvent;
using CustomHelpers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Button), typeof(Outline))]
    public class SelectableMenuButton: MonoBehaviour, ISelectHandler
    {
        [field: SerializeField] public Image background { get; protected set; }
        [field: SerializeField] public Button button { get; protected set; }
        [field: SerializeField] public Outline outline { get; protected set; }
        [field: SerializeField] public Color outlineColor { get; protected set; }
        [field: SerializeField] public Vector2 outlineSize { get; protected set; } = new Vector2(3, 3);
        
        public static readonly Evt<SelectableMenuButton> OnSelectButton = new Evt<SelectableMenuButton>();
        
        private Color defaultColor;
        private void Reset()
        {
            button = GetComponent<Button>();
            outline = GetComponent<Outline>();
            background = GetComponent<Image>();
            outlineColor = new Color(outlineColor.r, outlineColor.g, outlineColor.b, 1);
        }

        protected virtual void Awake()
        {
            if(button == null) button = GetComponent<Button>();
            if(outline == null) outline = GetComponent<Outline>();
            if(background == null) background = GetComponent<Image>();
            
            defaultColor = background.color;

            outline.effectDistance = outlineSize;
            outline.effectColor = Color.clear;
        }

        protected virtual void OnEnable()
        {
            OnSelectButton.AddListener(SelectButtonHandler);
        }
        
        protected virtual void OnDisable()
        {
            outline.effectColor = Color.clear;
            OnSelectButton.RemoveListener(SelectButtonHandler);
        }

        public void OnSelect(BaseEventData eventData)
        {
            OnSelectButton.Invoke(this);
        }
        
        public void SelectButtonHandler(SelectableMenuButton button_)
        {
            if (button_ == this) SelectButton();
            else DeselectButton();
        }

        public virtual void SelectButton()
        {
            // outline.effectColor = outlineColor;
            if (background.IsValid()) background.color = defaultColor.AddGrayishTint(0.35f);
        }

        public virtual void DeselectButton()
        {
            // outline.effectColor = Color.clear;
            if(background.IsValid()) background.color = defaultColor;
        }
    }
}