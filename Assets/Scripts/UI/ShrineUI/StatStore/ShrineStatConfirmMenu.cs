using CustomHelpers;
using Items;
using Items.ItemData;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.ShrineUI.StatStore
{
    public class ShrineStatConfirmMenu : MonoBehaviour
    {
        [SerializeField] private Image statIcon;
        [SerializeField] private TextMeshProUGUI messageText, statTXT;
        [SerializeField] private Button confirmButton, cancelButton, addButton, subtractButton;
        [SerializeField] private TMP_InputField countInput;
        [SerializeField] private TextMeshProUGUI errorText;
        
        [SerializeField] [NaughtyAttributes.ResizableTextArea] private string message;

        private int count;
        private ShrineStat menu;
        private ItemConsumable item;
        private RectTransform rectTransform;

        public void Initialize(ShrineStat shippingMenu_)
        {
            menu = shippingMenu_;
            gameObject.SetActive(false);
            confirmButton.onClick.AddListener(OnConfirm);
            cancelButton.onClick.AddListener(OnCancel);
            addButton.onClick.AddListener(OnAdd);
            subtractButton.onClick.AddListener(OnSubtract);
            countInput.onValueChanged.AddListener(OnCountChange);
            
            rectTransform = GetComponent<RectTransform>();
        }
        
        private void OnCountChange(string input_)
        {
            count = int.Parse(input_);
            ClampCount();
        }
        
        private void OnSubtract()
        {
            count--;
            ClampCount();
        }
        private void OnAdd()
        {
            count++;
            ClampCount();
        }
        
        private void OnCancel()
        {
            gameObject.SetActive(false);
            item = null;
        }
        
        private void OnConfirm()
        {
            if (count <= 0)
            {
                OnCancel();
                return;
            }
            
            menu.OfferConsumable(item, count);
            OnCancel();
        }

        public void DisplayConfirmation(ShrineStatBtn button_)
        {
            if (button_.IsEmptyOrDestroyed() || button_.item == null)
            {
                menu.CreateList();
                gameObject.SetActive(false);
                return;
            }
            item = button_.item;
            
            var _itemName = $"<color=yellow>{this.item.Data.ItemName}</color>";

            var _message = message.Replace("NAME", _itemName);
                
            messageText.text = _message;

            var _canOffer = menu.CanStillOffer(item, out var _error);
            confirmButton.interactable = _canOffer;
            subtractButton.interactable = false;
            addButton.interactable = _canOffer;
            count = _canOffer ? 1 : 0;
            
            countInput.text = count.ToString();
            
            errorText.gameObject.SetActive(!_canOffer);
            errorText.text = _error;
            
            gameObject.SetActive(true);
        }

        public void ClampCount()
        {
            var _max = menu.GetMaxCanOffer(item);
            
            count = Mathf.Clamp(count, 0, _max);
            countInput.text = count.ToString();
            
            subtractButton.interactable = count > 1;
            addButton.interactable = menu.CanStillOffer(item, count + 1);
            
            DisplayAdditionalStat();
        }

        private void DisplayAdditionalStat()
        {
            var _data = item.Data as ConsumableData;
            var _statType = _data.GetStatType();
            statIcon.sprite = _statType.GetSpriteIcon();
            statTXT.text = $"{_statType} +{count}";
        }
    }
}