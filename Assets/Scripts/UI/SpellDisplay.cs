using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using ScriptableObjectData.CharacterData;
using Spells.Base;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpellDisplay : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI spellNameText, spellTypeText, spellCostText, spellDescText, spellDamageText;

    [SerializeField] protected Image spellIcon;

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

    private void Awake()
    {
        DisplayNull();
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

    // NEW SPELL DISPLAY FUNCTION
    public virtual void DisplaySkill(int index_, SpellData spellData_)
    {
        if (spellIcon != null)
        {
            spellIcon.sprite = spellData_.icon;
            spellIcon.gameObject.SetActive(true);
        }
        if (spellNameText != null) spellNameText.text = spellData_.spellName;
        if (spellCostText != null) spellCostText.text = $"-{spellData_.manaCost}mp";
        if (spellDescText != null) spellDescText.text = spellData_.Description;

        if (index_ == 0)
        {
            var _dmg = Mathf.RoundToInt(spellData_.damageModifier * playerData.totalStats.intelligence);
            spellDamageText.text = $"{_dmg}dmg";
        }
        else
        {
            var _dmg = Mathf.RoundToInt(spellData_.damageModifier * playerData.totalPartyData[index_ - 1].totalStats.intelligence);
            spellDamageText.text = $"{_dmg}dmg";
        }
    }

    public virtual void DisplayNull()
    {
        if (spellIcon != null) spellIcon.gameObject.SetActive(false);
        if (spellNameText != null) spellNameText.text = "No Spell Selected";
        if (spellTypeText != null) spellTypeText.text = "Spell Type: ???";
        if (spellCostText != null) spellCostText.text = "Mana Cost: ???";
        if (spellDescText != null) spellDescText.text = "???";
        if (spellDamageText != null) spellDamageText.text = "Damage: ???";
    }
}
