using System;
using System.Collections.Generic;
using BaseCore;
using Character;
using Spells.Base;
using UnityEngine;
using AYellowpaper.SerializedCollections;
using CustomHelpers;

namespace BattleSystem
{
    [System.Serializable]
    public enum AIActionType { BasicAttack, SpellAttack, Heal, Buff, DeBuff }

    [System.Serializable]
    public struct AICombatAction
    {
        public AIActionType actionType;
        public SpellData spellData;
    }
    
    [System.Serializable]
    public class EnemyCombatTendency
    {
        [field: SerializeField] public WeightedDictionary<AIActionType> actionTypeWeights { get; private set; }
        [field: SerializeField] public SpellData[] spells { get; private set; }

        private SerializedDictionary<SpellType, List<SpellData>> spellDictionary;

        public AICombatAction GetAction(CharacterBase character_)
        {
            var _enemyAction = new AICombatAction();
            var _possibleActions = actionTypeWeights.Clone();
            
            if(character_.health.HpPercentage <= 0.4f)
            {
                _possibleActions.RemoveItem(AIActionType.Heal);
            }
            
            var _spells = GetPossibleSpells(character_.mana.CurrentMana);
            
            if(!_spells.ContainsKey(SpellType.Magical) && !_spells.ContainsKey(SpellType.Physical)) _possibleActions.RemoveItem(AIActionType.SpellAttack);
            if(!_spells.ContainsKey(SpellType.Heal)) _possibleActions.RemoveItem(AIActionType.Heal);
            if(!_spells.ContainsKey(SpellType.Buff)) _possibleActions.RemoveItem(AIActionType.Buff);
            if(!_spells.ContainsKey(SpellType.DeBuff)) _possibleActions.RemoveItem(AIActionType.DeBuff);
            
            _possibleActions.RecalculateChances();
            _enemyAction.actionType = _possibleActions.GetWeightedRandom();
            
            switch (_enemyAction.actionType)
            {
                case AIActionType.BasicAttack:
                    break;
                case AIActionType.SpellAttack:
                    if(_spells.TryGetValue(SpellType.Magical, out var _spell))
                        _enemyAction.spellData = _spell.GetRandomItem();
                    else if (_spells.TryGetValue(SpellType.Physical, out var _spell1))
                        _enemyAction.spellData = _spell1.GetRandomItem();
                    break;
                case AIActionType.Heal:
                    _enemyAction.spellData = _spells[SpellType.Heal].GetRandomItem();
                    break;
                case AIActionType.Buff:
                    _enemyAction.spellData = _spells[SpellType.Buff].GetRandomItem();
                    break;
                case AIActionType.DeBuff:
                    _enemyAction.spellData = _spells[SpellType.DeBuff].GetRandomItem();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            return _enemyAction;
        }

        public void SortSpells()
        {
            spellDictionary = new SerializedDictionary<SpellType, List<SpellData>>();
            spellDictionary.Add(SpellType.Physical, new List<SpellData>());
            spellDictionary.Add(SpellType.Magical, new List<SpellData>());
            spellDictionary.Add(SpellType.Heal, new List<SpellData>());
            spellDictionary.Add(SpellType.Buff, new List<SpellData>());
            spellDictionary.Add(SpellType.DeBuff, new List<SpellData>());
            
            foreach (var _spell in spells)
            {
                spellDictionary[_spell.spellType].Add(_spell);
            }
        }
        
        public SerializedDictionary<SpellType, List<SpellData>>GetPossibleSpells(int _currentMana)
        {
            if (spellDictionary == null) SortSpells();
            
            var _spellDictionary = new SerializedDictionary<SpellType, List<SpellData>>(spellDictionary);

            foreach (var _spellPair in _spellDictionary)
            {
                _spellPair.Value.RemoveAll(spell_ => spell_.manaCost > _currentMana);
            }


            if(_spellDictionary[SpellType.Physical].Count <= 0) _spellDictionary.Remove(SpellType.Physical);
            if(_spellDictionary[SpellType.Magical].Count <= 0) _spellDictionary.Remove(SpellType.Magical);
            if(_spellDictionary[SpellType.Heal].Count <= 0) _spellDictionary.Remove(SpellType.Heal);
            if(_spellDictionary[SpellType.Buff].Count <= 0) _spellDictionary.Remove(SpellType.Buff);
            if(_spellDictionary[SpellType.DeBuff].Count <= 0) _spellDictionary.Remove(SpellType.DeBuff);

            return _spellDictionary;
        }
    }
}
