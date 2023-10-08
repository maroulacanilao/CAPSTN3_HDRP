using Character;
using Managers;
using ScriptableObjectData;
using ScriptableObjectData.CharacterData;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.TabMenu.CharacterInfo
{
    public class PartyUI : MonoBehaviour
    {
        // public AllyDataBase woodcutterAllyData;

        [SerializeField] private PartySystemManager partySystemManager;
        [SerializeField] private PlayerData playerData;
        [SerializeField] private List<AllyData> alliesData;
        [SerializeField] private List<AllyData> offPartyData;

        public TextMeshProUGUI playerPartyMemberText;
        public TextMeshProUGUI[] mainPartyMemberTexts;
        public TextMeshProUGUI[] offPartyMemberTexts;

        private void Awake()
        {
            partySystemManager = GameManager.Instance.Player.GetComponent<PartySystemManager>();
            playerData = partySystemManager.playerData;
            alliesData = playerData.alliesData;
            offPartyData = playerData.offPartyData;
        }

        public void OnEnable()
        {
            UpdateMainPartyTexts();
            UpdateOffPartyButtons();
        }

        public void UpdateMainPartyTexts()
        {
            playerPartyMemberText.text = playerData.characterName;

            for (int i = 0; i < mainPartyMemberTexts.Length; i++)
            {
                if (alliesData != null && alliesData.Count > 0)
                {
                    if (i < alliesData.Count)
                    {
                        mainPartyMemberTexts[i].text = alliesData[i].characterName;
                        mainPartyMemberTexts[i].GetComponent<Button>().interactable = true;
                    }
                    else
                    {
                        mainPartyMemberTexts[i].text = "--";
                        mainPartyMemberTexts[i].GetComponent<Button>().interactable = false;
                    }
                }
                else
                {
                    mainPartyMemberTexts[i].text = "--";
                    mainPartyMemberTexts[i].GetComponent<Button>().interactable = false;
                }
            }
        }

        public void UpdateOffPartyButtons()
        {
            for (int i = 0; i < offPartyMemberTexts.Length; i++)
            {
                if (offPartyData != null && offPartyData.Count > 0)
                {
                    if (i < offPartyData.Count)
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
                else
                {
                    offPartyMemberTexts[i].text = "--";
                    offPartyMemberTexts[i].GetComponent<Button>().interactable = false;
                }
            }
        }

        #region Buttons

        public void AddOffPartyMemberToMainParty(int offPartyIndex)
        {
            if (alliesData != null && alliesData.Count < mainPartyMemberTexts.Length)
            {
                offPartyMemberTexts[offPartyIndex].GetComponent<Button>().interactable = false;
                partySystemManager.AddOffPartyIntoAlliesData(offPartyIndex);

                UpdateMainPartyTexts();
                UpdateOffPartyButtons();
            }
        }

        public void RemoveMainPartyMember(int mainPartyIndex)
        {
            if (alliesData != null && offPartyData.Count < offPartyMemberTexts.Length)
            {
                mainPartyMemberTexts[mainPartyIndex].GetComponent<Button>().interactable = false;
                partySystemManager.MoveAlliesDataIntoOffParty(mainPartyIndex);
            }

            UpdateMainPartyTexts();
            UpdateOffPartyButtons();
        }

        #region Debug Functions

        public void AddWoodcutter()
        {
            partySystemManager.MakePlayable("woodcutter");

            if (alliesData.Count < 2)
            {
                // AddOffPartyMemberToMainParty(0); // automatically add current character to main party
            }

            offPartyMemberTexts[0].GetComponent<Button>().interactable = true;
            UpdateOffPartyButtons();
        }

        public void AddHerbalist()
        {
            partySystemManager.MakePlayable("herbalist");

            if (alliesData.Count < 2)
            {
                // AddOffPartyMemberToMainParty(1); // automatically add current character to main party
            }

            offPartyMemberTexts[1].GetComponent<Button>().interactable = true;
            UpdateOffPartyButtons();
        }

        public void ResetPartyData()
        {
            alliesData.Clear();
            offPartyData.Clear();

            UpdateMainPartyTexts();
            UpdateOffPartyButtons();
        }

        #endregion

        #endregion
    }
}