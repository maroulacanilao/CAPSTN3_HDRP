using System;
using CustomEvent;
using NaughtyAttributes;
using ScriptableObjectData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.StatShop
{
    [DefaultExecutionOrder(3)]
    public class StatShopPanelItem : MonoBehaviour
    {
        [field: SerializeField] public StatType statType { get; private set; }
        [field: SerializeField] public TextMeshProUGUI priceTXT { get; private set; }
        [field: SerializeField] public TMP_InputField inputField { get; private set; }
        [field: SerializeField] public Button plusBTN { get; private set; }
        [field: SerializeField] public Button minusBTN { get; private set; }

        public int statBought
        {
            get
            {
                switch (statType)
                {
                    case StatType.Health:
                        return data.statsBought.vitality;
                    case StatType.Strength:
                        return data.statsBought.strength;
                    case StatType.Intelligence:
                        return data.statsBought.intelligence;
                    case StatType.Defense:
                        return data.statsBought.defense;
                    case StatType.Speed:
                        return data.statsBought.speed;
                }
                throw new Exception("No StatType Found");
            }
        }
        public int playerBaseStat
        {
            get
            {
                switch (statType)
                {
                    case StatType.Health:
                        return data.playerBaseStats.vitality;
                    case StatType.Strength:
                        return data.playerBaseStats.strength;
                    case StatType.Intelligence:
                        return data.playerBaseStats.intelligence;
                    case StatType.Defense:
                        return data.playerBaseStats.defense;
                    case StatType.Speed:
                        return data.playerBaseStats.speed;
                }
                throw new Exception("No StatType Found");
            }
        }
        public int tempStatIncrease
        {
            get
            {
                switch (statType)
                {
                    case StatType.Health:
                        return statShopPanel.tempStatsIncrease.vitality;
                    case StatType.Strength:
                        return statShopPanel.tempStatsIncrease.strength;
                    case StatType.Intelligence:
                        return statShopPanel.tempStatsIncrease.intelligence;
                    case StatType.Defense:
                        return statShopPanel.tempStatsIncrease.defense;
                    case StatType.Speed:
                        return statShopPanel.tempStatsIncrease.speed;
                }
                throw new Exception("No StatType Found");
            }
            set
            {
                switch (statType)
                {
                    case StatType.Health:
                        statShopPanel.tempStatsIncrease.vitality = value;
                        break;
                    case StatType.Strength:
                        statShopPanel.tempStatsIncrease.strength = value;
                        break;
                    case StatType.Intelligence:
                        statShopPanel.tempStatsIncrease.intelligence = value;
                        break;
                    case StatType.Defense:
                        statShopPanel.tempStatsIncrease.defense = value;
                        break;
                    case StatType.Speed:
                        statShopPanel.tempStatsIncrease.speed = value;
                        break;
                    default: throw new Exception("No StatType Found");
                }
            }
        }

        public int total => playerBaseStat + statBought + tempStatIncrease;

        public int totalPrice;

        public static readonly Evt OnChangeValue = new Evt();

        private StatShopPanel statShopPanel;
        private StatShopData data => statShopPanel.statShopData;
        
        public void Initialize(StatShopPanel statShopPanel_)
        {
            statShopPanel = statShopPanel_;
            plusBTN.onClick.AddListener(PlusClick);
            minusBTN.onClick.AddListener(MinusClick);
            inputField.onValueChanged.AddListener(InputFieldChange);
            SetTexts();
        }

        private void OnEnable()
        {
            if(statShopPanel == null) return;
            SetTexts();
        }

        private void PlusClick()
        {
            tempStatIncrease = tempStatIncrease + 1;
            SetTexts();
        }
        
        private void MinusClick()
        {
            tempStatIncrease--;
            SetTexts();
        }
        
        private void InputFieldChange(string newValue_)
        {
            SetTexts();
        }

        public void SetTexts()
        {
            var _max = data.maxStats - statBought;
            tempStatIncrease = Mathf.Clamp(tempStatIncrease, 0, _max);
            inputField.SetTextWithoutNotify(total.ToString());
            SetPrice();
            IsMax();
            IsMin();
            OnChangeValue.Invoke();
        }

        private bool IsMax()
        {
            var _isMax = data.maxStats <= statBought + tempStatIncrease;
            plusBTN.interactable = !_isMax;
            return _isMax;
        }
        
        private bool IsMin()
        {
            var _isMin = tempStatIncrease <= 0;
            minusBTN.interactable = !_isMin;
            return _isMin;
        }

        private void SetPrice()
        {
            totalPrice = 0;
            for (int i = 1; i < tempStatIncrease + 1; i++)
            {
                var _tempPrice = data.GetCost(statBought + i);
                totalPrice += _tempPrice;
            }
            
            priceTXT.SetText(totalPrice.ToString());
        }
    }
}
