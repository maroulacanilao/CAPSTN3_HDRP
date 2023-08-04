using System;
using BattleSystem;
using TMPro;
using UI.HUD;
using UnityEngine;

namespace UI.Battle
{
    public class SelectStatsDisplay : MonoBehaviour
    {
        [SerializeField] private StatsInfo statsInfo;
        [SerializeField] private TextMeshProUGUI nameTxt;
        
        private BattleCharacter currentCharacter;
        private GameObject panel => statsInfo.gameObject;

        private void Awake()
        {
            BattleCharacter.OnSelectMenu.AddListener(OnSelectMenuHandler);
            panel.SetActive(false);
        }

        private void OnDestroy()
        {
            BattleCharacter.OnSelectMenu.RemoveListener(OnSelectMenuHandler);
        }

        private void OnSelectMenuHandler(BattleCharacter character_)
        {
            currentCharacter = character_;
            
            if(character_ == null)
            {
                panel.SetActive(false);
                return;
            }
            
            nameTxt.text = character_.characterData.characterName;
            Debug.Log(character_.characterData.characterName);
            Debug.Log(character_.Level);
            Debug.Log(character_.character.level);
            statsInfo.DisplayCharacterStats(character_.characterData.statsData, character_.character.level);
            panel.SetActive(true);
        }
    }
}
