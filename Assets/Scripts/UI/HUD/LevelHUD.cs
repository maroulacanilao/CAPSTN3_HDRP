using System;
using System.Collections;
using BaseCore;
using CustomHelpers;
using DG.Tweening;
using ScriptableObjectData.CharacterData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.HUD
{
    public class LevelHUD : MonoBehaviour
    {
        [SerializeField] private PlayerData playerData;
        [SerializeField] private TextMeshProUGUI levelText, expText;
        [SerializeField] private Image expBar, expBarBG;
        [SerializeField] private float effectDuration = 0.5f;

        private PlayerLevel lvlData => playerData.LevelData;

        private Vector3 defaultSize;

        private void Awake()
        {
            defaultSize = levelText.transform.localScale;
        }
        
        private void OnEnable()
        {
            PlayerLevel.OnExperienceChanged.AddListener(UpdateExp);
            DisplayLevel();
        }

        private void OnDisable()
        {
            StopAllCoroutines();
            PlayerLevel.OnExperienceChanged.RemoveListener(UpdateExp);
        }

        private void UpdateExp(PlayerLevel.AddExpInfo addExpInfo_)
        {
            if(this.IsEmptyOrDestroyed()) return;
            if(!gameObject.activeInHierarchy) return;
            StartCoroutine(Co_UpdateExp(addExpInfo_));
        }

        private IEnumerator Co_UpdateExp(PlayerLevel.AddExpInfo addExpInfo_)
        {
            yield return Co_SetOtherObjectActive(true, 0);
            yield return new WaitForSeconds(0.1f);
            var _temp = addExpInfo_.prevScaledLvlExp + addExpInfo_.addedExp;
            _temp = Mathf.Clamp(_temp, 0, addExpInfo_.prevScaledLvlExpNeeded);
            var _fillAmount = _temp / (float) addExpInfo_.prevScaledLvlExpNeeded;

            var _neededExp = addExpInfo_.leveledUp ? addExpInfo_.prevNeededExp : lvlData.NextLevelExperience;

            UpdateExpText(addExpInfo_.prevScaledLvlExp,_temp,addExpInfo_.prevScaledLvlExpNeeded, addExpInfo_.addedExp);
            
            yield return expBar.DOFillAmount(_fillAmount, effectDuration).SetUpdate(true).WaitForCompletion();


            if (!addExpInfo_.leveledUp)
            {
                yield return Co_SetOtherObjectActive(false, 2f);
                yield break;
            }
            
            // if Level Up
            levelText.text = $"{addExpInfo_.prevLevel}";

            var _size = levelText.transform.localScale;
            
            yield return levelText.transform.DOScale(_size*2f, effectDuration/4f)
                .SetUpdate(true).SetEase(Ease.OutBack).WaitForCompletion();
            
            yield return levelText.transform.DOScale(_size, effectDuration/4f)
                .SetUpdate(true).SetEase(Ease.OutBack).WaitForCompletion();
            
            levelText.text = $"{lvlData.CurrentLevel}";
            
            Debug.Log($"Level Up! prevExp : 0 || newExp : {lvlData.CurrentLevelExperience} || neededExp : {lvlData.CurrentExperienceNeeded}");
            
            UpdateExpText(0,lvlData.CurrentLevelExperience,lvlData.CurrentExperienceNeeded,addExpInfo_.addedExp);

            var _temp2 = lvlData.CurrentLevelExperience / (float) lvlData.CurrentExperienceNeeded;
            expBar.fillAmount = 0;
            
            yield return expBar.DOFillAmount(_temp2, effectDuration).SetUpdate(true).WaitForCompletion();
            
            DisplayLevel(false);
            
            yield return Co_SetOtherObjectActive(false, 2f);
        }
        
        private void UpdateExpText(int prevExp_, int newExp_, int neededExp_, int addedExp_)
        {
            var _addedTxt = $" <color=green>+{addedExp_}</color>";
            DOTween.To(() => prevExp_, x => prevExp_ = x, newExp_, effectDuration).OnUpdate(() =>
            {
                expText.text = $"{Mathf.Clamp(prevExp_,0,neededExp_):0} / {neededExp_:0} {_addedTxt}";
            }).SetUpdate(true);
        }
        
        private void DisplayLevel(bool willDisableOtherObjects_ = true)
        {
            levelText.transform.localScale = defaultSize;
            levelText.text = $"{lvlData.CurrentLevel}";
            expText.text = $"{lvlData.CurrentLevelExperience} / {lvlData.CurrentExperienceNeeded}";
            
            expBar.fillAmount = lvlData.CurrentLevelExperience / (float) lvlData.CurrentExperienceNeeded;
            if(willDisableOtherObjects_) StartCoroutine(Co_SetOtherObjectActive(false, 0.5f));
        }
        
        private IEnumerator Co_SetOtherObjectActive(bool willActivate_, float initialDelay_)
        {
            yield return new WaitForSeconds(initialDelay_);
            expText.gameObject.SetActive(willActivate_);
            expBarBG.gameObject.SetActive(willActivate_);
        }
    }
}