using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class ButtonSpriteChanger : MonoBehaviour, ISelectHandler,IDeselectHandler,IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Button button;
        [SerializeField] private Sprite selectedSprite;

        private Sprite defaultSprite;
        
        private void Reset()
        {
            button = GetComponent<Button>();
            if (TryGetComponent<ButtonSpriteChanger>(out var _spriteChanger2))
            {
                if(_spriteChanger2 == this) DestroyImmediate(this);
                return;
            }
        }
        
        private void Awake()
        {
            if(button == null) button = GetComponent<Button>();
            if (button == null)
            {
                Destroy(this);
                return;
            }
            defaultSprite = button.image.sprite;
        }
        
        private void OnEnable()
        {
            button.image.sprite = defaultSprite;
        }
        
        private void Select()
        {
            button.image.sprite = selectedSprite;
        }
        
        private void Deselect()
        {
            button.image.sprite = defaultSprite;
        }
        
        public void OnSelect(BaseEventData eventData)
        {
            Select();
        }
        
        public void OnDeselect(BaseEventData eventData)
        {
            Deselect();
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            Select();
        }
        
        public void OnPointerExit(PointerEventData eventData)
        {
            Deselect();
        }
    }
}
