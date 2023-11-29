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

        [SerializeField] public GameObject[] switchBtns;

        public void DisplayAllySelected(PartyInfo allyInfo)
        {
            nameTxt.text = allyInfo.name;
            descriptionTxt.text = allyInfo.description;
            allyIcon.sprite = allyInfo.sprite;
            allyIcon.gameObject.SetActive(true);

            ShowButtons();
        }

        public void DisplayNull()
        {
            nameTxt.SetText("None");
            descriptionTxt.SetText("--");
            allyIcon.sprite = null;
            allyIcon.gameObject.SetActive(false);
        }

        public void ShowButtons()
        {
            for (int i = 0; i < switchBtns.Length; i++)
            {
                switchBtns[i].SetActive(true);
            }
        }
    }
}
