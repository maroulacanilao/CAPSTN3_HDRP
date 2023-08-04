using System;
using System.Collections;
using System.Collections.Generic;
using BattleSystem.BattleState;
using Character;
using CustomHelpers;
using Managers;
using UI.Battle;
using UnityEngine;

public class BonusStatScaled_SE : StatusEffectBase
{
    [SerializeField] private bool IsBeforeTick;
    [field: SerializeField] public float vitalityMultiplier { get; set; }
    [field: SerializeField] public float strengthMultiplier { get; set; }
    [field: SerializeField] public float intelligenceMultiplier { get; set; }
    [field: SerializeField] public float defenseMultiplier { get; set; }
    [field: SerializeField] public float SpeedMultiplier { get; set; }

    [Header("VFX")] 
    [SerializeField] private string VfxName;


    private CombatStats bonusStatApplied;

    private bool hasApplied;
    private bool hasDeApplied;
    
    private void Start()
    {
        hasApplied = false;
        hasDeApplied = false;
    }

    protected override IEnumerator OnActivate()
    {
        if(hasApplied) yield break;
        var _baseStats = Target.character.statsData.GetTotalNonBonusStats(Target.character.level);
        
        bonusStatApplied = new CombatStats()
        {
            vitality = Mathf.RoundToInt(_baseStats.vitality * vitalityMultiplier),
            strength = Mathf.RoundToInt(_baseStats.strength * strengthMultiplier),
            intelligence = Mathf.RoundToInt(_baseStats.intelligence * intelligenceMultiplier),
            defense = Mathf.RoundToInt(_baseStats.defense * defenseMultiplier),
            speed = Mathf.RoundToInt(_baseStats.speed * SpeedMultiplier)
        };

        Target.character.statsData.AddBonusStats(bonusStatApplied);
        hasApplied = true;
        
        if(!GameManager.IsBattleSceneActive()) yield break;
        
        yield return AssetHelper.Co_PlayEffect(VfxName, Target.character.transform, Vector3.zero, Quaternion.identity, 1f);
        var _name = $"<color=red>{Target.character.characterData.characterName}</color>";
        var _msg = BattleText.Replace("NAME", _name).Beautify();
            
        yield return BattleTextManager.DoWrite(_msg.ToString());
        
    }
    
    protected override void OnDeactivate()
    {
        if(hasDeApplied) return;
        Target.character.statsData.RemoveBonusStats(bonusStatApplied);
        hasDeApplied = true;
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
}
