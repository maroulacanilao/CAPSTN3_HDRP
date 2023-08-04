using System.Collections;
using System.Collections.Generic;
using System.Text;
using BattleSystem.BattleState;
using CustomHelpers;
using Managers;
using ObjectPool;
using UI.Battle;
using UnityEngine;

public class SkipTurn_SE : StatusEffectBase
{
    [SerializeField] private string vfxName;
    
    protected override IEnumerator OnActivate()
    {
        var _txt = BattleText.Replace("NAME", characterName).Replace("#", turnsLeft.ToString());
        
        if(!GameManager.IsBattleSceneActive()) yield break;
        var _sb = new StringBuilder();
        _sb.Append("<color=red>");
        _sb.Append(Target.character.characterData.characterName);
        _sb.Append("</color>");
        _sb.Append(" was stunned!");

        yield return BattleTextManager.DoWrite(_sb.ToString());
    }
    
    protected override void OnDeactivate()
    {
        
    }
    
    protected override IEnumerator OnBeforeTurnTick(TurnBaseState ownerTurnState_)
    {
        if (turnsLeft <= 0)
        {
            SelfRemove();
            yield break;
        }
        var _txt = BattleText.Replace("NAME", characterName).Replace("#", turnsLeft.ToString());
        
        AssetHelper.PlayEffectCoroutine(vfxName, Target.character.transform, Vector3.zero, Quaternion.identity);

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
