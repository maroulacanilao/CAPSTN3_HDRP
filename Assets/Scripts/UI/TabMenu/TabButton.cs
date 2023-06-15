using System;
using CustomEvent;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.TabMenu
{
    [RequireComponent(typeof(Image))]
    public class TabButton : MonoBehaviour, ISelectHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
    {
        [field: SerializeField] public int index { get; private set; }
        [SerializeField] private TabGroup tabGroup;
        [SerializeField] private Image background;
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private Color selectedColor;
        [SerializeField] private Color deselectedColor;
        [SerializeField] private Color highlightedColor;

        private void Reset()
        {
            background = GetComponent<Image>();
            text = GetComponentInChildren<TextMeshProUGUI>();
            tabGroup = GetComponentInParent<TabGroup>();
        }
        
        public void Initialize(TabGroup tabGroup_, int index_)
        {
            tabGroup = tabGroup_;
            index = index_;
        }

        public void OnSelect(BaseEventData eventData)
        {
            tabGroup.SelectTab(index);
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            if(tabGroup.selectedTab == this) return;
            background.color = highlightedColor;
        }
        
        public void OnPointerExit(PointerEventData eventData)
        {
            if(tabGroup.selectedTab == this) return;
            Deselect();
        }
        
        public void Select()
        {
            background.color = selectedColor;
        }

        public void Deselect()
        {
            background.color = deselectedColor;
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            OnSelect(eventData);
        }
    }
}
