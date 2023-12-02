using System;
using CustomHelpers;
using BaseCore;
using Character;
using UI.TabMenu.CharacterInfo;
using NaughtyAttributes;
using ScriptableObjectData.CharacterData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UI.HUD;
using BattleSystem;
using Character.CharacterComponents;
using Unity.VisualScripting;
using System.Collections.Generic;
using System.Dynamic;

namespace UI.TabMenu.CharacterInfo.Party
{
    public struct PartyInfo
    {
        public string name;
        public string description;
        public Sprite sprite;
        public int index;
    }

    public class PartySelectInfoPanel : MonoBehaviour
    {
        [SerializeField] protected Image allyIcon;
        [SerializeField] protected TextMeshProUGUI nameTxt;
        [SerializeField] protected TextMeshProUGUI descriptionTxt;

        // public GameObject[] switchBtns;
        public Button selectedPartyBtn;

        // DONT USE THIS
        public void DisplayAllySelected(PartyInfo allyInfo)
        {
            nameTxt.text = allyInfo.name;
            descriptionTxt.text = allyInfo.description;
            allyIcon.sprite = allyInfo.sprite;
            allyIcon.gameObject.SetActive(true);

            // ShowButtons();
        }

        public void DisplayAllyDetail(AllyData ally)
        {
            nameTxt.text = ally.characterName;
            descriptionTxt.text = ally.encyclopediaInfo.description;
            allyIcon.sprite = ally.encyclopediaInfo.sprite;
            allyIcon.gameObject.SetActive(true);

            selectedPartyBtn.interactable = true;
        }

        public void DisplayNull()
        {
            nameTxt.SetText("None");
            descriptionTxt.SetText("--");
            allyIcon.sprite = null;
            allyIcon.gameObject.SetActive(false);

            selectedPartyBtn.interactable = false;
        }

        // DONT USE THIS
        //public void ShowButtons()
        //{
        //    for (int i = 0; i < switchBtns.Length; i++)
        //    {
        //        switchBtns[i].SetActive(true);
        //    }
        //}
    }
}
