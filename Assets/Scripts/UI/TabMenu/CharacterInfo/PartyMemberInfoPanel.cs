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

namespace UI.TabMenu.CharacterInfo.Party
{
    public class PartyMemberInfoPanel : MonoBehaviour
    {
        [BoxGroup("Panels")]
        [SerializeField] protected GameObject namePanel;
        [BoxGroup("Panels")]
        [SerializeField] protected GameObject skillInfoPanel;

        [BoxGroup("Stats Text")]
        [SerializeField] protected StatsInfo statsPanel;

        [BoxGroup("HP/Mana Bar")]
        [SerializeField] protected HealthBarUI healthBarUI;

        [BoxGroup("Icon")]
        [SerializeField] protected Image allyIcon;

        [BoxGroup("Info Text")]
        [SerializeField] protected TextMeshProUGUI nameTxt;
        [BoxGroup("Info Text")]
        [SerializeField] protected TextMeshProUGUI descriptionTxt;

        protected AllyData currAlly;

        private void Awake()
        {
            DisplayNull();
        }

        public virtual void ShowAllyDetail(AllyData ally)
        {
            if (ally == null)
            {
                DisplayNull();
                return;
            }

            currAlly = ally;
            allyIcon.color = Color.white;

            namePanel.SetActive(true);
            statsPanel.gameObject.SetActive(true);
            skillInfoPanel.SetActive(true);

            nameTxt.SetText(currAlly.characterName);
            allyIcon.sprite = currAlly.icon;

            statsPanel.DisplayDynamic(currAlly.GetStats(), false);
        }

        public virtual void DisplayNull()
        {
            nameTxt.SetText("?????");
            descriptionTxt.SetText("???");
            allyIcon.sprite = null;
            allyIcon.color = Color.clear;
            statsPanel.DisplayDynamic(new CombatStats(), false);
        }

    }
}