using BaseCore;
using NaughtyAttributes;
using UnityEngine;

namespace StatusEffect
{
    public class HealPercentage_SE : StatusEffectBase
    {
        [Range(0,1f)]
        [SerializeField] private float healPercentage = 0.1f;

        protected override void OnActivate()
        {
            var _hpCom = Target.character.healthComponent;
            var _amount = Mathf.RoundToInt(_hpCom.MaxHp * healPercentage);
            
            HealInfo _healInfo = new HealInfo(_amount, Source);
            
            _hpCom.ReceiveHeal(_healInfo);
            
            SelfRemove();
        }
        
        protected override void OnDeactivate()
        {
            
        }
        
    }
}
