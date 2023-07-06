using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using ScriptableObjectData;
using UnityEngine;
using Application = UnityEngine.Device.Application;

public class DebugScript : MonoBehaviour
{
    public GameDataBase gameDataBase;
    public int expToAdd;
    
    [Button("Add Exp")]
    private void AddExp()
    {
        gameDataBase.playerData.LevelData.AddExp(expToAdd);
    }

    [Button("DebugL")]
    private void DebugL()
    {
        Debug.Log(Application.persistentDataPath);
    }
}
