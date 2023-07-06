using System;
using System.Collections.Generic;
using Character.CharacterComponents;
using UnityEngine;

namespace UI.HUD
{
    public class StatusEffectHUD : MonoBehaviour
    {
        [SerializeField] private EffectIconItem effectIconItemPrefab;
        [SerializeField] private Transform effectParent;

        private StatusEffectReceiver _statusEffectReceiver;
        private Dictionary<int, EffectIconItem> effectIcons;
        
        private bool hasInitialized;
        
        public void Initialize(StatusEffectReceiver statusEffectReceiver_)
        {
            _statusEffectReceiver = statusEffectReceiver_;
            effectIcons = new Dictionary<int, EffectIconItem>();
            
            foreach (var statusEffect in _statusEffectReceiver.StatusEffectsDictionary.Values)
            {
                AddEffectIcon(statusEffect);
            }
            
            _statusEffectReceiver.OnApply.AddListener(AddEffectIcon);
            _statusEffectReceiver.OnRemove.AddListener(RemoveEffectIcon);
            
            hasInitialized = true;
        }

        private void OnDestroy()
        {
            if(!hasInitialized) return;
            
            _statusEffectReceiver.OnApply.RemoveListener(AddEffectIcon);
            _statusEffectReceiver.OnRemove.RemoveListener(RemoveEffectIcon);
        }

        private void AddEffectIcon(StatusEffectBase statusEffectBase_)
        {
            if(effectIcons.ContainsKey(statusEffectBase_.ID)) return;
            if(statusEffectBase_.Icon == null) return;
            
            var _effectIconItem = Instantiate(effectIconItemPrefab, effectParent);
            
            _effectIconItem.SetIcon(statusEffectBase_);
            
            effectIcons.Add(statusEffectBase_.ID, _effectIconItem);
        }
        
        private void RemoveEffectIcon(StatusEffectBase statusEffectBase_)
        {
            if(!effectIcons.ContainsKey(statusEffectBase_.ID)) return;
            Destroy(effectIcons[statusEffectBase_.ID].gameObject);
            effectIcons.Remove(statusEffectBase_.ID);
        }
    }
}
