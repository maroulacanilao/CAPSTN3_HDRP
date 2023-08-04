using System;
using System.Collections.Generic;
using ScriptableObjectData.CharacterData;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.TabMenu.CharacterInfo
{
    public class SpellInfoPanel : MonoBehaviour
    {
        [SerializeField] private PlayerData playerData;
        [SerializeField] private SpellDisplay spellDisplay;
        [SerializeField] private Transform contentParent;
        [SerializeField] private SpellInfoBtn spellInfoBtnPrefab;
        [SerializeField] private TextMeshProUGUI errorTxt;
        
        List<SpellInfoBtn> spellInfoBtnList = new List<SpellInfoBtn>();

        private void OnEnable()
        {
            CreateList();
            SpellInfoBtn.OnClick.AddListener(ShowSpellInfo);
        }

        private void OnDisable()
        {
            SpellInfoBtn.OnClick.RemoveListener(ShowSpellInfo);
        }
        
        private void ShowSpellInfo(SpellInfoBtn spell_)
        {
            spellDisplay.DisplaySpell(spell_.spellData);
        }

        private void CreateList()
        {
            RemoveList();
            for (var _i = 0; _i < playerData.spells.Count; _i++)
            {
                var _spell = playerData.spells[_i];
                if(_spell == null) continue;
                
                var _spellInfoBtn = Instantiate(spellInfoBtnPrefab, contentParent);
                _spellInfoBtn.Initialize(_spell);
                spellInfoBtnList.Add(_spellInfoBtn);
            }

            if (spellInfoBtnList.Count <= 0)
            {
                spellDisplay.DisplayNull();
                errorTxt.gameObject.SetActive(true);
                return;
            }
            else
            {
                errorTxt.gameObject.SetActive(false);
            }
            
            EventSystem.current.SetSelectedGameObject(spellInfoBtnList[0].gameObject);
            spellInfoBtnList[0].SelectButton();
        }
        
        private void RemoveList()
        {
            foreach (var btn in spellInfoBtnList)
            {
                Destroy(btn.gameObject);
            }
            spellInfoBtnList.Clear();
        }
    }
}
