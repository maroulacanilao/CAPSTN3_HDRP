using System.Collections;
using System.Collections.Generic;
using System.Text;
using BattleSystem.BattleState;
using UI.Battle;
using UnityEngine;

public class SkipTurn_SE : StatusEffectBase
{
    protected override void OnActivate()
    {
        var _txt = BattleText.Replace("NAME", characterName).Replace("#", turnsLeft.ToString());
    }
    
    protected override void OnDeactivate()
    {
        
    }
    
    protected override IEnumerator OnBeforeTurnTick(TurnBaseState ownerTurnState_)
    {

        var _txt = BattleText.Replace("NAME", characterName).Replace("#", turnsLeft.ToString());

        yield return BattleTextManager.DoWrite(_txt);
        
        RemoveTurn();
        
        yield return null;
        
        yield return ownerTurnState_.StateMachine.NextTurnState();
    }

    protected override void OnStackEffect(StatusEffectBase newEffect_)
    {
        SetDurationLeft(turnsLeft + turnDuration);
    }
}
