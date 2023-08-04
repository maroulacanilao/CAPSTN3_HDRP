using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Character;
using CustomEvent;
using CustomHelpers;
using Items;
using Items.Inventory;
using Items.ItemData;
using ScriptableObjectData;
using Shop;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.ShrineUI.StatStore
{
    public class ShrineStat : ShrineMenu
    {
        [SerializeField] ShrineStatBtn shrineStatBtnPrefab;
        [SerializeField] Transform shrineStatBtnParent;
        [SerializeField] ShrineStatsDisplay shrineStatsDisplay;
        [SerializeField] private ShrineStatConfirmMenu confirmMenu;
        
        public int playerLevel => gameDataBase.playerData.level;
        
        public ShrineData shrineData => gameDataBase.shrineData;
        public StatShopData statShopData => gameDataBase.statShopData;
        public CombatStats boughtStats => statShopData.statsBought;
        public CombatStats baseStats => gameDataBase.playerData.baseStats;
        public PlayerInventory inventory => gameDataBase.playerInventory;
        
        private Dictionary<ItemConsumable, ShrineStatBtn> consumableBtns = new Dictionary<ItemConsumable, ShrineStatBtn>();
        
        public static readonly Evt OnUpdateDisplay = new Evt();

        private void Awake()
        {
            confirmMenu.Initialize(this);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            CreateList();
            OnUpdateDisplay.AddListener(CreateList);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            ClearList();
            OnUpdateDisplay.RemoveListener(CreateList);
        }
        
        private ItemConsumable[] GetConsumableInInventory()
        {
            return inventory.GetItemsInStorageByType(ItemType.Consumable).Select(i => i as ItemConsumable).ToArray();
        }

        public void CreateList()
        {
            confirmMenu.gameObject.SetActive(false);
            var _consumable = GetConsumableInInventory();

            ClearList();

            foreach (var _con in _consumable)
            {
                if(_con == null) continue;
                var _btn = Instantiate(shrineStatBtnPrefab, shrineStatBtnParent);
                _btn.SetItem(_con);
                consumableBtns.Add(_con, _btn);
                
                _btn.GetComponent<Button>().onClick.AddListener(() => OnClick(_btn));
            }
            shrineStatsDisplay.UpdateDisplay();
            errorTxt.gameObject.SetActive(consumableBtns.Count <= 0);
            if(consumableBtns.Count <= 0) return;
            
            EventSystem.current.SetSelectedGameObject(consumableBtns.First().Value.gameObject);
            Canvas.ForceUpdateCanvases();
        }

        private void ClearList()
        {
            foreach (var _btn in consumableBtns)
            {
                Destroy(_btn.Value.gameObject);
            }
            consumableBtns.Clear();
        }
        
        public void OfferConsumable(ItemConsumable itemConsumable_, int count_)
        {
            if (!CanStillOffer(itemConsumable_, count_))
            {
                Debug.Log("Can't offer");
                return;
            }
            var _result = shrineData.OfferConsumable(itemConsumable_, count_);
            
            confirmMenu.gameObject.SetActive(false);
            CreateList();
            shrineStatsDisplay.UpdateDisplay();
        }
        
        public bool CanStillOffer(ItemConsumable itemConsumable_, int tempOffer_)
        {
            if(tempOffer_ <= 0) return false;
            if(tempOffer_ > itemConsumable_.StackCount) return false;
            
            var _max = playerLevel * 10;
            var _data = itemConsumable_.Data as ConsumableData;
            var _statCount = boughtStats.GetStatByType(_data.GetStatType());

            return _statCount + tempOffer_ <= _max;
        }
        
        public bool CanStillOffer(ItemConsumable itemConsumable_,out string msg_)
        {
            msg_ = string.Empty;
            var _max = playerLevel * 10;
            
            var _data = itemConsumable_.Data as ConsumableData;
            var _statCount = boughtStats.GetStatByType(_data.GetStatType());

            if (_statCount < _max) return true;
            var _sb = new StringBuilder();
            _sb.Append("Maximum stats reached.");
            
            if (playerLevel < 10)
            {
                _sb.Append("\n");
                _sb.Append("Level up to increase limit.");
            }
            msg_ = _sb.ToString().Beautify();
            
            return false;

        }
        
        public int GetMaxCanOffer(ItemConsumable itemConsumable_)
        {
            var _max = playerLevel * 10;
            var _data = itemConsumable_.Data as ConsumableData;
            var _statCount = boughtStats.GetStatByType(_data.GetStatType());

            var _canOffer = _max - _statCount;
            return Mathf.Clamp(_canOffer, 0, itemConsumable_.StackCount);
        }

        public void OnClick(ShrineStatBtn btn_)
        {
            if(btn_.IsEmptyOrDestroyed() || btn_.item == null) CreateList();
            Debug.Log(GetMaxCanOffer(btn_.item));
            
            confirmMenu.DisplayConfirmation(btn_);
        }
    }
}
