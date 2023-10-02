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
    
    //Call this when interacting with the NPC for the first time
    public void MakePlayable(string id)
    {
        // This gets the ID of the NPC interacted with, crossmatches it with the ally data base, if valid, transfers it to the playable allies
        allyDataBase.TransferKeyValuePair(id);
    }
    
    

    
    
}
