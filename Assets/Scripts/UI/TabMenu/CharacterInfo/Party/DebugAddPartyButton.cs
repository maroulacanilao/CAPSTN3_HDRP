using Character;
using Managers;
using ScriptableObjectData;
using ScriptableObjectData.CharacterData;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugAddPartyButton : MonoBehaviour
{
    // public AllyDataBase woodcutterAllyData;

    [SerializeField] private PartySystemManager partySystemManager;
    [SerializeField] private PlayerData playerData;
    [SerializeField] private AllyDataBase allyDataBase;
    [SerializeField] private List<AllyData> alliesData;
    [SerializeField] private List<AllyData> offPartyData;

    [SerializeField] private List<CharacterData> currentPartyMembers;

    public TextMeshProUGUI playerPartyMemberText;
    public TextMeshProUGUI[] partyMemberTexts;

    public TextMeshProUGUI[] offPartyMemberTexts;

    // Start is called before the first frame update
    void Start()
    {        
        partySystemManager = GameManager.Instance.Player.GetComponent<PartySystemManager>();
        playerData = partySystemManager.playerData;
        allyDataBase = partySystemManager.allyDataBase;
        alliesData = partySystemManager.playerData.alliesData;
        offPartyData = partySystemManager.playerData.offPartyData;

        currentPartyMembers = new List<CharacterData>();

        UpdatePartyListTexts();
    }

    public void PartyCheck()
    {

    }

    public void AddOffPartyMemberToParty()
    {

    }

    public void FillEmptyPartySlots(int offPartyIndex)
    {
        if (currentPartyMembers.Count <= 0)
        {
            currentPartyMembers.Add(playerData);
        }
        else
        {
            if (currentPartyMembers.Count < 2)
            {
                alliesData.Add(offPartyData[offPartyIndex]);
                currentPartyMembers.Add(offPartyData[offPartyIndex]);
                UpdatePartyListTexts();
            }
        }
    }


    public void UpdatePartyListTexts()
    {
        if (currentPartyMembers.Count > 0)
        {
            for (int i = 0; i < currentPartyMembers.Count; i++)
            {
                partyMemberTexts[i].text = currentPartyMembers[i].characterName;
                partyMemberTexts[i].gameObject.SetActive(true);
            }
        }
    }

    public void AddWoodcutter()
    {
        partySystemManager.MakePlayable("woodcutter");

        if (alliesData.Count < 2)
        {
            FillEmptyPartySlots(0);
        }

        UpdatePartyListTexts();
    }

    public void AddHerbalist()
    {
        partySystemManager.MakePlayable("herbalist");
        UpdatePartyListTexts();
    }
}
