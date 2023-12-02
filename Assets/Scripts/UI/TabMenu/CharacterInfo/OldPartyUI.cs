using Character;
using Managers;
using ScriptableObjectData;
using ScriptableObjectData.CharacterData;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI.TabMenu.CharacterInfo.Party;
using UnityEngine;
using UnityEngine.UI;

public class OldPartyUI : MonoBehaviour
{
    // public AllyDataBase woodcutterAllyData;

    [SerializeField] private PartySystemManager partySystemManager;
    [SerializeField] private PlayerData playerData;
    [SerializeField] private List<AllyData> alliesData;
    [SerializeField] private List<AllyData> offPartyData;

    // public TextMeshProUGUI playerPartyMemberText;
    // public TextMeshProUGUI[] mainPartyMemberTexts;
    public GameObject offPartyMembersTxtGroup;
    public TextMeshProUGUI[] offPartyMemberTexts;

    public PartySelectInfoPanel[] PartySelectInfoPanels;

    public GameObject offPartyListPanel;

    [SerializeField] private TextMeshProUGUI offPartyErrortxt;
    [SerializeField] private TextMeshProUGUI dungeonErrortxt;

    private void Awake()
    {
        partySystemManager = GameManager.Instance.Player.GetComponent<PartySystemManager>();
        playerData = partySystemManager.playerData;
        alliesData = partySystemManager.playerData.alliesData;
        offPartyData = partySystemManager.playerData.offPartyData;
    }

    public void OnEnable()
    {
        partySystemManager = GameManager.Instance.Player.GetComponent<PartySystemManager>();
        playerData = partySystemManager.playerData;
        alliesData = partySystemManager.playerData.alliesData;
        offPartyData = partySystemManager.playerData.offPartyData;

        for (int i = 0; i < PartySelectInfoPanels.Length; i++)
        {
            PartySelectInfoPanels[i].DisplayNull();
        }

        dungeonErrortxt.gameObject.SetActive(false);
        offPartyMembersTxtGroup.SetActive(playerData.totalPartyData.Count > 0);
        offPartyErrortxt.gameObject.SetActive(playerData.totalPartyData.Count <= 0);

        PartySelectMenuAvailability();
    }


    public void UpdateMainParty()
    {
        if (alliesData == null)
        {
            return;
        }

        if (alliesData.Count > 0)
        {
            for (int i = 0; i < PartySelectInfoPanels.Length; i++)
            {
                if (i < alliesData.Count)
                {
                    PartySelectInfoPanels[i].DisplayAllyDetail(alliesData[i]);
                }
                else
                {
                    PartySelectInfoPanels[i].DisplayNull();
                }
            }
        }
        else
        {
            for (int i = 0; i < PartySelectInfoPanels.Length; i++)
            {
                PartySelectInfoPanels[i].DisplayNull();
            }
        }
    }

    public void UpdateOffParty()
    {
        if (offPartyData == null)
        {
            return;
        }

        for (int i = 0; i < offPartyMemberTexts.Length; i++)
        {
            if (offPartyData.Count > 0 && i < offPartyData.Count)
            {
                offPartyMemberTexts[i].text = offPartyData[i].characterName;
                offPartyMemberTexts[i].GetComponent<Button>().interactable = true;
            }
            else
            {
                offPartyMemberTexts[i].text = "--";
                offPartyMemberTexts[i].GetComponent<Button>().interactable = false;
            }
        }
    }

    private void PartySelectMenuAvailability()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == GameManager.Instance.DungeonSceneName)
        {
            for (int i = 0; i < PartySelectInfoPanels.Length; i++)
            {
                PartySelectInfoPanels[i].gameObject.SetActive(false);
            }

            offPartyMembersTxtGroup.SetActive(false);
            dungeonErrortxt.gameObject.SetActive(true);
        }
        else if (playerData.totalPartyData.Count < PartySelectInfoPanels.Length)
        {
            for (int i = 0; i < PartySelectInfoPanels.Length; i++)
            {
                if (i < playerData.totalPartyData.Count)
                {
                    PartySelectInfoPanels[i].gameObject.SetActive(true);
                }
                else
                {
                    PartySelectInfoPanels[i].gameObject.SetActive(false);
                }
            }

            UpdateMainParty();
            UpdateOffParty();
        }
        else
        {
            for (int i = 0; i < PartySelectInfoPanels.Length; i++)
            {
                PartySelectInfoPanels[i].gameObject.SetActive(true);
            }

            UpdateMainParty();
            UpdateOffParty();
        }
    }

    #region Buttons
    public void AddOffPartyMemberToMainParty(int offPartyIndex)
    {
        if (alliesData != null && alliesData.Count < PartySelectInfoPanels.Length)
        {
            offPartyMemberTexts[offPartyIndex].GetComponent<Button>().interactable = false;
            partySystemManager.AddOffPartyIntoAlliesData(offPartyIndex);

            UpdateMainParty();
            UpdateOffParty();
        }
    }

    public void RemoveMainPartyMember(int mainPartyIndex)
    {
        if (alliesData != null && offPartyData.Count < offPartyMemberTexts.Length)
        {
            PartySelectInfoPanels[mainPartyIndex].selectedPartyBtn.interactable = false;
            partySystemManager.MoveAlliesDataIntoOffParty(mainPartyIndex);
        }

        UpdateMainParty();
        UpdateOffParty();
    }
    #endregion

    #region Debug Functions

    //public void AddWoodcutter()
    //{
    //    partySystemManager.MakePlayable("woodcutter");

    //    if (alliesData.Count < 2)
    //    {
    //        // AddOffPartyMemberToMainParty(0); // automatically add current character to main party
    //    }

    //    offPartyMemberTexts[0].GetComponent<Button>().interactable = true;
    //    UpdateOffPartyButtons();
    //}

    //public void AddHerbalist()
    //{
    //    partySystemManager.MakePlayable("herbalist");

    //    if (alliesData.Count < 2)
    //    {
    //        // AddOffPartyMemberToMainParty(1); // automatically add current character to main party
    //    }

    //    offPartyMemberTexts[1].GetComponent<Button>().interactable = true;
    //    UpdateOffPartyButtons();
    //}

    //public void ResetPartyData()
    //{
    //    alliesData.Clear();
    //    offPartyData.Clear();

    //    UpdateMainPartyTexts();
    //    UpdateOffPartyButtons();
    //}

    #endregion    
}
