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
        
    }
}
