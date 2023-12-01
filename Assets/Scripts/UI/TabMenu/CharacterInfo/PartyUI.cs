using Items.ItemData;
using Managers;
using ScriptableObjectData;
using ScriptableObjectData.CharacterData;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UI.HUD;
using UI.TabMenu.Codex;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.TabMenu.CharacterInfo.Party
{
    public class PartyUI : MonoBehaviour
    {
        // public AllyDataBase woodcutterAllyData;

        [SerializeField] private PartySystemManager partySystemManager;
        [SerializeField] private PlayerData playerData;
        [SerializeField] private List<AllyData> alliesData;
        [SerializeField] private List<AllyData> offPartyData;
        [SerializeField] private List<AllyData> totalNpcsList;
        [SerializeField] private List<AllyData> tempAlliesData = new();
        [SerializeField] private List<AllyData> tempOffPartyData = new();

        [SerializeField] private PartySelectInfoPanel[] partyPanels;
        [SerializeField] private List<int> partyIndexes = new();

        [SerializeField] private GameObject offPartyPanel;
        [SerializeField] private TextMeshProUGUI[] offPartyTxts;
        [SerializeField] private Button ConfirmBtn;

        [SerializeField] private TextMeshProUGUI offPartyErrortxt;
        [SerializeField] private TextMeshProUGUI dungeonErrortxt;

        private void Awake()
        {
            partySystemManager = GameManager.Instance.Player.GetComponent<PartySystemManager>();
            playerData = partySystemManager.playerData;
            alliesData = playerData.alliesData;
            offPartyData = playerData.offPartyData;
            totalNpcsList = playerData.totalPartyData;

            for (int i = 0; i < partyPanels.Length; i++)
            {
                partyPanels[i].DisplayNull();
                partyIndexes.Add(0);
            }
        }

        public void OnEnable()
        {
            offPartyErrortxt.gameObject.SetActive(playerData.totalPartyData.Count <= 0);
            offPartyPanel.SetActive(playerData.totalPartyData.Count > 0);
            // ConfirmBtn.gameObject.SetActive(playerData.totalPartyData.Count > 0);

            if (playerData.totalPartyData.Count == 0)
            {
                EmptyParty();
                return;
            }

            tempAlliesData = alliesData;
            tempOffPartyData = alliesData;

            if (totalNpcsList.Count > 0)
            {
                ShowOffParty();
            }


            // checks if the player is in a dungeon
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == GameManager.Instance.DungeonSceneName)
            {
                dungeonErrortxt.gameObject.SetActive(true);

                for (int i = 0; i < partyPanels.Length; i++)
                {
                    partyPanels[i].gameObject.SetActive(false);
                }
            }
            else
            {
                dungeonErrortxt.gameObject.SetActive(false);

                if (alliesData.Count > 0)
                {
                    for (int i = 0; i < partyPanels.Length; i++)
                    {
                        partyPanels[i].gameObject.SetActive(true);
                    }

                    ShowCurrentParty();
                }
                else
                {
                    for (int i = 0; i < partyPanels.Length; i++)
                    {
                        partyPanels[i].DisplayNull();
                        partyIndexes[i] = 0;
                    }
                }
            }
        }

        public void ShowOffParty()
        {
            if (offPartyData != null)
            {
                for (var _i = 0; _i < offPartyTxts.Length; _i++)
                {
                    if (_i < offPartyData.Count)
                    {
                        var ally = offPartyData[_i];
                        offPartyTxts[_i].text = $"- {ally.characterName}";
                        offPartyTxts[_i].gameObject.SetActive(true);
                        continue;
                    }

                    offPartyTxts[_i].gameObject.SetActive(false);
                }

                for (int i = 0; i < partyPanels.Length; i++)
                {
                    // partyPanels[i].ShowButtons();
                }
            }
        }

        public void ShowParty(int partyPanelIndex_, int partyIndex_)
        {
            var currentAlly = totalNpcsList[partyIndex_];

            var _info = new PartyInfo
            {
                name = currentAlly.characterName,
                description = currentAlly.encyclopediaInfo.description,
                sprite = currentAlly.encyclopediaInfo.sprite,
            };

            partyPanels[partyPanelIndex_].DisplayAllySelected(_info);
            ShowOffParty();
        }

        public void ShowCurrentParty()
        {
            for (int i = 0; i < totalNpcsList.Count; i++)
            {
                if (totalNpcsList[i] == alliesData[0])
                {
                    ShowParty(0, i);
                    break;
                }
            }

            for (int i = 0; i < totalNpcsList.Count; i++)
            {
                if (alliesData[1] != null && totalNpcsList[i] == alliesData[1])
                {
                    ShowParty(1, i);
                    break;
                }
            }
        }

        public void EmptyParty()
        {
            for(int i = 0; i < partyPanels.Length; i++)
            {
                partyPanels[i].gameObject.SetActive(false);
            }
        }

        #region Buttons
        public void NextPartyMember(int panelIndex)
        {
            if (!offPartyData.Contains(totalNpcsList[partyIndexes[panelIndex]]))
                offPartyData.Add(totalNpcsList[partyIndexes[panelIndex]]);

            alliesData.Remove(totalNpcsList[partyIndexes[panelIndex]]);

            partyIndexes[panelIndex] = (partyIndexes[panelIndex] + 1) % totalNpcsList.Count;

            var i = partyIndexes[panelIndex];
            if (!alliesData.Contains(totalNpcsList[i]))
            {
                alliesData.Add(totalNpcsList[i]);
                offPartyData.Remove(totalNpcsList[i]);
                ShowParty(panelIndex, i);
                ShowOffParty();
            }
            else
            {
                partyIndexes[panelIndex] = (partyIndexes[panelIndex] + 1) % totalNpcsList.Count;
                alliesData.Add(totalNpcsList[partyIndexes[panelIndex]]);
                offPartyData.Remove(totalNpcsList[partyIndexes[panelIndex]]);
                ShowParty(panelIndex, partyIndexes[panelIndex]);
                ShowOffParty();
            }

        }

        public void PreviousPartyMember(int panelIndex)
        {
            if (!offPartyData.Contains(totalNpcsList[partyIndexes[panelIndex]]))
                offPartyData.Add(totalNpcsList[partyIndexes[panelIndex]]);

            alliesData.Remove(totalNpcsList[partyIndexes[panelIndex]]);

            partyIndexes[panelIndex]--;
            if (partyIndexes[panelIndex] < 0)
            {
                partyIndexes[panelIndex] += totalNpcsList.Count;
            }

            var i = partyIndexes[panelIndex];
            if (!alliesData.Contains(totalNpcsList[i]))
            {
                alliesData.Add(totalNpcsList[i]);
                offPartyData.Remove(totalNpcsList[i]);
                ShowParty(panelIndex, i);
                ShowOffParty();
            }
            else
            {
                partyIndexes[panelIndex]--;
                if (partyIndexes[panelIndex] < 0)
                {
                    partyIndexes[panelIndex] += totalNpcsList.Count;
                }

                alliesData.Add(totalNpcsList[partyIndexes[panelIndex]]);
                offPartyData.Remove(totalNpcsList[partyIndexes[panelIndex]]);
                ShowParty(panelIndex, partyIndexes[panelIndex]);
                ShowOffParty();
            }
        }

        public void ConfirmParty()
        {
            if (tempAlliesData == null)
            {
                return;
            }

            for (int i = 0; i < tempOffPartyData.Count; i++)
            {
                if (!offPartyData.Contains(tempOffPartyData[i]))
                {
                    partySystemManager.MoveAlliesDataIntoOffParty(tempOffPartyData[i]);
                }
            }

            for (int i = 0; i < tempAlliesData.Count; i++)
            {
                if (!alliesData.Contains(tempAlliesData[i]))
                {
                    partySystemManager.AddToAlliesData(tempAlliesData[i]);
                }
            }

            ShowCurrentParty();
            ShowOffParty();
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