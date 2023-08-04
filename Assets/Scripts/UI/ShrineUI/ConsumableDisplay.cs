using System;
using System.Collections.Generic;
using System.Linq;
using Items;
using Items.Inventory;
using Items.ItemData;
using NaughtyAttributes;
using ScriptableObjectData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.ShrineUI
{
    public class ConsumableDisplay : MonoBehaviour
    {
        [Serializable]
        public struct ConsumableDisplayItem
        {
            public GameObject panel;
            public Image icon;
            public TextMeshProUGUI countTxt;
        }

        [SerializeField]
        protected List<ConsumableDisplayItem>consumableDisplayItems;
        [SerializeField] protected GameDataBase gameDataBase;

        protected PlayerInventory inventory => gameDataBase.playerInventory;
        protected ItemDatabase itemDatabase => gameDataBase.itemDatabase;

        private ConsumableData[] GetAllConsumableData()
        {
            if (!itemDatabase.ItemDataByType.TryGetValue(ItemType.Consumable, out var _consumableDataList))
            {
                throw new Exception("No consumable data found");
            }
            return _consumableDataList
                .Where(d => d != null && d is ConsumableData)
                .Select(d => d as ConsumableData)
                .ToArray();
        }
        
        private int GetCount(ConsumableData consumableData)
        {
            if (!inventory.itemsLookup.TryGetValue(consumableData, out var _itemList))
            {
                return 0;
            }
            if(_itemList == null) return 0;
            if(_itemList.Count == 0) return 0;
            var _item = _itemList.FirstOrDefault(i => i != null);
            if(_item == null) return 0;
            
            return _item is not ItemConsumable _consumable ? 0 : _consumable.StackCount;
        }

        protected virtual void OnEnable()
        {
            UpdateDisplay();
        }
        
        public virtual void UpdateDisplay()
        {
            var _dataArr = GetAllConsumableData();
            for (var i = 0; i < consumableDisplayItems.Count; i++)
            {
                if(i >= _dataArr.Length) continue;
                var _data = _dataArr[i];
                if(_data == null) continue;
                var _count = GetCount(_data);
                var _displayItem = consumableDisplayItems[i];
                _displayItem.panel.SetActive(_data != null);
                _displayItem.icon.sprite = _data.Icon;
                _displayItem.countTxt.text = $"x{_count}";
            }
        }

        [Header("Ako ay tinatamad")] public GameObject[] panels;

        [Button("Ako ay tinatamad")]
        private void ConfigureDisplay()
        {
            consumableDisplayItems = new List<ConsumableDisplayItem>();
            for (int i = 0; i < panels.Length; i++)
            {
                var _panel = panels[i];
                var _display = new ConsumableDisplayItem
                {
                    panel = _panel,
                    icon = _panel.GetComponentInChildren<Image>(),
                    countTxt = _panel.GetComponentInChildren<TextMeshProUGUI>()
                };

                consumableDisplayItems.Add(_display);
            }
        }
    }
}
