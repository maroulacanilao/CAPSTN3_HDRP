using System;
using BaseCore;
using Character;
using CustomHelpers;
using DG.Tweening;
using Managers;
using NaughtyAttributes;
using ScriptableObjectData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class StatsInfo : MonoBehaviour
    {
        [BoxGroup("Vitality")] [SerializeField] private TextMeshProUGUI vitTXT;
        [BoxGroup("Vitality")][SerializeField] private Image vitIcon;
        [BoxGroup("Vitality")] [SerializeField] private GameObject vitPanel;
        
        [BoxGroup("Strength")] [SerializeField] private TextMeshProUGUI strTXT;
        [BoxGroup("Strength")][SerializeField] private Image strIcon;
        [BoxGroup("Strength")] [SerializeField] private GameObject strPanel;
        
        [BoxGroup("Intelligence")] [SerializeField] private TextMeshProUGUI intTXT;
        [BoxGroup("Intelligence")][SerializeField] private Image intIcon;
        [BoxGroup("Intelligence")] [SerializeField] private GameObject intPanel;
        
        [BoxGroup("Defense")] [SerializeField] private TextMeshProUGUI defTXT;
        [BoxGroup("Defense")][SerializeField] private Image defIcon;
        [BoxGroup("Defense")] [SerializeField] private GameObject defPanel;
        
        [BoxGroup("Speed")] [SerializeField] private TextMeshProUGUI spdTXT;
        [BoxGroup("Speed")][SerializeField] private Image spdIcon;
        [BoxGroup("Speed")] [SerializeField] private GameObject spdPanel;
        
        [BoxGroup("HoverText")] [SerializeField] bool showHoverText;
        [BoxGroup("HoverText")] [ShowIf("showHoverText")] [ResizableTextArea] [SerializeField] string vitHoverText;
        [BoxGroup("HoverText")] [ShowIf("showHoverText")] [ResizableTextArea] [SerializeField] string strHoverText;
        [BoxGroup("HoverText")] [ShowIf("showHoverText")] [ResizableTextArea] [SerializeField] string intHoverText;
        [BoxGroup("HoverText")] [ShowIf("showHoverText")] [ResizableTextArea] [SerializeField] string defHoverText;
        [BoxGroup("HoverText")] [ShowIf("showHoverText")] [ResizableTextArea] [SerializeField] string spdHoverText;
        

        [SerializeField] private AssetDataBase assetDatabase;

        private void OnValidate()
        {
            // if(assetDatabase == null) return;
            //
            // if (vitPanel != null)
            // {
            //     vitTXT = vitPanel.GetComponentInChildren<TextMeshProUGUI>();
            //     vitIcon = vitPanel.GetComponentInChildren<Image>();
            //     if(vitIcon != null) vitIcon.sprite = assetDatabase.vitalityIcon;
            // }
            //
            // if (strPanel != null)
            // {
            //     strTXT = strPanel.GetComponentInChildren<TextMeshProUGUI>();
            //     strIcon = strPanel.GetComponentInChildren<Image>();
            //     if(strIcon != null) strIcon.sprite = assetDatabase.phyDmgIcon;
            // }
            //
            // if (intPanel != null)
            // {
            //     intTXT = intPanel.GetComponentInChildren<TextMeshProUGUI>();
            //     intIcon = intPanel.GetComponentInChildren<Image>();
            //     if(intIcon != null) intIcon.sprite = assetDatabase.intelligenceIcon;
            // }
            //
            // if (defPanel != null)
            // {
            //     defTXT = defPanel.GetComponentInChildren<TextMeshProUGUI>();
            //     defIcon = defPanel.GetComponentInChildren<Image>();
            //     if(defIcon != null) defIcon.sprite = assetDatabase.defIcon;
            // }
            //
            // if (spdPanel != null)
            // {
            //     spdTXT = spdPanel.GetComponentInChildren<TextMeshProUGUI>();
            //     spdIcon = spdPanel.GetComponentInChildren<Image>();
            //     if(spdIcon != null) spdIcon.sprite = assetDatabase.spdIcon;
            // }
            //

        }

        // [Button("Set Text")]
        private void SetHoverText()
        {
            // if(!showHoverText) return;
            //
            // if (vitIcon != null)
            // {
            //     var _hover = vitIcon.gameObject.GetOrAddComponent<HoverTextTip>();
            //     _hover.SetMessage(vitHoverText);
            //     _hover.willShowTip = true;
            // }
            // if (strIcon != null)
            // {
            //     var _hover = strIcon.gameObject.GetOrAddComponent<HoverTextTip>();
            //     _hover.SetMessage(strHoverText);
            //     _hover.willShowTip = true;
            // }
            // if (intIcon != null)
            // {
            //     var _hover = intIcon.gameObject.GetOrAddComponent<HoverTextTip>();
            //     _hover.SetMessage(intHoverText);
            //     _hover.willShowTip = true;
            // }
            // if (defIcon != null)
            // {
            //     var _hover = defIcon.gameObject.GetOrAddComponent<HoverTextTip>();
            //     _hover.SetMessage(defHoverText);
            //     _hover.willShowTip = true;
            // }
            // if (spdIcon != null)
            // {
            //     var _hover = spdIcon.gameObject.GetOrAddComponent<HoverTextTip>();
            //     _hover.SetMessage(spdHoverText);
            //     _hover.willShowTip = true;
            // }
        }
        
        private void Awake()
        {
            if (assetDatabase == null) assetDatabase = GameManager.Instance.GameDataBase.assetDataBase;

            if (vitIcon != null) vitIcon.sprite = StatType.Health.GetSpriteIcon();
            if (strIcon != null) strIcon.sprite = StatType.Strength.GetSpriteIcon();
            if (intIcon != null) intIcon.sprite = StatType.Intelligence.GetSpriteIcon();
            if (defIcon != null) defIcon.sprite = StatType.Defense.GetSpriteIcon();
            if (spdIcon != null) spdIcon.sprite = StatType.Speed.GetSpriteIcon();
            
            // if(!showHoverText) return;
            //
            // if (vitIcon != null) vitIcon.gameObject.GetOrAddComponent<HoverTextTip>().SetMessage(vitHoverText);
            // if (strIcon != null) strIcon.gameObject.GetOrAddComponent<HoverTextTip>().SetMessage(strHoverText);
            // if (intIcon != null) intIcon.gameObject.GetOrAddComponent<HoverTextTip>().SetMessage(intHoverText);
            // if (defIcon != null) defIcon.gameObject.GetOrAddComponent<HoverTextTip>().SetMessage(defHoverText);
            // if (spdIcon != null) spdIcon.gameObject.GetOrAddComponent<HoverTextTip>().SetMessage(spdHoverText);
        }

        public void Display(CombatStats combatStats_, bool includeVitality_ = true)
        {
            if (!gameObject.activeInHierarchy) gameObject.SetActive(true);
            if (vitPanel != null) vitPanel.SetActive(includeVitality_);

            // TODO: Remove label
            if (includeVitality_) vitTXT.text =  $"Vit: {combatStats_.vitality}";
            strTXT.text =  $"STR: {combatStats_.strength}";
            defTXT.text =  $"Def: {combatStats_.defense}";
            intTXT.text =  $"INT: {combatStats_.intelligence}";
            spdTXT.text =  $"SPD: {combatStats_.speed}";
        }

        public void DisplayDynamic(CombatStats combatStats_, bool includeVitality_ = true)
        {
            if (!gameObject.activeSelf) gameObject.SetActive(true);
            
            RemoveDynamic(combatStats_);
            
            Display(combatStats_, includeVitality_);
        }
        
        public void DisplayDiffDynamic(CombatStats combatStats_, CombatStats oldCombatStats_, bool includeVitality_ = true)
        {
            if (!gameObject.activeSelf) gameObject.SetActive(true);
            
            RemoveDynamic(combatStats_);
            
            Display(combatStats_, includeVitality_);
            var _diff = combatStats_ - oldCombatStats_;
            
            if(_diff.strength != 0) strTXT.text += _diff.strength > 0? $" <color=green>(+{_diff.strength})</color>" : $" <color=red>({_diff.strength})</color>";
            if(_diff.defense != 0) defTXT.text += _diff.defense > 0? $" <color=green>(+{_diff.defense})</color>" : $" <color=red>({_diff.defense})</color>";
            if(_diff.intelligence != 0) intTXT.text += _diff.intelligence > 0? $" <color=green>(+{_diff.intelligence})</color>" : $" <color=red>({_diff.intelligence})</color>";
            if(_diff.speed != 0) spdTXT.text += _diff.speed > 0? $" <color=green>(+{_diff.speed})</color>" : $" <color=red>({_diff.speed})</color>";
            if(_diff.vitality != 0) vitTXT.text += _diff.vitality > 0? $" <color=green>(+{_diff.vitality})</color>" : $" <color=red>({_diff.vitality})</color>";
        }
        
        public void DisplayArmor(CombatStats combatStats_, bool includeVitality_ = false)
        {
            if (!gameObject.activeSelf) gameObject.SetActive(true);
            if (vitPanel != null) vitPanel.SetActive(includeVitality_);
            if(strPanel != null) strPanel.SetActive(false);
            if(intPanel != null) intPanel.SetActive(false);
            
            Display(combatStats_, includeVitality_);
        }
        
        public void DisplayWeapon(CombatStats combatStats_)
        {
            if (!gameObject.activeSelf) gameObject.SetActive(true);
            if (vitPanel != null) vitPanel.SetActive(false);
            if(defPanel != null) defPanel.SetActive(false);
            if(spdPanel != null) spdPanel.SetActive(false);
            
            Display(combatStats_, false);
        }
        
        public void DisplayIncreaseDynamic(CombatStats combatStats_, bool doReveal_)
        {
            if (!gameObject.activeInHierarchy) gameObject.SetActive(true);
            
            if(vitTXT != null) vitTXT.gameObject.SetActive(false);
            if(strTXT != null) strTXT.gameObject.SetActive(false);
            if(intTXT != null) intTXT.gameObject.SetActive(false);
            if(spdTXT != null) spdTXT.gameObject.SetActive(false);
            if(defTXT != null) defTXT.gameObject.SetActive(false);
            
            if(vitPanel != null) vitPanel.SetActive(false);
            if(strPanel != null) strPanel.SetActive(false);
            if(intPanel != null) intPanel.SetActive(false);
            if(spdPanel != null) spdPanel.SetActive(false);
            if(defPanel != null) defPanel.SetActive(false);


            // TODO: Remove label
            if (vitTXT != null && combatStats_.vitality != 0)
            {
                vitTXT.text = "MaxHP: +" + combatStats_.vitality.ToString();
                vitTXT.gameObject.SetActive(true);
            }
            if (strTXT != null && combatStats_.strength != 0)
            {
                strTXT.text = "Damage: +" + combatStats_.strength.ToString();
                strTXT.gameObject.SetActive(true);
            }
            if(intTXT != null && combatStats_.intelligence != 0)
            {
                intTXT.text = "MaxMp: +" + combatStats_.intelligence.ToString();
                intTXT.gameObject.SetActive(true);
            }
            if (defTXT != null && combatStats_.defense != 0)
            {
                defTXT.text = "Armor: +" + combatStats_.defense.ToString();
                defTXT.gameObject.SetActive(true);
            }
            if(spdTXT != null && combatStats_.speed != 0)
            {
                spdTXT.text = "Speed: +" + combatStats_.speed.ToString();
                spdTXT.gameObject.SetActive(true);
            }

            if(doReveal_) DoReveal(combatStats_);
        }

        private async void DoReveal(CombatStats combatStats_)
        {
            vitTXT.alpha = 0;
            intTXT.alpha = 0;
            strTXT.alpha = 0;
            defTXT.alpha = 0;
            spdTXT.alpha = 0;

            var _endVal = 255f;
            var _duration = 0.5f;
            
            if (vitPanel != null && combatStats_.vitality != 0)
            {
                vitTXT.text = "MaxHP: +" + combatStats_.vitality.ToString();
                await vitTXT.DOFade(_endVal, _duration).SetUpdate(true).AsyncWaitForCompletion();
                vitPanel.SetActive(true);
            }
            
            if (strPanel != null && combatStats_.strength != 0)
            {
                strTXT.text = "Damage: +" + combatStats_.strength.ToString();
                await strTXT.DOFade(_endVal, _duration).SetUpdate(true).AsyncWaitForCompletion();
                strPanel.SetActive(true);
            }
            
            if(intPanel != null && combatStats_.intelligence != 0)
            {
                intTXT.text = "MaxMp: +" + combatStats_.intelligence.ToString();
                await intTXT.DOFade(_endVal, _duration).SetUpdate(true).AsyncWaitForCompletion();
                intPanel.SetActive(true);
            }
            
            if (defPanel != null && combatStats_.defense != 0)
            {
                defTXT.text = "Armor: +" + combatStats_.defense.ToString();
                await defTXT.DOFade(_endVal, _duration).SetUpdate(true).AsyncWaitForCompletion();
                defPanel.SetActive(true);
            }
            
            if(spdPanel != null && combatStats_.speed != 0)
            {
                spdTXT.text = "Speed: +" + combatStats_.speed.ToString();
                await spdTXT.DOFade(_endVal, _duration).SetUpdate(true).AsyncWaitForCompletion();
                spdPanel.SetActive(true);
            }
        }

        private void RemoveDynamic(CombatStats stats_)
        {
            vitPanel.SetActive(stats_.vitality != 0);
            strPanel.SetActive(stats_.strength != 0);
            intPanel.SetActive(stats_.intelligence != 0);
            defPanel.SetActive(stats_.defense != 0);
            spdPanel.SetActive(stats_.speed != 0);
        }

        public void DisplayCharacterStats(StatsGrowth statsGrowth_, int level_, bool includeVitality_ = false)
        {
            var _base = statsGrowth_.GetTotalNonBonusStats(level_);
            var _bonus = statsGrowth_.bonusStats;
            
            if (!gameObject.activeInHierarchy) gameObject.SetActive(true);
            if (vitPanel != null) vitPanel.SetActive(includeVitality_);

            // TODO: Remove label
            if (includeVitality_) vitTXT.text =  $"VIT: {GetCharacterStatText(_base.vitality, _bonus.vitality)}";
            strTXT.text =  $"STR: {GetCharacterStatText(_base.strength, _bonus.strength)}";
            defTXT.text =  $"DEF: {GetCharacterStatText(_base.defense, _bonus.defense)}";
            intTXT.text =  $"INT: {GetCharacterStatText(_base.intelligence, _bonus.intelligence)}";
            spdTXT.text =  $"SPD: {GetCharacterStatText(_base.speed, _bonus.speed)}";
        }
        
        private string GetCharacterStatText(int statVal_, int bonusStatVal_)
        {
            var _sb = new System.Text.StringBuilder();
            _sb.Append(statVal_);
            if(bonusStatVal_ != 0) _sb.Append($" <color=green>(+{bonusStatVal_})</color>");
            return _sb.ToString();
        }
    }
}
