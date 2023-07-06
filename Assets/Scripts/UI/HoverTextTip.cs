using NaughtyAttributes;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class HoverTextTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [field: ResizableTextArea] [field: SerializeField] public string message { get; private set; }
        [field: SerializeField] public bool willShowTip { get; set; } = true;
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            if(!willShowTip) return;
            ToolTip.OnShowToolTip.Invoke(message);
        }
        
        public void OnPointerExit(PointerEventData eventData)
        {
            if(!willShowTip) return;
            ToolTip.OnHideToolTip.Invoke();
        }
        
        public void SetMessage(string message_)
        {
            message = message_;
        }
    }
}
