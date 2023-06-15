using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UI
{
    public class ScrollSnapToChild : MonoBehaviour
    {
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField]
        private bool isVertical = true, isHorizontal = false;

        private void OnValidate()
        {
            scrollRect = GetComponent<ScrollRect>();
        }
        private void Reset()
        {
            scrollRect = GetComponent<ScrollRect>();
        }

        private void OnEnable()
        {
            InputUIManager.OnMove.AddListener(Move);
            scrollRect.verticalScrollbar.onValueChanged.AddListener(ClampScroll);
        }

        private void OnDisable()
        {
            scrollRect.verticalScrollbar.onValueChanged.RemoveListener(ClampScroll);
            InputUIManager.OnMove.RemoveListener(Move);
        }

        private void Move(InputAction.CallbackContext context_) => SnapToChild();

        private void SnapToChild()
        {
            GameObject selectedObject = EventSystem.current.currentSelectedGameObject;

            if (selectedObject == null || scrollRect == null) return;
        
            RectTransform selectedTransform = selectedObject.GetComponent<RectTransform>();

            if (selectedTransform == null || !selectedTransform.IsChildOf(scrollRect.content)) return;
        
            var _scrollBarSize = scrollRect.verticalScrollbar.size;
        
            Vector2 selectedObjectPosition = selectedTransform.anchoredPosition;
            Vector2 scrollRectCenter = scrollRect.viewport.rect.center;
            Vector2 offset = selectedObjectPosition - scrollRectCenter;

            scrollRect.content.anchoredPosition -= offset;
        
            scrollRect.verticalScrollbar.size = _scrollBarSize;
            scrollRect.verticalScrollbar
                .SetValueWithoutNotify(Mathf.Clamp01(scrollRect.verticalScrollbar.value));
        }

        private void ClampScroll(float val_)
        {
            scrollRect.verticalScrollbar.SetValueWithoutNotify(Mathf.Clamp01(scrollRect.verticalScrollbar.value));
            scrollRect.verticalScrollbar.size = Mathf.Clamp01(scrollRect.verticalScrollbar.size);
        }
    }
}
