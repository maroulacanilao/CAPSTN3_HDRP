using Character;
using Managers;
using ScriptableObjectData;
using ScriptableObjectData.CharacterData;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI.HUD;
using UI.TabMenu.CharacterInfo.Party;
using UnityEngine;
using UnityEngine.EventSystems;
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

        public TextMeshProUGUI[] mainPartyMemberTexts;
        public TextMeshProUGUI[] offPartyMemberTexts;

        [field: SerializeField] public PartyMemberInfoPanel[] PartyMemberInfoPanels { get; private set; }

        public GameObject partymanagementPanel;

        private void Awake()
        {
            partySystemManager = GameManager.Instance.Player.GetComponent<PartySystemManager>();
            playerData = partySystemManager.playerData;
            alliesData = playerData.alliesData;
            offPartyData = playerData.offPartyData;

            for (int i = 0; i < PartyMemberInfoPanels.Length; i++)
            {
                PartyMemberInfoPanels[i].ShowAllyDetail(null);
            }
        }

        private void Start()
        {
            PartyManagementAvailability();
        }

        public void OnEnable()
        {
            PartyManagementAvailability();
        }

        public void UpdateMainParty()
        {
            if (alliesData != null)
            {
                for (int i = 0; i < mainPartyMemberTexts.Length; i++)
                {
                    if (alliesData.Count > 0 && i < alliesData.Count)
                    {
                        mainPartyMemberTexts[i].text = alliesData[i].characterName;
                        mainPartyMemberTexts[i].GetComponent<Button>().interactable = true;

                        PartyMemberInfoPanels[i].ShowAllyDetail(alliesData[i]);
                    }
                    else
                    {
                        mainPartyMemberTexts[i].text = "--";
                        mainPartyMemberTexts[i].GetComponent<Button>().interactable = false;

                        PartyMemberInfoPanels[i].ShowAllyDetail(null);
                    }
                }
            }
        }

        public void UpdateOffParty()
        {
            if (offPartyData != null)
            {
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
        }

        private void PartyManagementAvailability()
        {
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == GameManager.Instance.DungeonSceneName)
            {
                partymanagementPanel.SetActive(false);
            }
            else
            {
                partymanagementPanel.SetActive(true);
                UpdateMainParty();
                UpdateOffParty();
            }
        }

        #region Buttons

        public void AddOffPartyMemberToMainParty(int offPartyIndex)
        {
            if (alliesData != null && alliesData.Count < mainPartyMemberTexts.Length)
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
                mainPartyMemberTexts[mainPartyIndex].GetComponent<Button>().interactable = false;
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
        //    UpdateOffParty();
        //}

        //public void AddHerbalist()
        //{
        //    partySystemManager.MakePlayable("herbalist");

        //    if (alliesData.Count < 2)
        //    {
        //        // AddOffPartyMemberToMainParty(1); // automatically add current character to main party
        //    }

        //    offPartyMemberTexts[1].GetComponent<Button>().interactable = true;
        //    UpdateOffParty();
        //}

        //public void ResetPartyData()
        //{
        //    alliesData.Clear();
        //    offPartyData.Clear();

        //    UpdateMainParty();
        //    UpdateOffParty();
        //}

        #endregion

    }
}