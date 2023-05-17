using System;
using System.Collections.Generic;
using System.Linq;
using CustomEvent;
using UnityEngine;

namespace Character
{
    public class StatusEffectReceiver : MonoBehaviour
    {
        [field: SerializeField] public CharacterBase owner { get; private set; }
        private Transform container;

        public Evt<StatusEffectBase, StatusEffectReceiver> OnApply = new Evt<StatusEffectBase, StatusEffectReceiver>();
        public Evt<StatusEffectBase, StatusEffectReceiver> OnRemove = new Evt<StatusEffectBase, StatusEffectReceiver>();
        private Dictionary<int, StatusEffectBase> StatusEffectsDictionary;

        private void Start()
        {
            if (owner == null) owner = transform.parent.GetComponent<CharacterBase>();
            
            StatusEffectsDictionary = new Dictionary<int, StatusEffectBase>();
            container = new GameObject("StatusEffect Container").transform;
            container.parent = gameObject.transform;
            container.localPosition = Vector3.zero;
        }

        public bool ApplyStatusEffect(StatusEffectBase effect_, GameObject source_ = null)
        {
            // if stackable and status effect already in effect
            if (effect_.IsStackable && StatusEffectsDictionary.ContainsKey(effect_.ID))
            {
                var _effect = StatusEffectsDictionary[effect_.gameObject.GetInstanceID()];
                // _effect.StackEffect(effect_);
                return true;
            }

            var _effectInstance = Instantiate(effect_, Vector3.zero, Quaternion.identity, container);

            StatusEffectsDictionary.Add(effect_.ID, _effectInstance);

            _effectInstance.Activate(this, source_);
            OnApply?.Invoke(_effectInstance, this);
            return true;
        }

        public void RemoveStatusEffect(int effectID_)
        {
            var _effect = StatusEffectsDictionary[effectID_];
            StatusEffectsDictionary.Remove(effectID_);
            _effect.Deactivate();
            OnRemove?.Invoke(_effect, this);
        }

        public void RemoveStatusEffect(StatusEffectBase effect_)
        {
            StatusEffectsDictionary.Remove(effect_.gameObject.GetInstanceID());
            effect_.Deactivate();
            OnRemove?.Invoke(effect_, this);
        }

        public void RemoveEffectByType<T>()
            where T : StatusEffectBase
        {
            foreach (var effect in StatusEffectsDictionary.Values.OfType<T>())
            {
                RemoveStatusEffect(effect);
            }
        }

        public bool IsDuplicateByType<T>()
            where T : StatusEffectBase
        {
            return StatusEffectsDictionary.Values.OfType<T>().Any();
        }
    }
}