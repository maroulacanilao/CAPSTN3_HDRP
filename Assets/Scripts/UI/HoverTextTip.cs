using NaughtyAttributes;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class HoverTextTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [field: ResizableTextArea] [field: SerializeField] public string message { get; private set; }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            ToolTip.OnShowToolTip.Invoke(message);
        }
        
        public void OnPointerExit(PointerEventData eventData)
        {
            ToolTip.OnHideToolTip.Invoke();
        }
        
        public void SetMessage(string message_)
        {
            message = message_;
        }
    }
}
