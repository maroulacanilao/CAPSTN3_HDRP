using System.Collections;
using BaseCore;
using BattleSystem;
using BattleSystem.BattleState;
using CustomHelpers;
using UI.Battle;
using UnityEngine;

namespace StatusEffect
{
    public class BurnHpPercentage_SE : StatusEffectBase
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
            SetDurationLeft(turnsLeft + turnDuration);
        }
        
        protected override IEnumerator OnBeforeTurnTick(TurnBaseState ownerTurnState_)
        {
            var _hpCom = Target.character.health;
            var _amount = Mathf.RoundToInt(_hpCom.MaxHp * damagePercentage);
            
            RemoveTurn();
            
            DamageInfo _damageInfo = new DamageInfo(_amount, Source, effectTags);
            AttackResult _attackResult = new AttackResult()
            {
                attackResultType = AttackResultType.Hit,
                damageInfo = _damageInfo
            };
            Target.battleCharacter.Hit(_attackResult);

            var _msg = BattleText
                .Replace("NAME", characterName)
                .Replace("DMG", _damageInfo.DamageAmount.ToString());

            yield return null;
            
            yield return BattleTextManager.DoWrite(_msg);
            
            yield return CoroutineHelper.GetWait(0.1f);

        }
    }
}
