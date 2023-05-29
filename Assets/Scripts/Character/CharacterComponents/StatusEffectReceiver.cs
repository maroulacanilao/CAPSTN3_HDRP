using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using BaseCore;
using BattleSystem.BattleState;
using CustomEvent;
using CustomHelpers;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Character.CharacterComponents
{
    
    [System.Serializable]
    public class StatusEffectReceiver : CharacterCore
    {
        private Transform container;
        
        public string CharacterName => character.characterData.characterName;

        public Evt<StatusEffectBase, StatusEffectReceiver> OnApply = new Evt<StatusEffectBase, StatusEffectReceiver>();
        public Evt<StatusEffectBase, StatusEffectReceiver> OnRemove = new Evt<StatusEffectBase, StatusEffectReceiver>();
        
        private SerializedDictionary<int, StatusEffectBase> StatusEffectsDictionary;

        public StatusEffectReceiver(CharacterBase character_) : base(character_)
        {
            StatusEffectsDictionary = new SerializedDictionary<int, StatusEffectBase>();
            container = new GameObject("StatusEffect Container").transform;
            container.parent = character.transform;
            container.localPosition = Vector3.zero;
        }
        
        ~StatusEffectReceiver()
        {
            foreach (var _effect in StatusEffectsDictionary.Values)
            {
                Object.Destroy(_effect.gameObject);
            }
        }

        public bool ApplyStatusEffect(StatusEffectBase effect_, GameObject source_ = null)
        {
            if (StatusEffectsDictionary.TryGetValue(effect_.ID, out var _effect))
            {
                if (!_effect.IsStackable) return false;
                
                _effect.StackEffect(effect_);
                return true;
            }
            effect_.transform.ResetTransformation();
            effect_.transform.SetParent(container);
            
            StatusEffectsDictionary.Add(effect_.ID, effect_);

            effect_.Activate(this, source_);
            OnApply?.Invoke(effect_, this);
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
        
        public void RemoveAllStatusEffect()
        {
            foreach (var _effect in StatusEffectsDictionary.Values.ToList())
            {
                RemoveStatusEffect(_effect);
            }
        }
        
        public void TransferStatusEffect(StatusEffectReceiver target_)
        {
            foreach (var _effect in StatusEffectsDictionary.Values.ToList())
            {
                target_.ApplyStatusEffect(_effect);
            }
        }
    }
}