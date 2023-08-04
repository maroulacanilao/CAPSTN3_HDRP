using System.Collections;
using System.Collections.Generic;
using BaseCore;
using Dungeon;
using NaughtyAttributes;
using ScriptableObjectData;
using UnityEngine;
using Application = UnityEngine.Device.Application;

public class DebugScript : MonoBehaviour
{
    public GameDataBase gameDataBase;
    public int expToAdd;
    public int damage = 10;
    public int manaDamage = 10;
    public int dayCounter = 25;
    
    [Button("Add Exp")]
    private void AddExp()
    {
        gameDataBase.playerData.LevelData.AddExp(expToAdd);
    }
    
    [Button("Damage Player")]
    private void DamagePlayer()
    {
        var _damage = new DamageInfo(damage,null);
        gameDataBase.playerData.health.TakeDamage(_damage);
    }
    
    [Button("Damage Player Mana")]
    private void DamagePlayerMana()
    {
        gameDataBase.playerData.mana.UseMana(manaDamage);
    }

    [Button("Set DayCounter")]
    private void SetDayCounter()
    {
        gameDataBase.progressionData.dayCounter = dayCounter;
    }
}
