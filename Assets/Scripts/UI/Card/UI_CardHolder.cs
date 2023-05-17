using UnityEngine;

namespace UI.Card
{
    public class UI_CardHolder : MonoBehaviour
    {
        [SerializeField] private UI_Card[] cards;

        private void Awake()
        {
            UI_Card.OnCardChangeState.AddListener(CardChangeState);
        }

        private void OnDestroy()
        {
            UI_Card.OnCardChangeState.RemoveListener(CardChangeState);
        }
    
        private void CardChangeState(UI_Card card_)
        {
            if(card_.cardState != CardUiState.Selected) return;

            foreach (var _otherCard in cards)
            {
                if(card_ == _otherCard) continue;
            
                _otherCard.ChangeState(CardUiState.Idle);
            }
        }
    }
}