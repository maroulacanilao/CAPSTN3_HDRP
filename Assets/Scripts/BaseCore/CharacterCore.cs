using System;
using System.Collections;
using System.Collections.Generic;
using BattleSystem;
using Character;
using ScriptableObjectData.CharacterData;
using UnityEngine;

public abstract class CharacterCore : MonoBehaviour
{
    [field: SerializeField] public CharacterBase character { get; private set; }
    public BattleCharacter battleCharacter { get; private set; }
    public CharacterData characterData { get; private set; }

    protected void Awake()
    {
        if(character == null) character = GetComponentInParent<CharacterBase>();
        characterData = character.characterData;

        if (character.TryGetComponent(out BattleCharacter _battleCharacter))
        {
            battleCharacter = _battleCharacter;
        }
        Initialize();
    }

    protected abstract void Initialize();
}
