using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Items;
using NaughtyAttributes;
using Shop;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

namespace UI.ShrineUI.OfferEquipment
{
    public class ShrineGearToSeed : ShrineMenu
    {
        [SerializeField] private ShrineEquipmentDetailsPanel detailsPanel;
        [SerializeField] private Transform contentParent;
        [SerializeField] private ShrineUI_Item uiItemPrefab;
        [SerializeField] private SeedDisplay seedDisplay;

        private readonly List<ShrineUI_Item> uiItemList = new List<ShrineUI_Item>();
        private List<Item> equipments = new List<Item>();
        
        [BoxGroup("Received Panel")]
        [SerializeField] private Image receivedPanel;
        [BoxGroup("Received Panel")]
        [SerializeField] private TextMeshProUGUI receivedTxt;
        [BoxGroup("Received Panel")]
        [SerializeField] private TextMeshProUGUI receivedAmountTxt;
        [BoxGroup("Received Panel")]
        [SerializeField] private Image receivedIcon;

        [SerializeField] private GameObject ReceiveInfoPanel;
        
        // Vector3 receivedPanelPos;
        Vector3 originalPos;

        [SerializeField] private bool isReceivedPanelColored = true;

        private void Awake()
        {
            // receivedPanelPos = receivedPanel.transform.position;
            originalPos = receivedPanel.transform.localPosition;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            ShrineUI_Item.OnClickItem.AddListener(SetItemPanel);
            RemoveAllItem();
            if (seedDisplay != null) seedDisplay.UpdateDisplay();
            CreateList();

            // receivedPanel.transform.position = receivedPanelPos;
            receivedPanel.transform.localPosition = new Vector3(1200f, originalPos.y);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            ShrineUI_Item.OnClickItem.RemoveListener(SetItemPanel);
            RemoveAllItem();
        }

        void CreateList()
        {
            equipments.Clear();
            equipments = inventory.GetItemsInStorageByType(ItemType.Armor);
            equipments.AddRange(inventory.GetItemsInStorageByType(ItemType.Weapon));

            equipments.RemoveAll(e => e == null);

            for (var _index = 0; _index < equipments.Count; _index++)
            {
                var equipment = equipments[_index];
                var _uiItem = Instantiate(uiItemPrefab, contentParent);

                var _text = $"{equipment.Data.ItemName} - Lv.{equipment.Level}";
                _uiItem.Initialize(_text, equipment.Data.Icon);

                uiItemList.Add(_uiItem);
            }
            
            SelectFirstItem();
        }
        
        public void RemoveAllItem()
        {
            foreach (var item in uiItemList)
            {
                Destroy(item.gameObject);
            }
            
            uiItemList.Clear();
            equipments.Clear();
            
            SelectFirstItem();
        }
        
        public void RemoveItem(int index_)
        {
            var _menuItem = uiItemList[index_];
            Destroy(_menuItem.gameObject);
            
            uiItemList.RemoveAt(index_);
            equipments.RemoveAt(index_);
        }

        public void SetItemPanel(ShrineUI_Item item_)
        {
            if(item_ == null) return;
            if(!uiItemList.Contains(item_)) return;
            
            var _index = uiItemList.IndexOf(item_);
            var _item = equipments[_index];
            detailsPanel.DisplayItem(_item);
            ReceiveInfoPanel.SetActive(true);
            detailsPanel.gameObject.SetActive(true);
        }

        public void OfferWeapon(Item item_)
        {
            if(item_ is not ItemGear _gear) return;
            
            var _result = shrineData.OfferGear(_gear, out var _seed);

            if (_result != OfferingResult.Success)
            {
                Debug.Log($"Offer result: {_result}");
                return;
            }

            var _index = equipments.IndexOf(_gear);

            RemoveItem(_index);
            
            detailsPanel.DisplayItem(null);
            detailsPanel.gameObject.SetActive(false);
            ReceiveInfoPanel.SetActive(false);

            if (seedDisplay != null) seedDisplay.UpdateDisplay();
            // StartCoroutine(DisplayReceivedSeed(_seed));
            PlaySeedReceivedAnimation(_seed);
        }
        
        private IEnumerator DisplayReceivedSeed(ItemSeed seed_)
        {
            receivedPanel.gameObject.SetActive(true);
            
            // receivedPanel.transform.position = receivedPanelPos;

            if (isReceivedPanelColored)
            {
                receivedPanel.color = new Color(0, 0, 0, 235f);
                receivedTxt.color = new Color(1, 1, 1, 255f);
                receivedAmountTxt.color = new Color(1, 1, 1, 255f);
                receivedIcon.color = new Color(1, 1, 1, 255f);
            }

            receivedTxt.text = $"{seed_.Data.ItemName}";
            receivedAmountTxt.text = $"+{seed_.StackCount}";
            receivedIcon.sprite = seed_.Data.Icon;


            receivedPanel.DOFade(0, 1f).SetUpdate(true);
            receivedTxt.DOFade(0, 1f).SetUpdate(true);
            receivedAmountTxt.DOFade(0, 1f).SetUpdate(true);
            receivedIcon.DOFade(0, 1f).SetUpdate(true);
            
            yield return receivedPanel.transform
                .DOMoveY(originalPos.y + 200f, 1f)
                .SetEase(Ease.Linear)
                .SetUpdate(true)
                .WaitForCompletion();
            
            
            receivedPanel.gameObject.SetActive(false);
        }

        private void PlaySeedReceivedAnimation(ItemSeed seed_)
        {
            receivedTxt.text = $"{seed_.Data.ItemName}";
            receivedAmountTxt.text = $"+{seed_.StackCount}";
            receivedIcon.sprite = seed_.Data.Icon;

            var sequence = DOTween.Sequence();
            sequence.Append(receivedPanel.transform.DOLocalMoveX(originalPos.x, 0.5f));
            sequence.AppendInterval(1f);
            sequence.Append(receivedPanel.transform.DOLocalMoveX(1200f, 0.5f));
        }

        private void SelectFirstItem()
        {
            errorTxt.gameObject.SetActive(false);
            if (uiItemList.Count > 0)
            {
                var _selected = uiItemList[0];
                _selected.SelectButton();
                EventSystem.current.SetSelectedGameObject(_selected.gameObject);
                detailsPanel.gameObject.SetActive(true);
                detailsPanel.DisplayItem(equipments[0]);
                ReceiveInfoPanel.SetActive(true);
            }
            else
            {
                detailsPanel.DisplayItem(null);
                detailsPanel.gameObject.SetActive(false);
                ReceiveInfoPanel.SetActive(false);
                errorTxt.text = "No items to offer";
                errorTxt.gameObject.SetActive(true);
            }
        }
    }
}