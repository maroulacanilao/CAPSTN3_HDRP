using System;
using System.Collections.Generic;
using ScriptableObjectData.CharacterData;
using Spells.Base;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.TabMenu.CharacterInfo
{
    public class SkillInfoPanel : MonoBehaviour
    {
        [SerializeField] private PlayerData playerData;
        [SerializeField] private SpellDisplay spellDisplay;

        [field: SerializeField] public List<SpellData> SpellsList { get; private set; }

        private void Awake()
        {
            SpellsList = new();
        }

        public void ShowSpellInfo(int index_, int spellIndex_)
        {
            spellDisplay.DisplaySkill(index_, SpellsList[spellIndex_]);
        }

        public void CreateList(int index_)
        {
            SpellsList.Clear();

            if (index_ == 0)
            {
                for (var _i = 0; _i < playerData.spells.Count; _i++)
                {
                    var _spell = playerData.spells[_i];
                    if (_spell == null) continue;

                    SpellsList.Add(_spell);
                }
            }
            else
            {
                for (var _i = 0; _i < playerData.totalPartyData[index_ - 1].spells.Count; _i++)
                {
                    var _spell = playerData.totalPartyData[index_ - 1].spells[_i];
                    if (_spell == null) continue;

                    SpellsList.Add(_spell);
                }
            }

            ShowSpellInfo(index_, 0);
        }
    }
}
