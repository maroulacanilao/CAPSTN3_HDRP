using System;
using System.Collections;
using System.Collections.Generic;
using BattleSystem;
using Character;
using CustomEvent;
using NaughtyAttributes;
using Spells.Base;
using UnityEngine;

public class SpellUser : MonoBehaviour
{
    [field: SerializeField] public CharacterBase character { get; private set; }
    [field: SerializeField] public BattleCharacter battleCharacter { get; private set; }
    [field: SerializeField] public int maxSkillSlot = 6; 
    [field: SerializeField] public List<SpellObject> spellList { get; private set; }
    private SpellObject activeSpell;

    private Transform spellContainer;

    private readonly Evt<SpellUser, SpellObject> OnAddSpell = new Evt<SpellUser, SpellObject>();

    private void Awake()
    {
        spellList = new List<SpellObject>();
        spellContainer = new GameObject("Spell Container").transform;
        spellContainer.SetParent(transform);
        spellContainer.localPosition = Vector3.zero;
    }

    public IEnumerator UseSkill(int index_, BattleCharacter target_)
    {
        if(spellList[index_] == null) yield break;
        
        var _spell = spellList[index_];
        
        if(character.manaComponent.UseMana(_spell.spellData.manaCost) == false) yield break;

        yield return _spell.Activate(target_);
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
    
    [Header("FOR DEBUGGING")]
    public SpellData[] testSpells;
    public int spellTestIndex = 0;

    private void Start()
    {
        // FOR DEBUGGING
        Debug.Log($"Test Spells Count : {testSpells.Length}");
        foreach (var _spell in testSpells)
        {
            var _newSpell = _spell.GetSpellObject(this);
            Debug.Log(_newSpell.gameObject);
            Debug.Log(_spell.spellName);
            spellList.Add(_newSpell);
            _newSpell.transform.SetParent(spellContainer);
        }
    }
    
    [Button("Use Spell #")]
    public void DebugUseSpell()
    {
        StartCoroutine(UseSkill(spellTestIndex, BattleManager.Instance.enemy));
    }
}
