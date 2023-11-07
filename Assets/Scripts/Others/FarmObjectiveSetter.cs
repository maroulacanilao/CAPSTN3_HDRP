using System;
using System.Collections;
using System.Collections.Generic;
using CustomHelpers;
using Managers;
using ScriptableObjectData;
using UI.HUD;
using UnityEngine;

[DefaultExecutionOrder(5)]
public class FarmObjectiveSetter : MonoBehaviour
{
    [SerializeField] ProgressionData progressionData;
    [SerializeField] private string defeatedSidapaMsg = "DefeatedSidapa";
    private string daysCounterTxt;
    
    private void Awake()
    {
        TimeManager.OnEndDay.AddListener(OnEndDay);
        UpdateText();
    }

    private void OnDestroy()
    {
        TimeManager.OnEndDay.RemoveListener(OnEndDay);
    }
    
    private void OnEnable()
    {
        SetDefaultObjective();
    }
    
    private void OnEndDay()
    {
        UpdateText();
        SetDefaultObjective();
    }

    public void SetDefaultObjective()
    {
        if(!progressionData.hasDefeatedSidapa)ObjectiveHUD.OnUpdateObjectiveText.Invoke(daysCounterTxt);
        else ObjectiveHUD.OnSendMessage.Invoke(defeatedSidapaMsg);
    }

    private void UpdateText()
    {
        var _sb = new System.Text.StringBuilder();
        var _daysRemaining = TimeManager.DaysLeft;
        _sb.Append("Defeat Sidapa in the cave before time runs out!\n");
        // _sb.Append($"(Days Remaining: {_daysRemaining})");
        daysCounterTxt = _sb.ToString().Beautify();
    }
}
