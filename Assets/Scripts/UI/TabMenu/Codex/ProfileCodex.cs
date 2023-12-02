using System.Collections.Generic;
using System.Linq;
using BaseCore;
using Character;
using Items.Inventory;
using Items.ItemData;
using Managers;
using NaughtyAttributes;
using ScriptableObjectData;
using ScriptableObjectData.CharacterData;
using TMPro;
using UI.Farming;
using UI.TabMenu.Codex;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.TabMenu.CharacterInfo
{
    public class ProfileCodex : CodexMenu
    {
        [SerializeField] private PlayerData playerData;

        [BoxGroup("Panels")]
        [SerializeField] private GameObject statsPanel, skillPanel, animationPanel;

        [SerializeField] private Image[] icons;

        [BoxGroup("Level")]
        [SerializeField] private TextMeshProUGUI level_TXT, currentExp_TXT, description_Txt;
        [BoxGroup("Level")]
        [SerializeField] private Image expBar;
        [BoxGroup("Level")]
        [SerializeField] private StatsInfo totalStatsInfo;

        [SerializeField] private SkillInfoPanel skillInfoPanel;

        [SerializeField] private Animator UiAnimator;

        private PlayerInventory inventory => playerData.inventory;
        private PlayerLevel level => playerData.LevelData;

        public int currentCodexindex;
        public int selectedSkill = 0;

        public Button skillsBtn;

        protected override void OnEnable()
        {
            base.OnEnable();
            RemoveItems();

            var _codexItem0 = Instantiate(codexItemPrefab, contentParent).Initialize(0, playerData.characterName);
            codexItems.Add(_codexItem0);

            for (var _i = 0; _i < playerData.totalPartyData.Count; _i++)
            {
                var _npc = playerData.totalPartyData[_i];
                var _codexItem = Instantiate(codexItemPrefab, contentParent).Initialize(_i + 1, _npc.characterName);
                codexItems.Add(_codexItem);
            }

            EventSystem.current.SetSelectedGameObject(codexItems[0].gameObject);

            if (playerData.spells.Count <= 0)
            {
                skillsBtn.gameObject.SetActive(false);
            }
            else
            {
                skillsBtn.gameObject.SetActive(true);
            }

            statsPanel.SetActive(true);
            skillPanel.SetActive(false);

            ShowCodex(0);
        }

        public override void ShowCodex(int index_)
        {
            statsPanel.SetActive(true);
            skillPanel.SetActive(false);

            level_TXT.text = $"{level.CurrentLevel}";
            currentExp_TXT.text = $"{level.CurrentLevelExperience} / {level.CurrentExperienceNeeded}";

            expBar.fillAmount = (float)level.CurrentLevelExperience / level.CurrentExperienceNeeded;

            if (index_ == 0)
            {
                UiAnimator.SetTrigger("Player");
                icons[0].sprite = playerData.encyclopediaInfo.sprite;
                icons[1].sprite = playerData.encyclopediaInfo.sprite;
                description_Txt.text = playerData.encyclopediaInfo.description;
                totalStatsInfo.DisplayCharacterStats(playerData.statsData, playerData.level);
            }
            else
            {
                var i = index_ - 1;

                switch (playerData.totalPartyData[i].characterName)
                {
                    case "Woodcutter":
                        UiAnimator.SetTrigger("Woodcutter");
                        break;
                    case "Stonemason":
                        UiAnimator.SetTrigger("Stonemason");
                        break;
                    case "Herbalist":
                        UiAnimator.SetTrigger("Herbalist");
                        break;
                    case "Scout":
                        UiAnimator.SetTrigger("Scout");
                        break;
                    case "Witch":
                        UiAnimator.SetTrigger("Witch");
                        break;
                }

                icons[0].sprite = playerData.totalPartyData[i].encyclopediaInfo.sprite;
                icons[1].sprite = playerData.totalPartyData[i].encyclopediaInfo.sprite;
                description_Txt.text = playerData.totalPartyData[i].encyclopediaInfo.description;
                totalStatsInfo.DisplayCharacterStats(playerData.totalPartyData[i].statsData, playerData.level);
            }

            currentCodexindex = index_;
        }

        public void OpenSkills()
        {
            statsPanel.SetActive(false);
            skillPanel.SetActive(true);

            skillInfoPanel.CreateList(currentCodexindex);
        }

        public void OpenStats()
        {
            skillPanel.SetActive(false);
            statsPanel.SetActive(true);
            ShowCodex(currentCodexindex);
        }

        public void NextSkill()
        {
            if (skillInfoPanel.SpellsList.Count > 0)
            {
                selectedSkill = (selectedSkill + 1) % skillInfoPanel.SpellsList.Count;
                skillInfoPanel.ShowSpellInfo(currentCodexindex, selectedSkill);
            }
        }

        public void PrevSkill()
        {
            if (skillInfoPanel.SpellsList.Count > 0)
            {
                selectedSkill--;
                if (selectedSkill < 0)
                {
                    selectedSkill += skillInfoPanel.SpellsList.Count;
                }
                skillInfoPanel.ShowSpellInfo(currentCodexindex, selectedSkill);
            }
        }
    }
}
