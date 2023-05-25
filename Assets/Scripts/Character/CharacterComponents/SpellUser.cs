using System;
using System.Collections;
using System.Collections.Generic;
using BattleSystem;
using Character;
using CustomEvent;
using NaughtyAttributes;
using ScriptableObjectData.CharacterData;
using Spells.Base;
using UnityEngine;

public class SpellUser : CharacterCore
{
    [field: SerializeField] public int maxSkillSlot = 6; 
    [field: SerializeField] public List<SpellObject> spellList { get; private set; }
    private SpellObject activeSpell;

    private Transform spellContainer;

    private readonly Evt<SpellUser, SpellObject> OnAddSpell = new Evt<SpellUser, SpellObject>();
    protected override void Initialize()
    {
        spellList = new List<SpellObject>();
        spellContainer = new GameObject("Spell Container").transform;
        spellContainer.SetParent(transform);
        spellContainer.localPosition = Vector3.zero;
        
        if(character.characterData.spells.Count <= 0) return;
        
        foreach (var _spell in character.characterData.spells)
        {
            var _newSpell = _spell.GetSpellObject(this);
            spellList.Add(_newSpell);
            _newSpell.transform.SetParent(spellContainer);
        }
    }

    public IEnumerator UseSkill(int index_, BattleCharacter target_)
    {
        if(spellList[index_] == null) yield break;
        
        var _spell = spellList[index_];
        
        if(character.mana.UseMana(_spell.spellData.manaCost) == false) yield break;

        yield return _spell.Activate(target_);
    }

    public IEnumerator UseSkill(SpellData spellData_, BattleCharacter target_)
    {
        var _spell = spellData_.GetSpellObject(this);
        yield return null;

        character.mana.UseMana(_spell.spellData.manaCost);
        
        yield return _spell.Activate(target_);
        
        yield return null;
        Destroy(_spell.gameObject);
    }

    public void AddSpell(SpellData spellData_, int slot_)
    {
        if(slot_ > maxSkillSlot) throw new Exception("Slot is out of range");
        if(slot_ < 0) throw new Exception("Slot is out of range");
        
        if (slot_ >= spellList.Count)
        {
            AddSpell(spellData_);
            return;
        }
        
        if (spellList[slot_] == null)
        {
            Destroy(spellList[slot_].gameObject);
        }

        spellList[slot_] = spellData_.GetSpellObject(this);
        spellList[slot_].transform.SetParent(spellContainer);
        OnAddSpell.Invoke(this,spellList[slot_]);
        
        if(spellList[slot_].spellData.spellType != SpellType.Passive) return;
        
        spellList[slot_].StartCoroutine(spellList[slot_].Activate(battleCharacter));
    }
    
    public void AddSpell(SpellData spellData_)
    {
        var _newSpell = spellData_.GetSpellObject(this);
        _newSpell.transform.SetParent(spellContainer);
        spellList.Add(_newSpell);
    }
    
    public void RemoveSkill(int slot_)
    {
        spellList[slot_].RemoveSkill();
        spellList[slot_] = null;
    }
    
    public void RemoveSkill(SpellObject spell_)
    {
        int _slot = spellList.IndexOf(spell_);
        RemoveSkill(_slot);
    }
}
