using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BattleSystem;
using BattleSystem.BattleState;
using CustomEvent;
using CustomHelpers;
using UnityEngine;

namespace Character
{
    public class StatusEffectReceiver : CharacterCore
    {
        private Transform container;
        
        public string CharacterName => character.characterData.characterName;

        public Evt<StatusEffectBase, StatusEffectReceiver> OnApply = new Evt<StatusEffectBase, StatusEffectReceiver>();
        public Evt<StatusEffectBase, StatusEffectReceiver> OnRemove = new Evt<StatusEffectBase, StatusEffectReceiver>();
        private Dictionary<int, StatusEffectBase> StatusEffectsDictionary;

        protected override void Initialize()
        {
            StatusEffectsDictionary = new Dictionary<int, StatusEffectBase>();
            container = new GameObject("StatusEffect Container").transform;
            container.parent = gameObject.transform;
            container.localPosition = Vector3.zero;
        }
        
        public bool ApplyStatusEffect(StatusEffectBase effect_, GameObject source_ = null)
        {
            // if stackable and status effect already in effect
            if (effect_.IsStackable && StatusEffectsDictionary.TryGetValue(effect_.ID, out var _effect))
            {
                _effect.StackEffect(effect_);
                return true;
            }
            if(!effect_.IsStackable && StatusEffectsDictionary.ContainsKey(effect_.ID)) return false;
            
            var _effectInstance = Instantiate(effect_, Vector3.zero, Quaternion.identity, container);

            StatusEffectsDictionary.Add(effect_.ID, _effectInstance);

            _effectInstance.Activate(this, source_);
            OnApply?.Invoke(_effectInstance, this);
            return true;
        }

        public void RemoveStatusEffect(int effectID_)
        {
            var _effect = StatusEffectsDictionary[effectID_];
            RemoveStatusEffect(_effect);
        }

        public void RemoveStatusEffect(StatusEffectBase effect_)
        {
            StatusEffectsDictionary.Remove(effect_.ID);
            effect_.Deactivate();
            OnRemove?.Invoke(effect_, this);
        }
        
        public IEnumerator BeforeTurnTick(TurnBaseState ownerTurnState_)
        {
            if(StatusEffectsDictionary.Count <= 0) yield break;
            // PurgeNulls();
            yield return null;
            foreach (var _effect in StatusEffectsDictionary.Values.ToList())
            {
                yield return _effect.BeforeTurnTick(ownerTurnState_);
            }
        }
        
        public IEnumerator AfterTurnTick(TurnBaseState ownerTurnState_)
        {
            if(StatusEffectsDictionary.Count <= 0) yield break;
            PurgeNulls();
            yield return null;
            foreach (var _effect in StatusEffectsDictionary.Values.ToList())
            {
                yield return _effect.AfterTurnTick(ownerTurnState_);
            }
        }
        
        private void PurgeNulls()
        {
            var _nulls = StatusEffectsDictionary.Where(x => x.Value == null || x.Value.IsDestroyed()).ToList();
            foreach (var _null in _nulls)
            {
                StatusEffectsDictionary.Remove(_null.Key);
            }
        }
    }
}