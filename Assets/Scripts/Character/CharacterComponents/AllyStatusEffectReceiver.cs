using AYellowpaper.SerializedCollections;
using BattleSystem;
using Managers;
using ScriptableObjectData.CharacterData;
using UnityEngine;

namespace Character.CharacterComponents
{
    public class AllyStatusEffectReceiver : StatusEffectReceiver
    {
        public AllyStatusEffectReceiver(AllyData allyData_) : base(null)
        {
            characterData = allyData_;
            StatusEffectsDictionary = new SerializedDictionary<int, StatusEffectBase>();
            container = new GameObject("StatusEffect Container").transform;
            container.parent = GameManager.Instance.transform;
            container.localPosition = Vector3.zero;
        }

        public void SetCharacter(CharacterBase characterBase_)
        {
            character = characterBase_;
            
            if (character.TryGetComponent(out BattleCharacter _battleCharacter))
            {
                battleCharacter = _battleCharacter;
            }
        }
    }
}