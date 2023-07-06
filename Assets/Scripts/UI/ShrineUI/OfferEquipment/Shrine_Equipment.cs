using System;
using System.Collections;
using System.Collections.Generic;
using CustomHelpers;
using DG.Tweening;
using Items;
using NaughtyAttributes;
using Shop;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.ShrineUI
{
    public class Shrine_Equipment : ShrineMenu
    {
        [SerializeField] private ShrineEquipmentDetailsPanel detailsPanel;
        [SerializeField] private Transform contentParent;
        [SerializeField] private ShrineUI_Item uiItemPrefab;

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
        
        Vector3 receivedPanelPos;

        private void Awake()
        {
            receivedPanelPos = receivedPanel.transform.position;
        }

        private void OnEnable()
        {
            Time.timeScale = 0;
            ShrineUI_Item.OnClickItem.AddListener(SetItemPanel);
            RemoveAllItem();
            CreateList();

            receivedPanel.transform.position = receivedPanelPos;
            receivedPanel.gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            Time.timeScale = 1;
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

            StartCoroutine(DisplayReceivedSeed(_seed)); 
        }
        
        private IEnumerator DisplayReceivedSeed(ItemSeed seed_)
        {
            receivedPanel.gameObject.SetActive(true);
            
            receivedPanel.transform.position = receivedPanelPos;
            
            receivedPanel.color = new Color(1,1,1,255f);
            receivedTxt.color = new Color(1,1,1,255f);
            receivedAmountTxt.color = new Color(1,1,1,255f);
            receivedIcon.color = new Color(1,1,1,255f);
            
            receivedTxt.text = $"{seed_.Data.ItemName}";
            receivedAmountTxt.text = $"+{seed_.StackCount}";
            receivedIcon.sprite = seed_.Data.Icon;


            receivedPanel.DOFade(0, 1f).SetUpdate(true);
            receivedTxt.DOFade(0, 1f).SetUpdate(true);
            receivedAmountTxt.DOFade(0, 1f).SetUpdate(true);
            receivedIcon.DOFade(0, 1f).SetUpdate(true);
            
            yield return receivedPanel.transform
                .DOMoveY(receivedPanelPos.y + 200f, 1f)
                .SetEase(Ease.Linear)
                .SetUpdate(true)
                .WaitForCompletion();
            
            
            receivedPanel.gameObject.SetActive(false);
        }

        private void SelectFirstItem()
        {
            errorTxt.gameObject.SetActive(false);
            if (uiItemList.Count > 0)
            {
                var _selected = uiItemList[0];
                _selected.SelectButton();
                EventSystem.current.SetSelectedGameObject(_selected.gameObject);
                detailsPanel.DisplayItem(equipments[0]);
            }
            else
            {
                detailsPanel.DisplayItem(null);
                errorTxt.text = "No items to offer";
                errorTxt.gameObject.SetActive(true);
            }
        }
    }
}