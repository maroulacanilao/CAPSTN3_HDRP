using System.Collections;
using BaseCore;
using BattleSystem;
using BattleSystem.BattleState;
using CustomHelpers;
using UnityEngine;

namespace StatusEffect
{
    public class BurnPercentage_SE : StatusEffectBase
    {
        [Range(0,1f)] [SerializeField] private float damagePercentage = 0.1f;
        protected override void OnActivate()
        {
        
        }
    
        protected override void OnDeactivate()
        {
        
        }
        
        protected override void OnStackEffect(StatusEffectBase newEffect_)
        {
            RefreshStatusEffect();
        }
        
        protected override IEnumerator OnBeforeTurnTick(TurnBaseState ownerTurnState_)
        {
            var _hpCom = Target.character.health;
            var _amount = Mathf.RoundToInt(_hpCom.MaxHp * damagePercentage);
            turns++;
            
            DamageInfo _damageInfo = new DamageInfo(_amount, Source);
            AttackResult _attackResult = new AttackResult()
            {
                attackResultType = AttackResultType.Hit,
                damageInfo = _damageInfo
            };
            Target.battleCharacter.Hit(_attackResult);
            yield return CoroutineHelper.GetWait(0.5f);
            Debug.Log($"{Target.CharacterName} was burned for {_amount} damage");
            yield return CoroutineHelper.GetWait(0.2f);

        }
    }
}
