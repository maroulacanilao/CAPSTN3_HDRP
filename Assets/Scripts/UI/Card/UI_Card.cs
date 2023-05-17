using System.Collections;
using CustomEvent;
using CustomHelpers;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Card
{
    public enum CardUiState { Idle, Hover, Selected }

    public class UI_Card : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private RectTransform card;
        [SerializeField] private Vector2 hoverSizeScale, hoverPosition;
        [SerializeField] private RectTransform selectedTargetTransform;
        [SerializeField] private float hoverAnimationDuration = 0.2f;
    
        public static readonly Evt<UI_Card> OnCardChangeState = new Evt<UI_Card>();

        private Vector2 defaultSize;
        private Vector2 hoverTargetSize;
    
        private Vector2 defaultPosition;
        private Vector2 hoverTargetPosition;
    
        public CardUiState cardState { get; private set; }
    
        private void Awake()
        {
            defaultSize = card.sizeDelta;
            defaultPosition = card.anchoredPosition;
        
            hoverTargetPosition = card.anchoredPosition.Add(hoverPosition.x,hoverPosition.y);
            hoverTargetSize = new Vector2(defaultSize.x * hoverSizeScale.x, defaultSize.y * hoverSizeScale.y);

            cardState = CardUiState.Idle;
        }
    
        public void OnPointerClick(PointerEventData eventData)
        {
            ChangeState(CardUiState.Selected);
        }
    
        public void OnPointerEnter(PointerEventData eventData)
        {
            if(cardState != CardUiState.Idle) return;
            ChangeState(CardUiState.Hover);
        }
    
        public void OnPointerExit(PointerEventData eventData)
        {
            if(cardState== CardUiState.Selected) return;
            ChangeState(CardUiState.Idle);
        }

        public IEnumerator HoverAnimation()
        {
            Sequence _sequence = DOTween.Sequence();
            _sequence.Append(card.DOSizeDelta(hoverTargetSize, hoverAnimationDuration));
            _sequence.Join(card.DOAnchorPos(hoverTargetPosition, hoverAnimationDuration));
            yield return _sequence.WaitForCompletion();
        }

        public IEnumerator ReturnAnimation()
        {
            Sequence _sequence = DOTween.Sequence();
            _sequence.Append(card.DOAnchorPos(defaultPosition, hoverAnimationDuration));
            _sequence.Join(card.DOSizeDelta(defaultSize, hoverAnimationDuration));
            yield return _sequence.WaitForCompletion();
        }

        public IEnumerator SelectAnimation()
        {
            Sequence _sequence = DOTween.Sequence();
            _sequence.Append(card.DOMove(selectedTargetTransform.position, hoverAnimationDuration));
            _sequence.Join(card.DOSizeDelta(selectedTargetTransform.sizeDelta, hoverAnimationDuration));
            yield return _sequence.WaitForCompletion();
        }

        public void ChangeState(CardUiState state_)
        {
            cardState = state_;
            if (state_ != CardUiState.Idle) OnCardChangeState.Invoke(this);
        
            switch (cardState)
            {
                case CardUiState.Idle:
                    StartCoroutine(ReturnAnimation());
                    break;
                case CardUiState.Hover:
                    StartCoroutine(HoverAnimation());
                    break;
                case CardUiState.Selected:
                    StartCoroutine(SelectAnimation());
                    break;
            }
        }
    }
}