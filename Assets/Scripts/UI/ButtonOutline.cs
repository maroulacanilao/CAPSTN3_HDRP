using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Outline), typeof(Image))]
    public class ButtonOutline : Button
    {
        [field: SerializeField] public Color outlineColor { get; protected set; } = new Color(255, 0, 0, 255);
        [field: SerializeField] public Vector2 outlineSize { get; protected set; } = new Vector2(3, 3);
        
        private Outline _outline;
        private Outline outline
        {
            get
            {
                if (_outline == null) _outline = GetComponent<Outline>();
                if (_outline == null) _outline = gameObject.AddComponent<Outline>();
                return _outline;
            }
        }
        


        protected override void Awake()
        {
            base.Awake();
            outline.effectDistance = outlineSize;
            outline.effectColor = Color.clear;
        }
        
        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            outline.effectColor = outlineColor;
        }
        
        
    }
}