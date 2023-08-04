using BaseCore;
using Character;
using Items.Inventory;
using NaughtyAttributes;
using ScriptableObjectData.CharacterData;
using TMPro;
using UI.Farming;
using UnityEngine;
using UnityEngine.UI;

namespace UI.TabMenu.CharacterInfo
{
    public class CharacterInfoMenu : MonoBehaviour
    {
        [SerializeField] private PlayerData playerData;

        [BoxGroup("Level")]
        [SerializeField] private TextMeshProUGUI level_TXT, currentExp_TXT, nextLevelExp_TXT;
        [BoxGroup("Level")]
        [SerializeField] private Image expBar;
        [BoxGroup("Level")]
        [SerializeField] private StatsInfo totalStatsInfo;
        
        [BoxGroup("Weapon")]
        [SerializeField] private TextMeshProUGUI weaponName_TXT;
        [BoxGroup("Weapon")]
        [SerializeField] private StatsInfo weaponStatsInfo;
        [BoxGroup("Weapon")]
        [SerializeField] private Image weaponIcon;
        
        [BoxGroup("Armor")]
        [SerializeField] private TextMeshProUGUI armor_TXT;
        [BoxGroup("Armor")]
        [SerializeField] private StatsInfo armorStatsInfo;
        [BoxGroup("Armor")]
        [SerializeField] private Image armorIcon;

        private PlayerInventory inventory => playerData.inventory;
        private PlayerLevel level => playerData.LevelData;
        
        private PlayerMenuManager playerMenuManager;

        public void OnEnable()
        {
            level_TXT.text = $"Level: {level.CurrentLevel}";
            currentExp_TXT.text = $"{level.CurrentLevelExperience} / {level.CurrentExperienceNeeded}";
            
            expBar.fillAmount = (float) level.CurrentLevelExperience/ level.CurrentExperienceNeeded;
            nextLevelExp_TXT.text = $"Next Level: {level.ExperienceNeededToLevelUp}";

            var _weapon = inventory.WeaponEquipped;
            var _armor = inventory.ArmorEquipped;
            
            if (_weapon != null)
            {
                weaponName_TXT.text = _weapon.Data.ItemName;
                weaponIcon.sprite = _weapon.Data.Icon;
                weaponIcon.color = Color.white;
                weaponStatsInfo.DisplayWeapon(_weapon.Stats);
            }
            else
            {
                weaponStatsInfo.DisplayWeapon(new CombatStats());
                weaponIcon.sprite = null;
                weaponIcon.color = Color.clear;
                weaponName_TXT.text = "No Weapon Equipped";
            }

            if (_armor != null)
            {
                armor_TXT.text = _armor.Data.ItemName;
                armorIcon.sprite = _armor.Data.Icon;
                armorIcon.color = Color.white;
                armorStatsInfo.DisplayArmor(_armor.Stats);
            }
            else
            {
                armorStatsInfo.DisplayArmor(new CombatStats());
                armorIcon.sprite = null;
                armorIcon.color = Color.clear;
                armor_TXT.text = "No Armor Equipped";
            }
            
            totalStatsInfo.DisplayCharacterStats(playerData.statsData, playerData.level);
        }
        
        
    }
}
