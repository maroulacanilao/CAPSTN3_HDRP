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

        [BoxGroup("HP/Mana Panel")]
        [SerializeField] protected Image hpIcon;
        [BoxGroup("HP/Mana Panel")]
        [SerializeField] protected TextMeshProUGUI hpTxt;
        [BoxGroup("HP/Mana Panel")]
        [SerializeField] protected Image manaIcon;
        [BoxGroup("HP/Mana Panel")]
        [SerializeField] protected TextMeshProUGUI manaTxt;

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
            descriptionTxt.text = $"This character has no available skills at the moment.";
            allyIcon.sprite = currAlly.icon;

            statsPanel.DisplayDynamic(currAlly.GetStats(), false);


            hpTxt.text = $"Health: {ally.health.CurrentHp}/{ally.health.MaxHp}";
            manaTxt.text = $"Mana: {ally.mana.CurrentMana} / {ally.mana.MaxMana}";
            hpIcon.gameObject.SetActive(true);
            manaIcon.gameObject.SetActive(true);
        }

        public virtual void DisplayNull()
        {
            nameTxt.SetText("?????");
            descriptionTxt.SetText("??????");
            allyIcon.sprite = null;
            allyIcon.color = Color.clear;
            statsPanel.DisplayDynamic(new CombatStats(), false);
            hpTxt.text = $"????";
            manaTxt.text = $"????";
            hpIcon.gameObject.SetActive(false);
            manaIcon.gameObject.SetActive(false);
        }

    }
}