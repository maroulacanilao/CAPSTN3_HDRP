using System.Collections;
using System.Collections.Generic;
using BaseCore;
using GameTutorial;
using Items;
using ScriptableObjectData.CharacterData;
using UI.Battle;
using UnityEngine;

public class BattleTutorial : Singleton<BattleTutorial>
{
    [SerializeField] PlayerData playerData;

    static PlayerData PlayerData => Instance.playerData;
    
    public bool skipTutorial;

    public static IEnumerator Welcome()
    {
        if(Instance.skipTutorial) yield break;
        if(TutorialValues.HasWelcomedCombat) yield break;
        var _msg1 = "Welcome to the combat!";
        var _msg2 = "Use your mouse and click the left mouse button on the action you want to perform.";
        var _msg3 = "You can also use the keyboard to select the action.";
        var _msg4 = "If you have the advantage, you will perform the action first.";
        var _msg5 = "If you have the disadvantage, the enemy will perform the action first.";
        
        var _wait = new WaitForSeconds(0.1f);
        
        yield return BattleTextManager.DoWrite(_msg1);
        yield return _wait;
        yield return BattleTextManager.DoWrite(_msg2);
        yield return _wait;
        yield return BattleTextManager.DoWrite(_msg3);
        yield return _wait;
        yield return BattleTextManager.DoWrite(_msg4);
        yield return _wait;
        yield return BattleTextManager.DoWrite(_msg5);
        TutorialValues.HasWelcomedCombat = true;
        yield return _wait;
    }

    public static IEnumerator TurnTutorial()
    {
        if(Instance.skipTutorial) yield break;
        if(TutorialValues.HasTaughtRun 
           && TutorialValues.HasTaughtBasicAttack 
           && TutorialValues.HasTaughtSpell 
           && TutorialValues.HasTaughtItem ) yield break;

        yield return BasicAttack();
    }
    
    public static IEnumerator BasicAttack()
    {
        var _wait = new WaitForSeconds(0.1f);
        if (TutorialValues.HasTaughtBasicAttack)
        {
            yield return Co_Spell();
            yield return _wait;
            yield return Co_Heal();
            yield break;
        }

        var _msg1 = "Hit the enemy with your basic attack.";
        var _msg2 = "It costs no mana, no cooldown, but it has no special effects.";
        var _msg3 = "The damage is based on your your strength.";
        yield return BattleTextManager.DoWrite(_msg1);
        yield return _wait;
        yield return BattleTextManager.DoWrite(_msg2);
        yield return _wait;
        yield return BattleTextManager.DoWrite(_msg3);
        TutorialValues.HasTaughtBasicAttack = true;
        yield return _wait;
    }
    
    public static IEnumerator Co_Spell()
    {
        if(TutorialValues.HasTaughtSpell) yield break;
        if(PlayerData.spells.Count == 0) yield break;
        
        var _msg1 = "Use one of your spells on the enemy.";
        var _msg2 = "There are different types of spells in the game and can be used offensively and defensively.";
        var _msg3 = "Some spells have special effects.";
        var _msg4 = "If an enemy is weak to a certain element, it will take more damage from that element and will be stunned for 1 turn.";
        
        var _wait = new WaitForSeconds(0.1f);
        
        yield return BattleTextManager.DoWrite(_msg1);
        yield return _wait;
        yield return BattleTextManager.DoWrite(_msg2);
        yield return _wait;
        yield return BattleTextManager.DoWrite(_msg3);
        yield return _wait;
        yield return BattleTextManager.DoWrite(_msg4);
        yield return _wait;
        TutorialValues.HasTaughtSpell = true;
    }

    public static IEnumerator Co_Heal()
    {
        if(!IsPlayerLowHealth()) yield break;
        if (!HasConsumable())
        {
            yield return Co_Run();
            yield break;
        }
        if(TutorialValues.HasTaughtItem) yield break;
        
        var _msg1 = "You've taken a significant amount of damage.";
        var _msg2 = "Use a healing item to restore your health.";

        yield return BattleTextManager.DoWrite(_msg1);
        yield return new WaitForSeconds(0.1f);
        yield return BattleTextManager.DoWrite(_msg2);
        yield return new WaitForSeconds(0.1f);

        TutorialValues.HasTaughtItem = true;
    }
    
    public static IEnumerator Co_Run()
    {
        if(TutorialValues.HasTaughtRun) yield break;
        var _msg1 = "You've taken a significant amount of damage.";
        var _msg2 = "You can run away from the battle.";
        var _msg3 = "You wont lose any items or money, but you will not gain any experience or items.";
        
        var _wait = new WaitForSeconds(0.1f);
        
        yield return BattleTextManager.DoWrite(_msg1);
        yield return _wait;
        yield return BattleTextManager.DoWrite(_msg2);
        yield return _wait;
        yield return BattleTextManager.DoWrite(_msg3);
        TutorialValues.HasTaughtRun = true;
        yield return _wait;
    }
    
    public static bool HasConsumable()
    {
        return PlayerData.inventory.GetItemsInStorageByType(ItemType.Consumable).Count > 0;
    }
    
    public static bool IsPlayerLowHealth()
    {
        return PlayerData.health.CurrentHp < PlayerData.health.MaxHp / 2f;
    }
}
