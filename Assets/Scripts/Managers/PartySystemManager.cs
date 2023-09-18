using System.Collections;
using System.Collections.Generic;
using BattleSystem;
using Character;
using ScriptableObjectData.CharacterData;
using UnityEngine;

public class PartySystemManager : MonoBehaviour
{
    private static PartySystemManager _instance;
    public static PartySystemManager Instance { get; private set; }
    public PlayerData player;
    
    List<AllyData> party = new List<AllyData>();
    
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

    public void Start()
    {
        //transfer the party list content to player.alliesData
        // player.alliesData = party;
    }
    
    public void AddCharacter(AllyData character)
    {
        party.Add(character);
    }
    
    public void RemoveCharacter(AllyData character)
    {
        party.Remove(character);
    }
    
}
