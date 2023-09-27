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

    private PartySystemManager partySystemManager;

    public TextMeshProUGUI playerPartyMemberText;
    public TextMeshProUGUI[] allyPartyMemberTexts;

    // Start is called before the first frame update
    void Start()
    {
        partySystemManager = GameManager.Instance.Player.GetComponent<PartySystemManager>();
        UpdatePartyList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdatePartyList()
    {
        playerPartyMemberText.text = partySystemManager.playerData.characterName;
        playerPartyMemberText.gameObject.SetActive(true);

        if (partySystemManager.playerData.alliesData.Count > 0)
        {
            for (int i = 0; i < partySystemManager.playerData.alliesData.Count; i++)
            {
                if (partySystemManager.playerData.alliesData.Count <= allyPartyMemberTexts.Length)
                {
                    allyPartyMemberTexts[i].text = partySystemManager.playerData.alliesData[i].characterName;
                    allyPartyMemberTexts[i].gameObject.SetActive(true);
                }
                else
                {
                    break;
                }
            }
        }
    }

    public void AddWoodcutter()
    {
        partySystemManager.MakePlayable("woodcutter");
        UpdatePartyList();
    }

    public void AddHerbalist()
    {
        partySystemManager.MakePlayable("herbalist");
        UpdatePartyList();
    }
}
