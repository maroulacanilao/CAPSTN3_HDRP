using System;
using CustomEvent;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

namespace UI.TabMenu
{
    [RequireComponent(typeof(Image))]
    public class TabButton : MonoBehaviour, ISelectHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
    {
        [field: SerializeField] public int index { get; private set; }
        [SerializeField] private TabGroup tabGroup;
        [SerializeField] private Image background;
        // [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private Sprite selectedSprite;
        [SerializeField] private Sprite deselectedSprite;
        [SerializeField] private Color highlightedColor;

        [SerializeField] private GameObject deselectedImageGO;
        [SerializeField] private GameObject selectedImageGO;

        [SerializeField] private float movePos = 173.54f;
        private Vector3 originalPos;

        private void Reset()
        {
            background = GetComponentInChildren<Image>();
            // text = GetComponentInChildren<TextMeshProUGUI>();
            tabGroup = GetComponentInParent<TabGroup>();
        }
        
        public void Initialize(TabGroup tabGroup_, int index_)
        {
            tabGroup = tabGroup_;
            index = index_;
            if (selectedImageGO != null)
            {
                originalPos = selectedImageGO.transform.localPosition;
                Debug.Log(originalPos);
                selectedImageGO.SetActive(false);
            }
        }

        public void OnSelect(BaseEventData eventData)
        {
            tabGroup.SelectTab(index);
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            if(tabGroup.selectedTab == this) return;
            if (background != null) background.color = highlightedColor;
        }
        
        public void OnPointerExit(PointerEventData eventData)
        {
            if(tabGroup.selectedTab == this) return;
            Deselect();
        }
        
        public void Select()
        {
            if (background != null) background.sprite = selectedSprite;

            PlaySelectedAnimation();
        }

        public void Deselect()
        {
            if (background != null) background.sprite = deselectedSprite;
            if (background != null) background.color = Color.white;

            PlayDeselectedAnimation();
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            OnSelect(eventData);
        }

        public void PlaySelectedAnimation()
        {
            if (selectedImageGO != null)
            {
                selectedImageGO.SetActive(true);
                selectedImageGO.transform.DOLocalMoveY(movePos, 0.25f);
            }
        }

        public void PlayDeselectedAnimation()
        {
            if (selectedImageGO != null)
            {
                selectedImageGO.transform.DOLocalMoveY(originalPos.y, 0.25f);
                selectedImageGO.SetActive(false);
            }
        }
    }
}
