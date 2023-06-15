using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using ScriptableObjectData;
using UnityEngine;

public class DebugScript : MonoBehaviour
{
    public GameDataBase gameDataBase;
    public int expToAdd;
    
    [Button("Add Exp")]
    private void AddExp()
    {
        gameDataBase.playerData.LevelData.AddExp(expToAdd);
    }
}
