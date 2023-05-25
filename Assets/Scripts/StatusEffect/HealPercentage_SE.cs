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
            var _hpCom = Target.character.health;
            var _amount = Mathf.RoundToInt(_hpCom.MaxHp * healPercentage);
            
            HealInfo _healInfo = new HealInfo(_amount, Source);
            
            Target.character.Heal(_healInfo);
            
            SelfRemove();
        }
        
        protected override void OnDeactivate()
        {
            
        }
        
    }
}
