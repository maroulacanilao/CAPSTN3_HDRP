using System;
using Character;
using CustomHelpers;
using Fungus;
using Items.ItemData;
using ScriptableObjectData;
using ScriptableObjectData.CharacterData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.ShrineUI.StatStore
{
    public class ShrineStatsDisplay : MonoBehaviour
    {
        [SerializeField] private GameDataBase  gameDataBase;
        
        [SerializeField] private TextMeshProUGUI vitality_TXT, strength_TXT, intelligence_TXT, defense_TXT, speed_TXT;
        [SerializeField] private TextMeshProUGUI vitalityBought_TXT, strengthBought_TXT, intelligenceBought_TXT, defenseBought_TXT, speedBought_TXT;
        [SerializeField] private Image vitality_IMG, strength_IMG, intelligence_IMG, defense_IMG, speed_IMG;
        [SerializeField] private TextMeshProUGUI vitalityLabel, strengthLabel, intelligenceLabel, defenseLabel, speedLabel;
        [SerializeField] private Color selectColor;
        [SerializeField] private Color originalColor = Color.white;

        private PlayerData playerData => gameDataBase.playerData;
        private CombatStats baseStats => playerData.statsData.GetLeveledStats(playerData.level) + playerData.statsData.equipmentStats;
        CombatStats statsBought => playerData.statsData.additionalStats;
        
        private Vector3 vitalityLabelScale, strengthLabelScale, intelligenceLabelScale, defenseLabelScale, speedLabelScale;

        private void Awake()
        {
            vitality_IMG.sprite = StatType.Health.GetSpriteIcon();
            strength_IMG.sprite = StatType.Strength.GetSpriteIcon();
            intelligence_IMG.sprite = StatType.Intelligence.GetSpriteIcon();
            defense_IMG.sprite = StatType.Defense.GetSpriteIcon();
            speed_IMG.sprite = StatType.Speed.GetSpriteIcon();
            
            vitalityLabelScale = vitalityLabel.transform.localScale;
            strengthLabelScale = strengthLabel.transform.localScale;
            intelligenceLabelScale = intelligenceLabel.transform.localScale;
            defenseLabelScale = defenseLabel.transform.localScale;
            speedLabelScale = speedLabel.transform.localScale;
        }

        private void OnEnable()
        {
            UpdateDisplay();
        }

        public void UpdateDisplay()
        {
            vitality_TXT.text = baseStats.vitality.ToString();
            strength_TXT.text = baseStats.strength.ToString();
            intelligence_TXT.text = baseStats.intelligence.ToString();
            defense_TXT.text = baseStats.defense.ToString();
            speed_TXT.text = baseStats.speed.ToString();

            vitalityBought_TXT.text = "+" + statsBought.vitality.ToString();
            strengthBought_TXT.text = "+" + statsBought.strength.ToString();
            intelligenceBought_TXT.text = "+" + statsBought.intelligence.ToString();
            defenseBought_TXT.text = "+" + statsBought.defense.ToString();
            speedBought_TXT.text = "+" + statsBought.speed.ToString();
        }
        
        private void ResetScale()
        {
            vitalityLabel.transform.localScale = vitalityLabelScale;
            strengthLabel.transform.localScale = strengthLabelScale;
            intelligenceLabel.transform.localScale = intelligenceLabelScale;
            defenseLabel.transform.localScale = defenseLabelScale;
            speedLabel.transform.localScale = speedLabelScale;
            
            vitalityLabel.color = originalColor;
            strengthLabel.color = originalColor;
            intelligenceLabel.color = originalColor;
            defenseLabel.color = originalColor;
            speedLabel.color = originalColor;
        }

        private void Enlarge(StatType statType_)
        {
            ResetScale();
            switch (statType_)
            {
                case StatType.Health:
                    vitalityLabel.transform.localScale = vitalityLabelScale * 1.2f;
                    vitalityLabel.color = selectColor;
                    break;

                case StatType.Strength:
                    strengthLabel.transform.localScale = strengthLabelScale * 1.2f;
                    strengthLabel.color = selectColor;
                    break;
                case StatType.Intelligence:
                    intelligenceLabel.transform.localScale = intelligenceLabelScale * 1.2f;
                    intelligenceLabel.color = selectColor;
                    break;
                case StatType.Defense:
                    defenseLabel.transform.localScale = defenseLabelScale * 1.2f;
                    defenseLabel.color = selectColor;
                    break;
                case StatType.Speed:
                    speedLabel.transform.localScale = speedLabelScale * 1.2f;
                    speedLabel.color = selectColor;
                    break;
                default:
                    ResetScale();
                    break;
            }
        }

        private void FixedUpdate()
        {
            var _btn = ShrineStatBtn.CurrentSelected;
            
            if (_btn.IsEmptyOrDestroyed())
            {
                ResetScale();
                return;
            }
            
            var _item = _btn.item;
            
            if (_item == null)
            {
                ResetScale();
                return;
            }
            
            var _data = _item.Data as ConsumableData;
            
            if(_data == null)
            {
                ResetScale();
                return;
            }
            
            Enlarge(_data.GetStatType());
        }
    }
}