using System.Collections;
using BaseCore;
using BattleSystem;
using BattleSystem.BattleState;
using CustomHelpers;
using UI.Battle;
using UnityEngine;

namespace StatusEffect
{
    public class BurnFixedDamage_SE : StatusEffectBase
    {
        [SerializeField] private int damage = 1;
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
    
        public void SetDamage(int damage_) =>  damage = damage_;

        protected override IEnumerator OnBeforeTurnTick(TurnBaseState ownerTurnState_)
        {
            DamageInfo _damageInfo = new DamageInfo(damage, Source, effectTags);
            AttackResult _attackResult = new AttackResult()
            {
                attackResultType = AttackResultType.Hit,
                damageInfo = _damageInfo
            };
            Target.battleCharacter.Hit(_attackResult);
            
            RemoveTurn();
            
            var _msg = BattleText
                .Replace("NAME", characterName)
                .Replace("DMG", _damageInfo.DamageAmount.ToString());
            
            yield return null;
            
            yield return BattleTextManager.DoWrite(_msg);
            
            yield return CoroutineHelper.GetWait(0.1f);
        }
    }
}
