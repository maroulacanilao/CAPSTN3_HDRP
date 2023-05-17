using TMPro;
using UnityEngine;

namespace UI.Battle
{
    public class SpellDetailsPanel : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI spellNameText, spellTypeText, spellCostText, spellDescText, spellDamageText;
        
        public void ShowPanel(SpellBtnItemUI spellBtn_)
        {
            var _data = spellBtn_.spellData;
            var _spell = spellBtn_.spellObject;
            
            spellNameText.text = _data.spellName;
            spellTypeText.text = $"Spell Type: {_data.spellType}";
            spellCostText.text = $"Mana: {_data.manaCost}";
            spellDescText.text = _data.Description;
            spellDamageText.text = $"Damage: {_spell.damage}";
            
            gameObject.SetActive(true);
        }
        
    }
}
