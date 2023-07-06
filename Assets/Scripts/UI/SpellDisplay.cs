using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using ScriptableObjectData.CharacterData;
using Spells.Base;
using TMPro;
using UnityEngine;

public class SpellDisplay : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI spellNameText, spellTypeText, spellCostText, spellDescText, spellDamageText;

    private PlayerData mPlayerData;
    protected PlayerData playerData
    {
        get
        {
            if (mPlayerData == null)
            {
                mPlayerData = GameManager.Instance.GameDataBase.playerData;
            }
            return mPlayerData;
        }
    }
    
    public virtual void DisplaySpell(SpellData spellData_)
    {
        spellNameText.text = spellData_.spellName;
        spellTypeText.text = $"Spell Type: {spellData_.spellType}";
        spellCostText.text = $"Mana Cost: {spellData_.manaCost}";
        spellDescText.text = spellData_.Description;

        var _dmg = Mathf.RoundToInt(spellData_.damageModifier * playerData.totalStats.intelligence);
        spellDamageText.text = $"Damage: {_dmg}";
    }
    
    public virtual void DisplaySpell(SpellObject spellObject_)
    {
        spellNameText.text = spellObject_.spellData.spellName;
        spellTypeText.text = $"Spell Type: {spellObject_.spellData.spellType}";
        spellCostText.text = $"Mana Cost: {spellObject_.spellData.manaCost}";
        spellDescText.text = spellObject_.spellData.Description;
        spellDamageText.text = $"Damage: {spellObject_.damage}";
    }
}
