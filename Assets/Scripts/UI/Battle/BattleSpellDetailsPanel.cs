using Spells.Base;
using TMPro;
using UnityEngine;

namespace UI.Battle
{
    public class BattleSpellDetailsPanel : SpellDisplay
    {
        public void ShowPanel(SpellBtnItemUI spellBtn_)
        {
            DisplaySpell(spellBtn_.spellObject);
            gameObject.SetActive(true);
        }

        public override void DisplaySpell(SpellData spellData_)
        {
            spellTypeText.text = $"Spell Type: {spellData_.spellType}";
            spellCostText.text = $"{spellData_.manaCost} MP";
            spellDescText.text = spellData_.Description;

            var _dmg = Mathf.RoundToInt(spellData_.damageModifier * playerData.totalStats.intelligence);
            spellDamageText.text = $"Damage: {_dmg}";
        }
    }
}
