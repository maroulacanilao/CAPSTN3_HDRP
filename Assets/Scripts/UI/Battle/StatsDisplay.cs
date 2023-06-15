using System;
using BattleSystem;
using TMPro;
using UI.HUD;
using UnityEngine;

namespace UI.Battle
{
    public class StatsDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI name_TXT, atk_TXT, arm_TXT, spd_TXT, mag_TXT, res_TXT, acc_TXT;
        [SerializeField] private GameObject panel;
        private BattleCharacter currentCharacter;

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
            currentCharacter = character_;
            if (currentCharacter == null)
            {
                panel.SetActive(false);
                return;
            }
            
            name_TXT.text = currentCharacter.character.characterData.characterName;
            atk_TXT.text = $"Physical Damage: {currentCharacter.character.stats.physicalDamage}";
            arm_TXT.text = $"Physical Resistance: {currentCharacter.character.stats.armor}";
            spd_TXT.text = $"Speed: {currentCharacter.character.stats.speed}";
            mag_TXT.text = $"Magic Damage: {currentCharacter.character.stats.magicDamage}";
            res_TXT.text = $"Magic Resistance: {currentCharacter.character.stats.magicResistance}";
            acc_TXT.text = $"Accuracy: {currentCharacter.character.stats.accuracy}";
            panel.SetActive(true);
        }
    }
}
