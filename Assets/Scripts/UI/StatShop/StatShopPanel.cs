using System;
using System.Collections;
using System.Collections.Generic;
using Character;
using CustomEvent;
using CustomHelpers;
using Items.Inventory;
using NaughtyAttributes;
using ScriptableObjectData;
using TMPro;
using UI.Farming;
using UnityEngine;
using UnityEngine.UI;

namespace UI.StatShop
{


    [DefaultExecutionOrder(2)]
    public class StatShopPanel : MonoBehaviour
    {
        [SerializeField] private GameDataBase gameDataBase;
        [SerializeField] private TextMeshProUGUI totalPriceTXT, errorTXT;
        [SerializeField] private Button buyBTN;
        [SerializeField] private List<StatShopPanelItem> panelItems = new List<StatShopPanelItem>();

        public StatShopData statShopData => gameDataBase.statShopData;
        public PlayerInventory Inventory => gameDataBase.playerInventory;
        public CombatStats statsBought => statShopData.statsBought;
        public CombatStats tempStatsIncrease = new CombatStats();

        private int totalPrice;

        private void Awake()
        {
            foreach (var panel in panelItems)
            {
                panel.Initialize(this);
            }
            buyBTN.onClick.AddListener(Buy);
        }

        private void OnEnable()
        {
            errorTXT.gameObject.SetActive(false);
            tempStatsIncrease = new CombatStats();
            SetTotalPrice();
            StatShopPanelItem.OnChangeValue.AddListener(SetTotalPrice);
        }

        private void OnDisable()
        {
            StatShopPanelItem.OnChangeValue.RemoveListener(SetTotalPrice);
        }
        
        public void OpenMenu()
        {
            if (this.IsEmptyOrDestroyed())
            {
                return;
            }
            PlayerMenuManager.OnCloseAllUI.Invoke();
            RevisedPlayerMenuManager.OnCloseAllUIRevised.Invoke();
            gameObject.SetActive(true);
        }

        private void SetTotalPrice()
        {
            totalPrice = 0;

            foreach (var panelItems in panelItems)
            {
                totalPrice += panelItems.totalPrice;
            }

            totalPriceTXT.text = totalPrice.ToString();

            buyBTN.interactable = totalPrice > 0;
        }

        private void Buy()
        {
            if (!Inventory.Gold.RemoveGold(totalPrice))
            {
                var _message = $"<color=red>Not Enough Gold</color>";
                StartCoroutine(Co_SetErrorTXT(_message));
                return;
            }

            statShopData.SetBoughtStats(tempStatsIncrease + statsBought);

            tempStatsIncrease = new CombatStats();

            foreach (var panel in panelItems)
            {
                panel.SetTexts();
            }
        }

        private IEnumerator Co_SetErrorTXT(string message_)
        {
            errorTXT.SetText(message_);
            errorTXT.gameObject.SetActive(true);
            yield return new WaitForSeconds(2f);
            errorTXT.gameObject.SetActive(false);
            errorTXT.SetText("");
        }
    }
}
