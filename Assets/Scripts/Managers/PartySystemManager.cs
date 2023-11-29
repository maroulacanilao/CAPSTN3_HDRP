using System.Collections;
using System.Collections.Generic;
using BattleSystem;
using Character;
using NaughtyAttributes;
using ScriptableObjectData;
using ScriptableObjectData.CharacterData;
using UI.TabMenu.CharacterInfo;
using UnityEngine;
using AYellowpaper.SerializedCollections;
using Items.ItemData;

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
        if (playerData.totalPartyData.Contains(allyDataBase.allyDataDictionary[id])) return;

        playerData.totalPartyData.Add(allyDataBase.allyDataDictionary[id]);

        // This gets the ID of the NPC interacted with, crossmatches it with the ally data base, if valid, transfers it to the playable allies
        playerData.offPartyData.Add(allyDataBase.allyDataDictionary[id]);
    }

    public void AddToAlliesData(AllyData ally)
    {
        if (playerData != null)
        {
            playerData.AddAlly(ally);
            playerData.offPartyData.Remove(ally);
        }
    }

    public void MoveAlliesDataIntoOffParty(AllyData ally)
    {
        if (playerData != null)
        {
            playerData.offPartyData.Add(ally);
            playerData.alliesData.Remove(ally);
        }
    }
}
