using System.Collections;
using System.Collections.Generic;
using System.Text;
using BattleSystem.BattleState;
using Character;
using Managers;
using UI.Battle;
using UnityEngine;

public class BonusStatFixed_SE : StatusEffectBase
{
    [SerializeField] private bool IsBeforeTick;
    [SerializeField] private CombatStats bonusStat;
    
    protected override IEnumerator OnActivate()
    {
        Target.character.statsData.AddBonusStats(bonusStat);
        
        if(!GameManager.IsBattleSceneActive()) yield break;
        
        var _sb = new StringBuilder();
        var _name = Target.character.characterData.characterName;
        
        var _msg = BattleText.Replace("NAME", _name);
        
        yield return BattleTextManager.DoWrite(_msg);
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
    
    protected override void OnStackEffect(StatusEffectBase newEffect_)
    {
        SetDurationLeft(turnsLeft + turnDuration);
    }
}
