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
    
    public List<AllyData> offPartyData; //Implement the off party data here if something is wrong with saving.
    
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
        if (playerData.offPartyData.Contains(allyDataBase.allyDataDictionary[id])) return;
        
        // This gets the ID of the NPC interacted with, crossmatches it with the ally data base, if valid, transfers it to the playable allies
        playerData.offPartyData.Add(allyDataBase.allyDataDictionary[id]);
    }

    public void AddOffPartyIntoAlliesData(int offPartyIndex)
    {
        if (playerData != null)
        {
            playerData.AddAlly(playerData.offPartyData[offPartyIndex]);
            playerData.offPartyData.Remove(playerData.offPartyData[offPartyIndex]);
        }
    }

    public void MoveAlliesDataIntoOffParty(int alliesDataIndex)
    {
        if (playerData != null)
        {
            playerData.offPartyData.Add(playerData.alliesData[alliesDataIndex]);
            playerData.alliesData.Remove(playerData.alliesData[alliesDataIndex]);
        }
    }
}
