using System.Collections;
using System.Collections.Generic;
using BattleSystem.BattleState;
using Character;
using UnityEngine;

public class BonusStatFixed_SE : StatusEffectBase
{
    [SerializeField] private bool IsBeforeTick;
    [SerializeField] private CombatStats bonusStat;
    
    protected override void OnActivate()
    {
        Target.character.statsData.AddBonusStats(bonusStat);
    }
    
    protected override void OnDeactivate()
    {
        Target.character.statsData.RemoveBonusStats(bonusStat);
    }

    protected override IEnumerator OnAfterTurnTick(TurnBaseState ownerTurnState_)
    {
        if(!IsBeforeTick) RemoveTurn();
        yield break;
    }
    
    
    protected override IEnumerator OnBeforeTurnTick(TurnBaseState ownerTurnState_)
    {
        if(IsBeforeTick) RemoveTurn();
        yield break;
    }
    
    public void SetStat(CombatStats stat_)
    {
        bonusStat = stat_;
    }
}
