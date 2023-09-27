using System.Collections;
using System.Collections.Generic;
using BattleSystem;
using Character;
using NaughtyAttributes;
using ScriptableObjectData;
using ScriptableObjectData.CharacterData;
using UnityEngine;

public class PartySystemManager : MonoBehaviour
{
    private static PartySystemManager _instance;
    public static PartySystemManager Instance { get; private set; }

    public PlayerData playerData;
    public AllyDataBase allyDataBase;
    
    public string id_;
    
    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        
    }

    [Button("Make ally playable")]
    public void MakePlayable(string id)
    {
        // var id = "woodcutter";
        playerData.alliesData.Add(allyDataBase.allyDataDictionary[id]); 
    }
    
    

    
    
}
