using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using ScriptableObjectData;
using Shop;
using Spells.Base;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.ShrineUI.GetSpells
{
    public class Shrine_GetSpells : ShrineMenu
    {
        [SerializeField] private Transform contentParent;
        [SerializeField] private Shrine_SpellDetails spellDetails;
        [SerializeField] private ShrineSpellItem spellItemPrefab;
        [SerializeField] private ConsumableDisplay consumableDisplay;
        private SpellDataBase spellDataBase => gameDataBase.spellDataBase;
        
        private SerializedDictionary<SpellData,OfferRequirement> spellOfferList;
        
        private readonly List<ShrineSpellItem> spellItems = new List<ShrineSpellItem>();

        private void Awake()
        {
            spellOfferList = new SerializedDictionary<SpellData, OfferRequirement>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            CreateSpellOffers();
            ShrineSpellItem.OnClickSpell.AddListener(OnClickSpell);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            ShrineSpellItem.OnClickSpell.RemoveListener(OnClickSpell);
        }

        private void CreateSpellOffers()
        {
            spellOfferList.Clear();
            RemoveAllItems();
            
            spellOfferList = new SerializedDictionary<SpellData, OfferRequirement>(spellDataBase.spellOffers);

            foreach (var _spell in gameDataBase.playerData.spells)
            {
                spellOfferList.Remove(_spell);
            }
            
            foreach (var _spellOffer in spellOfferList)
            {
                var _spellItem = Instantiate(spellItemPrefab, contentParent);
                _spellItem.Set(_spellOffer.Key,_spellOffer.Value);
                spellItems.Add(_spellItem);
            }
            
            SelectFirstSpell();
            consumableDisplay.UpdateDisplay();
        }
        
        private void OnClickSpell(ShrineSpellItem spellBtn_)
        {
            spellDetails.DisplaySpell(spellBtn_);
        }

        private void SelectFirstSpell()
        {
            if (spellItems.Count > 0)
            {
                var _btn = spellItems[0];
                _btn.SelectButton();
                EventSystem.current.SetSelectedGameObject(_btn.gameObject);
                spellDetails.DisplaySpell(_btn);
                errorTxt.gameObject.SetActive(false);
            }
            else
            {
                errorTxt.text = "No more spells available";
                errorTxt.gameObject.SetActive(true);
                spellDetails.DisplayNull();
            }
        }
        
        public bool CanLearn(SpellData spellData_, out string message)
        {
            message = string.Empty;
            if (!spellOfferList.TryGetValue(spellData_, out var _offer))
            {
                message = "Spell already learned";
                return false;
            }
            
            if(_offer.consumableData == null) throw new Exception("Offer requirement is null");
            
            var _requirementData = _offer.consumableData;
            
            if(!inventory.StackableDictionary.TryGetValue(_requirementData, out var _stackable))
            {
                message = "You don't have the required item";
                return false;
            }
            
            if(_stackable.StackCount < _offer.count)
            {
                message = "You don't have enough of the required item";
                return false;
            }
            
            return true;
        }
        
        public void LearnSpell(SpellData spellData_)
        {
            errorTxt.gameObject.SetActive(false);
            if(!CanLearn(spellData_, out var _message))
            {
                errorTxt.text = _message;
                errorTxt.gameObject.SetActive(true);
                return;
            }
            
            if (!spellOfferList.TryGetValue(spellData_, out var _requirement)) return;
            
            if(_requirement.consumableData == null) throw new Exception("Offer requirement is null");
            
            var _requirementData = _requirement.consumableData;
            
            if(!inventory.StackableDictionary.TryGetValue(_requirementData, out var _stackable)) return;
            
            if(_stackable.StackCount >= _requirement.count) inventory.RemoveStackable(_stackable, _requirement.count);

            gameDataBase.playerData.spells.Add(spellData_);
            spellOfferList.Remove(spellData_);
            CreateSpellOffers();
        }

        private void RemoveAllItems()
        {
            foreach (var _spellItem in spellItems)
            {
                Destroy(_spellItem.gameObject);
            }
            
            spellItems.Clear();
        }
    }
}
