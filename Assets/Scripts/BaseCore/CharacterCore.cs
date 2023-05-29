using BattleSystem;
using Character;
using ScriptableObjectData.CharacterData;
using UnityEngine;

namespace BaseCore
{
    public abstract class CharacterCore
    {
        [field: SerializeField] public CharacterBase character { get; private set; }
        public BattleCharacter battleCharacter { get; private set; }
        public CharacterData characterData { get; private set; }

    
        public CharacterCore(CharacterBase character_)
        {
            character = character_;
            characterData = character_.characterData;
            if (character.TryGetComponent(out BattleCharacter _battleCharacter))
            {
                battleCharacter = _battleCharacter;
            }
        }
    
        public virtual void OnCharacterEnable() {}
    }
}
