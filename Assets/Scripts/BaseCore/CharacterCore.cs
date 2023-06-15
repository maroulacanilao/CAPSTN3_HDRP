using BattleSystem;
using Character;
using ScriptableObjectData.CharacterData;
using UnityEngine;

namespace BaseCore
{
    public abstract class CharacterCore
    {
        [field: SerializeField] public CharacterBase character { get; protected set; }
        public BattleCharacter battleCharacter { get; protected set; }
        public CharacterData characterData { get; protected set; }

    
        public CharacterCore(CharacterBase character_)
        {
            if(character_ == null) return;
            
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
